using CaptureOnlyMovements.Interfaces;
using CaptureOnlyMovements.Types;
using System;

namespace CaptureOnlyMovements.FrameComparers;

public class FrameComparerParallelUnsafe(
    IComparerConfig config,
    Resolution resolution,
    IPreview? preview = null) : IFrameComparer
{
    private readonly byte[] PreviousFrameData = new byte[resolution.Width * resolution.Height * 3];

    public Resolution Resolution { get; } = resolution;
    public bool[] MaskData { get; } = new bool[resolution.Width * resolution.Height];
    public int Difference { get; private set; }

    public bool IsDifferent(byte[] newFrameData)
    {
        int stride = Resolution.Width * 3;
        int totalDifferent = 0;
        bool differenceExceeded = false;

        Parallel.For(0, Resolution.Height, (y, state) =>
        {
            unsafe
            {
                fixed (byte* previousFrameDataPointer = PreviousFrameData)
                fixed (byte* newFrameDataPointer = newFrameData)
                fixed (bool* maskDataPointer = MaskData)
                {
                    for (int x = 0; x < Resolution.Width; x++)
                    {
                        int index = y * stride + x * 3;

                        int diff =
                            Math.Abs(previousFrameDataPointer[index] - newFrameDataPointer[index]) +
                            Math.Abs(previousFrameDataPointer[index + 1] - newFrameDataPointer[index + 1]) +
                            Math.Abs(previousFrameDataPointer[index + 2] - newFrameDataPointer[index + 2]);

                        bool isDifferent = diff > config.MaximumPixelDifferenceValue;

                        if (preview?.ShowMask == true)
                        {
                            maskDataPointer[y * Resolution.Width + x] = isDifferent;
                        }

                        if (isDifferent)
                        {
                            int current = Interlocked.Increment(ref totalDifferent);
                            if (current > config.MaximumDifferentPixelCount)
                            {
                                differenceExceeded = true;
                                if (preview?.ShowMask != true)
                                {
                                    state.Stop(); // Abort parallel execution early
                                    return;
                                }
                            }
                        }
                    }
                }
            }
        });

        Difference = totalDifferent;

        if (differenceExceeded)
        {
            Array.Copy(newFrameData, PreviousFrameData, newFrameData.Length);
            return true;
        }

        return false;
    }

    public void Dispose()
    {
    }
}
