namespace CaptureOnlyMovements.FrameConverters;

public class BgrToBgraConverter : IDisposable
{
    public byte[] BgrToBgra(byte[] bgr, byte[]? bgra = null)
    {
        if (bgra == null || bgr.Length / 3 * 4 != bgra.Length)
        {
            bgra = new byte[bgr.Length / 3 * 4];
        }
        for (int i = 0, j = 0; i < bgr.Length; i += 3, j += 4)
        {
            bgra[j] = bgr[i];
            bgra[j + 1] = bgr[i + 1];
            bgra[j + 2] = bgr[i + 2];
            bgra[j + 3] = 255;
        }
        return bgra;
    }

    public void Dispose()
    {
    }
}
