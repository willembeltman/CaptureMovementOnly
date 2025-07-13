namespace CaptureOnlyMovements.FrameConverters;

public static class BwToBgraConverterParallel
{
    public static byte[] BwToBgraParallel(this bool[] bw, byte[]? bgra = null)
    {
        if (bgra == null || bw.Length * 4 != bgra.Length)
        {
            bgra = new byte[bw.Length * 4];
        }
        var black = (byte)0;
        var white = (byte)255;
        Parallel.For(0, bw.Length, i =>
        {
            int j = i * 4;
            byte color = bw[i] ? white : black;
            bgra[j] = color;
            bgra[j + 1] = color;
            bgra[j + 2] = color;
            bgra[j + 3] = 255;
        });
        return bgra;
    }
}
