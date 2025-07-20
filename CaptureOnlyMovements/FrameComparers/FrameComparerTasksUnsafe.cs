using CaptureOnlyMovements.Interfaces;
using CaptureOnlyMovements.Types;
using System;
using System.Threading.Tasks;

namespace CaptureOnlyMovements.FrameComparers;

public class FrameComparerTasksUnsafe(
    IComparerConfig config,
    Resolution resolution,
    IPreview? preview = null) : IBgrComparer
{
    private readonly byte[] PreviousFrameData = new byte[resolution.Width * resolution.Height * 3];

    public Resolution Resolution { get; } = resolution;
    public bool[] MaskData { get; } = new bool[resolution.Width * resolution.Height];
    public int Difference { get; private set; } = 0;

    public bool IsDifferent(byte[] newFrameData)
    {
        int stride = Resolution.Width * 3;
        int height = Resolution.Height;
        int width = Resolution.Width;
        int maxDiffPixels = config.MaximumDifferentPixelCount;
        int maxPixelDiff = config.MaximumPixelDifferenceValue;
        bool showMask = preview?.ShowMask == true;

        // Reset difference counter
        Difference = 0;

        // Calculate chunk size: lines / (cpu-count * 8)
        int cpuCount = Environment.ProcessorCount;
        int chunkSize = Math.Max(1, height / (cpuCount * 8));
        int chunkCount = (height + chunkSize - 1) / chunkSize; // Ceiling division

        unsafe
        {
            fixed (byte* previousFramePointerBase = PreviousFrameData)
            fixed (byte* newFramePointerBase = newFrameData)
            fixed (bool* maskFramePointerBase = MaskData)
            {
                var previousFramePointerTransfer = (nint)previousFramePointerBase;
                var newFramePointerTransfer = (nint)newFramePointerBase;
                var maskFramePointerTransfer = (nint)maskFramePointerBase;

                // Create tasks for each chunk
                Task<int>[] tasks = new Task<int>[chunkCount];
                for (int i = 0; i < chunkCount; i++)
                {
                    int startY = i * chunkSize;
                    int endY = Math.Min(startY + chunkSize, height);

                    tasks[i] = Task.Run(() =>
                    {
                        var previousFramePointer = (byte*)(previousFramePointerTransfer);
                        var newFramePointer = (byte*)(newFramePointerTransfer);
                        var maskFramePointer = (bool*)(maskFramePointerTransfer);

                        int localSum = 0;
                        for (int y = startY; y < endY; y++)
                        {
                            for (int x = 0; x < width; x++)
                            {
                                int index = y * stride + x * 3;

                                // Compute color differences
                                int pixelColorDiff1 = Math.Abs(newFramePointer[index] - previousFramePointer[index]);
                                int pixelColorDiff2 = Math.Abs(newFramePointer[index + 1] - previousFramePointer[index + 1]);
                                int pixelColorDiff3 = Math.Abs(newFramePointer[index + 2] - previousFramePointer[index + 2]);
                                int pixelColorDiff = pixelColorDiff1 + pixelColorDiff2 + pixelColorDiff3;

                                bool isDifferent = pixelColorDiff > maxPixelDiff;

                                // Store mask if needed
                                if (showMask)
                                {
                                    maskFramePointer[y * width + x] = isDifferent;
                                }

                                // Increment local sum if pixel is different
                                if (isDifferent)
                                {
                                    localSum++;
                                }
                            }
                        }
                        return localSum;
                    });
                }

                Task.WaitAll(tasks);

                // Aggregate results
                foreach (var task in tasks)
                {
                    Difference += task.Result;
                }
            }
        }

        // Check if frame is different
        bool isFrameDifferent = Difference > maxDiffPixels;

        // Update PreviousFrameData if necessary
        if (isFrameDifferent)
        {
            Array.Copy(newFrameData, PreviousFrameData, newFrameData.Length);
        }

        return isFrameDifferent;
    }

    public void Dispose()
    {
        GC.SuppressFinalize(this);
    }
}
