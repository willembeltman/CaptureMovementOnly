//using CaptureOnlyMovements.Types;

//namespace CaptureOnlyMovements.FrameResizers;

//public class BgraResizerParallelUnsafe(Resolution Resolution) : IDisposable
//{
//    private readonly byte[] Buffer = new byte[Resolution.PixelCount * 4];

//    public Frame Resize(Frame frame)
//    {
//        var width = Resolution.Width;
//        var height = Resolution.Height;

//        var inputResolution = frame.Resolution;
//        if (inputResolution == Resolution)
//            return frame;

//        var inputStride = inputResolution.Width * 4;
//        var outputStride = Resolution.Width * 4;

//        unsafe
//        {
//            fixed (byte* previousFramePointerBase = Buffer)
//            fixed (byte* newFramePointerBase = frame.Buffer)
//            {
//                var previousFramePointerTransfer = (nint)previousFramePointerBase;
//                var newFramePointerTransfer = (nint)newFramePointerBase;

//                Parallel.For(0, height, (y, state) =>
//                {
//                    Parallel.For(0, width, (x, state) =>
//                    {
//                        var previousFramePointer = (byte*)(previousFramePointerTransfer);
//                        var newFramePointer = (byte*)(newFramePointerTransfer);

//                        int outputIndex = y * outputStride + x * 4;
//                        if (x < inputResolution.Width && y < inputResolution.Height)
//                        {
//                            int inputIndex = y * inputStride + x * 4;
//                            previousFramePointer[outputIndex + 0] = newFramePointer[inputIndex + 0];
//                            previousFramePointer[outputIndex + 1] = newFramePointer[inputIndex + 1];
//                            previousFramePointer[outputIndex + 2] = newFramePointer[inputIndex + 2];
//                            previousFramePointer[outputIndex + 3] = newFramePointer[inputIndex + 3];
//                        }
//                        else
//                        {
//                            previousFramePointer[outputIndex] = 0;
//                            previousFramePointer[outputIndex + 1] = 0;
//                            previousFramePointer[outputIndex + 2] = 0;
//                            previousFramePointer[outputIndex + 3] = 0;
//                        }
//                    });
//                });
//            }
//        }

//        return new Frame(Buffer, Resolution);
//    }

//    public void Dispose()
//    {
//        GC.SuppressFinalize(this);
//    }
//}