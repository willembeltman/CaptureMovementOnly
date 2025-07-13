using CaptureOnlyMovements.Types;

namespace CaptureOnlyMovements;

public class RgbResizer
{
    private readonly Resolution OutputResolution;
    private readonly byte[] Buffer;

    public RgbResizer(Resolution outputResolution)
    {
        OutputResolution = outputResolution;
        Buffer = new byte[outputResolution.ByteLength];
    }

    public Frame Resize(Frame frame)
    {
        var inputResolution = frame.Resolution;
        if (inputResolution == OutputResolution)
            return frame;

        var inputStride = inputResolution.Width * 3;
        var outputStride = OutputResolution.Width * 3;

        for (int x = 0; x < OutputResolution.Width; x++)
        {
            for (int y = 0; y < OutputResolution.Height; y++)
            {
                int outputIndex = y * outputStride + x * 3;
                if (x < inputResolution.Width || y < inputResolution.Height)
                {
                    int inputIndex = y * inputStride + x * 3;
                    Buffer[outputIndex + 0] = frame.Buffer[inputIndex + 0];
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

        return new Frame(Buffer, OutputResolution);
    }
}