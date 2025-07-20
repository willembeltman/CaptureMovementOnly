using CaptureOnlyMovements.Interfaces;
using System;
using System.Threading.Tasks;

namespace CaptureOnlyMovements.FrameConverters;

public class BwToBgraConverterTasks : IBwToBgraConverter
{
    /// <summary>
    /// Converteert een BW‐buffer (8‑bpp) naar BGRA (32‑bpp) in parallel.
    /// </summary>
    public byte[] BwToBgra(bool[] bw, byte[]? bgra = null)
    {
        ArgumentNullException.ThrowIfNull(bw);
        int pixelCount = bw.Length / 3;

        if (bgra == null || pixelCount * 4 != bgra.Length)
        {
            bgra = new byte[pixelCount * 4];
        }

        // Chunk‑verdeling: (cpuCount * 8) gekozen om vals te geven m.b.t. context‑switch‑kosten,
        // zelfde logica als in FrameComparerTasks.
        int cpuCount = Environment.ProcessorCount;
        int chunkSize = Math.Max(1, pixelCount / (cpuCount * 8));
        int chunkCount = (pixelCount + chunkSize - 1) / chunkSize;

        var black = (byte)0;
        var white = (byte)255;
        Task[] tasks = new Task[chunkCount];
        for (int c = 0; c < chunkCount; c++)
        {
            int startPixel = c * chunkSize;
            int endPixel = Math.Min(startPixel + chunkSize, pixelCount);

            tasks[c] = Task.Run(() =>
            {
                // Lokale indices in bytes.
                int srcIndex = startPixel * 3;
                int dstIndex = startPixel * 4;

                for (int p = startPixel; p < endPixel; p++, srcIndex += 3, dstIndex += 4)
                {
                    bgra[dstIndex] = bw[srcIndex] ? white : black;
                    bgra[dstIndex + 1] = bw[srcIndex] ? white : black;
                    bgra[dstIndex + 2] = bw[srcIndex] ? white : black;
                    bgra[dstIndex + 3] = 255;
                }
            });
        }

        Task.WaitAll(tasks);
        return bgra;
    }

    public void Dispose()
    {
        GC.SuppressFinalize(this);
    }
}
