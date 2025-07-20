using CaptureOnlyMovements.Interfaces;
using CaptureOnlyMovements.Types;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace CaptureOnlyMovements.FrameComparers;

public class FrameComparerParallel(
    IComparerConfig config,
    Resolution resolution,
    IPreview? preview = null) : IBgrComparer
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

        var pixelCount = Resolution.Width * Resolution.Height;

        Parallel.For(0, pixelCount, (k, state) =>
        {
            int index = k * 3;

            int diff =
                Math.Abs(newFrameData[index] - PreviousFrameData[index]) +
                Math.Abs(newFrameData[index + 1] - PreviousFrameData[index + 1]) +
                Math.Abs(newFrameData[index + 2] - PreviousFrameData[index + 2]);

            bool isDifferent = diff > config.MaximumPixelDifferenceValue;

            if (preview?.ShowMask == true)
            {
                MaskData[index] = isDifferent;
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
        GC.SuppressFinalize(this);
    }
}
