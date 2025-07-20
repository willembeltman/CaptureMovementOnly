using CaptureOnlyMovements.Interfaces;
using CaptureOnlyMovements.Types;
using System;

namespace CaptureOnlyMovements.FrameResizers;

public class BgrResizerUnsafe(Resolution Resolution) : IFrameResizer
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
            fixed (byte* previousFramePointer = Buffer)
            fixed (byte* newFramePointer = frame.Buffer)
            {
                for (int y = 0; y < height; y++)
                {
                    for (int x = 0; x < width; x++)
                    {
                        int index = y * outputStride + x * 3;
                        if (x < inputResolution.Width && y < inputResolution.Height)
                        {
                            int inputIndex = y * inputStride + x * 3;
                            previousFramePointer[index + 0] = newFramePointer[inputIndex + 0];
                            previousFramePointer[index + 1] = newFramePointer[inputIndex + 1];
                            previousFramePointer[index + 2] = newFramePointer[inputIndex + 2];
                        }
                        else
                        {
                            previousFramePointer[index] = 0;
                            previousFramePointer[index + 1] = 0;
                            previousFramePointer[index + 2] = 0;
                        }
                    }
                }
            }
        }

        return new Frame(Buffer, Resolution);
    }

    public void Dispose()
    {
        GC.SuppressFinalize(this);
    }
}