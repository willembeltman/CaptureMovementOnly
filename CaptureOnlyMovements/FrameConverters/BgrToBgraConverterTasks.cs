//using System;
//using System.Threading.Tasks;

//namespace CaptureOnlyMovements.FrameConverters;

//public class BgrToBgraConverterTasks : IDisposable
//{
//    /// <summary>
//    /// Converteert een BGR‐buffer (24‑bpp) naar BGRA (32‑bpp) in parallel.
//    /// </summary>
//    public byte[] ConvertBgrToBgra(byte[] bgr, byte[]? bgra = null)
//    {
//        if (bgr == null) throw new ArgumentNullException(nameof(bgr));
//        int pixelCount = bgr.Length / 3;

//        if (bgra == null || pixelCount * 4 != bgra.Length)
//        {
//            bgra = new byte[pixelCount * 4];
//        }

//        // Chunk‑verdeling: (cpuCount * 8) gekozen om vals te geven m.b.t. context‑switch‑kosten,
//        // zelfde logica als in FrameComparerTasks.
//        int cpuCount = Environment.ProcessorCount;
//        int chunkSize = Math.Max(1, pixelCount / (cpuCount * 8));
//        int chunkCount = (pixelCount + chunkSize - 1) / chunkSize;

//        var tasks = new Task[chunkCount];
//        for (int c = 0; c < chunkCount; c++)
//        {
//            int startPixel = c * chunkSize;
//            int endPixel = Math.Min(startPixel + chunkSize, pixelCount);

//            tasks[c] = Task.Run(() =>
//            {
//                // Lokale indices in bytes.
//                int srcIndex = startPixel * 3;
//                int dstIndex = startPixel * 4;

//                for (int p = startPixel; p < endPixel; p++, srcIndex += 3, dstIndex += 4)
//                {
//                    bgra[dstIndex] = bgr[srcIndex];   // B
//                    bgra[dstIndex + 1] = bgr[srcIndex + 1];   // G
//                    bgra[dstIndex + 2] = bgr[srcIndex + 2];   // R
//                    bgra[dstIndex + 3] = 255;                 // A (opaque)
//                }
//            });
//        }

//        Task.WaitAll(tasks);
//        return bgra;
//    }

//    public void Dispose() { }
//}
