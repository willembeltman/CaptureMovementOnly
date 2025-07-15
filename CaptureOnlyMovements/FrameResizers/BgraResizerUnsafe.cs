using CaptureOnlyMovements.Types;

namespace CaptureOnlyMovements.FrameResizers;

public class BgraResizerUnsafe(Resolution Resolution) : IDisposable
{
    private readonly byte[] Buffer = new byte[Resolution.PixelCount * 4];

    public Frame Resize(Frame frame)
    {
        var inputResolution = frame.Resolution;
        if (inputResolution == Resolution)
            return frame;

        var inputStride = inputResolution.Width * 4;
        var outputStride = Resolution.Width * 4;

        unsafe
        {
            fixed (byte* previousFramePointer = Buffer)
            fixed (byte* newFramePointer = frame.Buffer)
            {
                for (int y = 0; y < Resolution.Height; y++)
                {
                    for (int x = 0; x < Resolution.Width; x++)
                    {
                        int index = y * outputStride + x * 4;
                        if (x < inputResolution.Width && y < inputResolution.Height)
                        {
                            int inputIndex = y * inputStride + x * 4;
                            previousFramePointer[index + 0] = newFramePointer[inputIndex + 0];
                            previousFramePointer[index + 1] = newFramePointer[inputIndex + 1];
                            previousFramePointer[index + 2] = newFramePointer[inputIndex + 2];
                            previousFramePointer[index + 3] = newFramePointer[inputIndex + 3];
                        }
                        else
                        {
                            previousFramePointer[index] = 0;
                            previousFramePointer[index + 1] = 0;
                            previousFramePointer[index + 2] = 0;
                            previousFramePointer[index + 3] = 0;
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