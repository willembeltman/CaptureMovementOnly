//namespace CaptureOnlyMovements.FrameConverters;

//public class BgrToBgraConverterParallel : IDisposable
//{
//    public byte[] ConvertBgrToBgra(byte[] bgr, byte[]? bgra = null)
//    {
//        if (bgra == null || bgr.Length / 3 * 4 != bgra.Length)
//        {
//            bgra = new byte[bgr.Length / 3 * 4];
//        }
//        int pixelCount = bgr.Length / 3;
//        Parallel.For(0, pixelCount, k =>
//        {
//            int i = k * 3;
//            int j = k * 4;
//            bgra[j] = bgr[i];
//            bgra[j + 1] = bgr[i + 1];
//            bgra[j + 2] = bgr[i + 2];
//            bgra[j + 3] = 255;
//        });
//        return bgra;
//    }

//    public void Dispose()
//    {
//    }
//}
