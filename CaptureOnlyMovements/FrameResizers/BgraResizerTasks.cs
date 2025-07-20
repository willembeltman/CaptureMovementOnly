using CaptureOnlyMovements.Interfaces;
using CaptureOnlyMovements.Types;
using System;
using System.Threading.Tasks;

namespace CaptureOnlyMovements.FrameResizers;

public class BgraResizerTasks(Resolution outputResolution) : IBgraResizer
{
    private readonly byte[] Buffer = new byte[outputResolution.PixelCount * 4];

    public Frame Resize(Frame frame)
    {
        var inputResolution = frame.Resolution;
        if (inputResolution == outputResolution)
            return frame;

        var inputStride = inputResolution.Width * 4;
        var outputStride = outputResolution.Width * 4;

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
                return 0;
            });
        }

        Task.WaitAll(tasks);

        return new Frame(Buffer, outputResolution);
    }

    public void Dispose()
    {
        GC.SuppressFinalize(this);
    }
}