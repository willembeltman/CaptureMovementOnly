using CaptureOnlyMovements.Interfaces;
using CaptureOnlyMovements.Types;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace CaptureOnlyMovements.FrameComparers;

public class FrameComparerParallelUnsafe(
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

        unsafe
        {
            fixed (byte* previousFramePointerBase = PreviousFrameData)
            fixed (byte* newFramePointerBase = newFrameData)
            fixed (bool* maskFramePointerBase = MaskData)
            {
                var previousFramePointerTransfer = (nint)previousFramePointerBase;
                var newFramePointerTransfer = (nint)newFramePointerBase;
                var maskFramePointerTransfer = (nint)maskFramePointerBase;

                var pixelCount = Resolution.Width * Resolution.Height;
                Parallel.For(0, pixelCount, (k, state) =>
                {
                    var previousFramePointer = (byte*)(previousFramePointerTransfer);
                    var newFramePointer = (byte*)(newFramePointerTransfer);
                    var maskFramePointer = (bool*)(maskFramePointerTransfer);

                    int index = k * 3;

                    int diff =
                        Math.Abs(newFramePointer[index] - previousFramePointer[index]) +
                        Math.Abs(newFramePointer[index + 1] - previousFramePointer[index + 1]) +
                        Math.Abs(newFramePointer[index + 2] - previousFramePointer[index + 2]);

                    bool isDifferent = diff > config.MaximumPixelDifferenceValue;

                    if (preview?.ShowMask == true)
                    {
                        maskFramePointer[index] = isDifferent;
                    }

                    if (isDifferent)
                    {
                        int current = Interlocked.Increment(ref totalDifferent);
                        if (current > config.MaximumDifferentPixelCount)
                        {
                            differenceExceeded = true;
                            if (preview?.ShowMask != true)
                            {
                                state.Stop();
                                return;
                            }
                        }
                    }
                });
            }
        }

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
