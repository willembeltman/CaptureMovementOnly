using CaptureOnlyMovements.Interfaces;
using CaptureOnlyMovements.Types;
using System;
using System.Threading.Tasks;

namespace CaptureOnlyMovements.FrameResizers;

public class BgrResizerParallelUnsafe(Resolution Resolution) : IBgrResizer
{
    private readonly byte[] Buffer = new byte[Resolution.PixelCount * 3];

    public Frame Resize(Frame frame)
    {
        var width = Resolution.Width;
        var height = Resolution.Height;

        var inputResolution = frame.Resolution;
        if (inputResolution == Resolution)
            return frame;

        var inputStride = inputResolution.Width * 3;
        var outputStride = Resolution.Width * 3;

        unsafe
        {
            fixed (byte* previousFramePointerBase = Buffer)
            fixed (byte* newFramePointerBase = frame.Buffer)
            {
                var previousFramePointerTransfer = (nint)previousFramePointerBase;
                var newFramePointerTransfer = (nint)newFramePointerBase;

                Parallel.For(0, height, (y, state) =>
                {
                    Parallel.For(0, width, (x, state) =>
                    {
                        var previousFramePointer = (byte*)(previousFramePointerTransfer);
                        var newFramePointer = (byte*)(newFramePointerTransfer);

                        int outputIndex = y * outputStride + x * 3;
                        if (x < inputResolution.Width && y < inputResolution.Height)
                        {
                            int inputIndex = y * inputStride + x * 3;
                            previousFramePointer[outputIndex + 0] = newFramePointer[inputIndex + 0];
                            previousFramePointer[outputIndex + 1] = newFramePointer[inputIndex + 1];
                            previousFramePointer[outputIndex + 2] = newFramePointer[inputIndex + 2];
                        }
                        else
                        {
                            previousFramePointer[outputIndex] = 0;
                            previousFramePointer[outputIndex + 1] = 0;
                            previousFramePointer[outputIndex + 2] = 0;
                        }
                    });
                });
            }
        }

        return new Frame(Buffer, Resolution);
    }

    public void Dispose()
    {
        GC.SuppressFinalize(this);
    }
}