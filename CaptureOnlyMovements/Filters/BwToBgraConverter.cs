namespace CaptureOnlyMovements.Filters;

public static class BwToBgraConverter
{
    public static byte[] BwToBgra(this bool[] bw)
    {
        var black = (byte)0;
        var white = (byte)255;
        var bgra = new byte[bw.Length * 4];
        for (int i = 0, j = 0; i < bw.Length; i++, j += 4)
        {
            bgra[j] = bw[i] ? white : black;
            bgra[j + 1] = bw[i] ? white : black;
            bgra[j + 2] = bw[i] ? white : black;
            bgra[j + 3] = 255;
        }
        return bgra;
    }
}
public static class BgrToBgraConverter
{
    public static byte[] BgrToBgra(this byte[] bgr)
    {
        var bgra = new byte[bgr.Length / 3 * 4];
        for (int i = 0, j = 0; i < bgr.Length; i += 3, j += 4)
        {
            bgra[j] = bgr[i];
            bgra[j + 1] = bgr[i + 1];
            bgra[j + 2] = bgr[i + 2];
            bgra[j + 3] = 255;
        }
        return bgra;
    }
}
