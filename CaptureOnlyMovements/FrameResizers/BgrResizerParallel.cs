using CaptureOnlyMovements.Interfaces;
using CaptureOnlyMovements.Types;
using System;
using System.Threading.Tasks;

namespace CaptureOnlyMovements.FrameResizers;

public class BgrResizerParallel(Resolution Resolution) : IBgrResizer
{
    private readonly byte[] Buffer = new byte[Resolution.PixelCount * 3];

    public Frame Resize(Frame frame)
    {
        var inputResolution = frame.Resolution;
        if (inputResolution == Resolution)
            return frame;

        var inputStride = inputResolution.Width * 3;
        var outputStride = Resolution.Width * 3;

        Parallel.For(0, Resolution.Height, (y, state) =>
        {
            Parallel.For(0, Resolution.Width, (x, state) =>
            {
                int outputIndex = y * outputStride + x * 3;
                if (x < inputResolution.Width && y < inputResolution.Height)
                {
                    int inputIndex = y * inputStride + x * 3;
                    Buffer[outputIndex] = frame.Buffer[inputIndex];
                    Buffer[outputIndex + 1] = frame.Buffer[inputIndex + 1];
                    Buffer[outputIndex + 2] = frame.Buffer[inputIndex + 2];
                }
                else
                {
                    Buffer[outputIndex] = 0;
                    Buffer[outputIndex + 1] = 0;
                    Buffer[outputIndex + 2] = 0;
                }
            });
        });

        return new Frame(Buffer, Resolution);
    }

    public void Dispose()
    {
        GC.SuppressFinalize(this);
    }
}