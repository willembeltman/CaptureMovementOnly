//using CaptureOnlyMovements.Interfaces;
//using CaptureOnlyMovements.Types;

//namespace CaptureOnlyMovements.FrameComparers;

//public class FrameComparerParallelUnsafe(
//    IComparerConfig config,
//    Resolution resolution,
//    IPreview? preview = null) : IFrameComparer
//{
//    private readonly byte[] PreviousFrameData = new byte[resolution.Width * resolution.Height * 3];

//    public Resolution Resolution { get; } = resolution;
//    public bool[] MaskData { get; } = new bool[resolution.Width * resolution.Height];
//    public int Difference { get; private set; }

//    public bool IsDifferent(byte[] newFrameData)
//    {
//        var width = Resolution.Width;
//        var height = Resolution.Height;
//        var stride = width * 3;
//        var totalDifferent = 0;
//        var differenceExceeded = false;

//        unsafe
//        {
//            fixed (byte* previousFramePointerBase = PreviousFrameData)
//            fixed (byte* newFramePointerBase = newFrameData)
//            fixed (bool* maskFramePointerBase = MaskData)
//            {
//                var previousFramePointerTransfer = (nint)previousFramePointerBase;
//                var newFramePointerTransfer = (nint)newFramePointerBase;
//                var maskFramePointerTransfer = (nint)maskFramePointerBase;

//                Parallel.For(0, height, (y, state) =>
//                {
//                    var previousFramePointer = (byte*)(previousFramePointerTransfer);
//                    var newFramePointer = (byte*)(newFramePointerTransfer);
//                    var maskFramePointer = (bool*)(maskFramePointerTransfer);

//                    for (int x = 0; x < width; x++)
//                    {
//                        var index = y * stride + x * 3;

//                        var diff =
//                            Math.Abs(previousFramePointer[index] - newFramePointer[index]) +
//                            Math.Abs(previousFramePointer[index + 1] - newFramePointer[index + 1]) +
//                            Math.Abs(previousFramePointer[index + 2] - newFramePointer[index + 2]);

//                        var isDifferent = diff > config.MaximumPixelDifferenceValue;

//                        if (preview?.ShowMask == true)
//                        {
//                            maskFramePointer[y * width + x] = isDifferent;
//                        }

//                        if (isDifferent)
//                        {
//                            var current = Interlocked.Increment(ref totalDifferent);
//                            if (current > config.MaximumDifferentPixelCount)
//                            {
//                                differenceExceeded = true;
//                                if (preview?.ShowMask != true)
//                                {
//                                    state.Stop(); // Abort parallel execution early
//                                    return;
//                                }
//                            }
//                        }
//                    }
//                });
//            }
//        }

//        Difference = totalDifferent;

//        if (differenceExceeded)
//        {
//            Array.Copy(newFrameData, PreviousFrameData, newFrameData.Length);
//            return true;
//        }

//        return false;
//    }

//    public void Dispose()
//    {
//    }
//}
