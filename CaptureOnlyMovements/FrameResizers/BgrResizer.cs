using CaptureOnlyMovements.Interfaces;
using CaptureOnlyMovements.Types;
using System;

namespace CaptureOnlyMovements.FrameResizers;

public class BgrResizer(Resolution outputResolution) : IBgrResizer
{
    private readonly byte[] Buffer = new byte[outputResolution.PixelCount * 3];

    public Frame Resize(Frame frame)
    {
        var inputResolution = frame.Resolution;
        if (inputResolution == outputResolution)
            return frame;

        var inputStride = inputResolution.Width * 3;
        var outputStride = outputResolution.Width * 3;

        for (int x = 0; x < outputResolution.Width; x++)
        {
            for (int y = 0; y < outputResolution.Height; y++)
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
            }
        }

        return new Frame(Buffer, outputResolution);
    }

    public void Dispose()
    {
        GC.SuppressFinalize(this);
    }
}