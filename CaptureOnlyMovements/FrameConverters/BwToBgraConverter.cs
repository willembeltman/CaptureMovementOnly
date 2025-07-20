using CaptureOnlyMovements.Interfaces;
using System;

namespace CaptureOnlyMovements.FrameConverters;

public class BwToBgraConverter : IBwToBgraConverter
{
    public byte[] BwToBgra(bool[] bw, byte[]? bgra = null)
    {
        if (bgra == null || bw.Length * 4 != bgra.Length)
        {
            bgra = new byte[bw.Length * 4];
        }
        var black = (byte)0;
        var white = (byte)255;
        for (int srcIndex = 0, dstIndex = 0; srcIndex < bw.Length; srcIndex++, dstIndex += 4)
        {
            bgra[dstIndex] = bw[srcIndex] ? white : black;
            bgra[dstIndex + 1] = bw[srcIndex] ? white : black;
            bgra[dstIndex + 2] = bw[srcIndex] ? white : black;
            bgra[dstIndex + 3] = 255;
        }
        return bgra;
    }

    public void Dispose()
    {
        GC.SuppressFinalize(this);
    }
}