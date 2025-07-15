using CaptureOnlyMovements.Interfaces;
using CaptureOnlyMovements.Types;
using System.Runtime.InteropServices;

namespace CaptureOnlyMovements.FrameComparers;

public class FrameComparerUnsafe(
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
        Difference = 0;
        bool differenceExceeded = false;

        unsafe
        {
            fixed (byte* previousFramePointer = PreviousFrameData)
            fixed (byte* newFramePointer = newFrameData)
            fixed (bool* maskFramePointer = MaskData)
            {
                var pixelCount = Resolution.Width * Resolution.Height;
                for (int k = 0; k < pixelCount; k++)
                {
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
                        Difference++;
                        if (Difference > config.MaximumDifferentPixelCount)
                        {
                            differenceExceeded = true;
                            if (preview?.ShowMask != true)
                            {
                                break;
                            }
                        }
                    }
                }

                if (differenceExceeded)
                {
                    Marshal.Copy(newFrameData, 0, new IntPtr(previousFramePointer), newFrameData.Length);
                    return true;
                }
            }
        }
        return false;
    }

    public void Dispose()
    {
    }
}
