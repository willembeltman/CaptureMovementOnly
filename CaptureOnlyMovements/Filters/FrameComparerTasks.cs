using CaptureOnlyMovements.Interfaces;
using CaptureOnlyMovements.Types;

namespace CaptureOnlyMovements.Filters;

public class FrameComparerTasks : IFrameComparer
{
    public FrameComparerTasks(
        IComparerConfig config,
        Resolution resolution,
        IPreview? preview = null)
    {
        Config = config;
        Preview = preview;
        Resolution = resolution;
        CalculationFrameData = new bool[resolution.Width * resolution.Height];
        PreviousFrameData = new byte[resolution.Width * resolution.Height * 3];
        Result_Difference = 0;
    }

    private readonly IComparerConfig Config;
    private readonly IPreview? Preview;
    private readonly byte[] PreviousFrameData;

    public Resolution Resolution { get; }
    public bool[] CalculationFrameData { get; }
    public int Result_Difference { get; private set; }

    public bool IsDifferent(byte[] newFrameData)
    {
        int stride = Resolution.Width * 3;
        int height = Resolution.Height;
        int width = Resolution.Width;
        int maxDiffPixels = Config.MaximumDifferentPixelCount;
        int maxPixelDiff = Config.MaximumPixelDifferenceValue;
        bool showMask = Preview.ShowMask;

        // Reset difference counter
        Result_Difference = 0;

        // Calculate chunk size: lines / (cpu-count * 8)
        int cpuCount = Environment.ProcessorCount;
        int chunkSize = Math.Max(1, height / (cpuCount * 8));
        int chunkCount = (height + chunkSize - 1) / chunkSize; // Ceiling division

        // Create tasks for each chunk
        Task<int>[] tasks = new Task<int>[chunkCount];
        for (int i = 0; i < chunkCount; i++)
        {
            int startY = i * chunkSize;
            int endY = Math.Min(startY + chunkSize, height);

            tasks[i] = Task.Run(() =>
            {
                int localSum = 0;
                for (int y = startY; y < endY; y++)
                {
                    for (int x = 0; x < width; x++)
                    {
                        int index = y * stride + x * 3;

                        // Compute color differences
                        int pixelColorDiff1 = Math.Abs(newFrameData[index] - PreviousFrameData[index]);
                        int pixelColorDiff2 = Math.Abs(newFrameData[index + 1] - PreviousFrameData[index + 1]);
                        int pixelColorDiff3 = Math.Abs(newFrameData[index + 2] - PreviousFrameData[index + 2]);
                        int pixelColorDiff = pixelColorDiff1 + pixelColorDiff2 + pixelColorDiff3;

                        bool isDifferent = pixelColorDiff > maxPixelDiff;

                        // Store mask if needed
                        if (showMask)
                        {
                            CalculationFrameData[y * width + x] = isDifferent;
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

        // Aggregate results
        Task.WaitAll(tasks);
        foreach (var task in tasks)
        {
            Result_Difference += task.Result;
        }

        // Check if frame is different
        bool isFrameDifferent = Result_Difference > maxDiffPixels;

        // Update PreviousFrameData if necessary
        if (isFrameDifferent)
        {
            Array.Copy(newFrameData, PreviousFrameData, newFrameData.Length);
        }

        return isFrameDifferent;
    }
}
