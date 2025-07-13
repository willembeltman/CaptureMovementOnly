using CaptureOnlyMovements.Types;

namespace CaptureOnlyMovements.FrameResizers;

public class BgrResizerTasks(Resolution outputResolution)
{
    private readonly byte[] Buffer = new byte[outputResolution.PixelCount * 3];

    public Frame Resize(Frame frame)
    {
        var inputResolution = frame.Resolution;
        if (inputResolution == outputResolution)
            return frame;

        var inputStride = inputResolution.Width * 3;
        var outputStride = outputResolution.Width * 3;

        var height = outputResolution.Height;
        var width = outputResolution.Width;

        int cpuCount = Environment.ProcessorCount;
        int chunkSize = Math.Max(1, height / (cpuCount * 8));
        int chunkCount = (height + chunkSize - 1) / chunkSize; // Ceiling division

        // Create tasks for each chunk
        Task<int>[] tasks = new Task<int>[chunkCount];
        for (int i = 0; i < chunkCount; i++)
        {
            int startY = i * chunkSize;
            int endY = Math.Min(startY + chunkSize, height);

            tasks[i] = Task.Run(() =>
            {
                for (int y = startY; y < endY; y++)
                {
                    for (int x = 0; x < width; x++)
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
                return 0;
            });
        }

        Task.WaitAll(tasks);

        return new Frame(Buffer, outputResolution);
    }
}