using CaptureOnlyMovements.Types;

namespace CaptureOnlyMovements.FrameResizers;

public class BgraResizer(Resolution outputResolution) : IDisposable
{
    private readonly byte[] Buffer = new byte[outputResolution.PixelCount * 4];

    public Frame Resize(Frame frame)
    {
        var inputResolution = frame.Resolution;
        if (inputResolution == outputResolution)
            return frame;

        var inputStride = inputResolution.Width * 4;
        var outputStride = outputResolution.Width * 4;

        for (int x = 0; x < outputResolution.Width; x++)
        {
            for (int y = 0; y < outputResolution.Height; y++)
            {
                int outputIndex = y * outputStride + x * 4;
                if (x < inputResolution.Width && y < inputResolution.Height)
                {
                    int inputIndex = y * inputStride + x * 4;
                    Buffer[outputIndex + 0] = frame.Buffer[inputIndex + 0];
                    Buffer[outputIndex + 1] = frame.Buffer[inputIndex + 1];
                    Buffer[outputIndex + 2] = frame.Buffer[inputIndex + 2];
                    Buffer[outputIndex + 3] = frame.Buffer[inputIndex + 3];
                }
                else
                {
                    Buffer[outputIndex] = 0;
                    Buffer[outputIndex + 1] = 0;
                    Buffer[outputIndex + 2] = 0;
                    Buffer[outputIndex + 3] = 0;
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