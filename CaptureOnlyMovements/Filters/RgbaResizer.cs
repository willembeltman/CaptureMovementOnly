using CaptureOnlyMovements.Types;

namespace CaptureOnlyMovements;

public class RgbaResizer
{
    private readonly Resolution OutputResolution;
    private readonly byte[] Buffer;

    public RgbaResizer(Resolution outputResolution)
    {
        OutputResolution = outputResolution;
        Buffer = new byte[outputResolution.PixelLength * 4];
    }

    public Frame Resize(Frame frame)
    {
        var inputResolution = frame.Resolution;
        if (inputResolution == OutputResolution)
            return frame;

        var inputStride = inputResolution.Width * 4;
        var outputStride = OutputResolution.Width * 4;

        for (int x = 0; x < OutputResolution.Width; x++)
        {
            for (int y = 0; y < OutputResolution.Height; y++)
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

        return new Frame(Buffer, OutputResolution);
    }
}