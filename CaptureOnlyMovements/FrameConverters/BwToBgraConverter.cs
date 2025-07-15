namespace CaptureOnlyMovements.FrameConverters;

public class BwToBgraConverter : IDisposable
{
    public byte[] BwToBgra(bool[] bw, byte[]? bgra = null)
    {
        if (bgra == null || bw.Length * 4 != bgra.Length)
        {
            bgra = new byte[bw.Length * 4];
        }
        var black = (byte)0;
        var white = (byte)255;
        for (int i = 0, j = 0; i < bw.Length; i++, j += 4)
        {
            bgra[j] = bw[i] ? white : black;
            bgra[j + 1] = bw[i] ? white : black;
            bgra[j + 2] = bw[i] ? white : black;
            bgra[j + 3] = 255;
        }
        return bgra;
    }

    public void Dispose()
    {
    }
}
