using CaptureOnlyMovements.Interfaces;
using System;

namespace CaptureOnlyMovements.FrameConverters;

public class BwToBgraConverterUnsafe : IBwToBgraConverter
{
    public byte[] BwToBgra(bool[] bw, byte[]? bgra = null)
    {
        if (bgra == null || bw.Length * 4 != bgra.Length)
        {
            bgra = new byte[bw.Length * 4];
        }

        var black = (byte)0;
        var white = (byte)255;

        var pixelCount = bw.Length;

        unsafe
        {
            fixed (bool* bwPointer = bw)
            fixed (byte* bgraPointer = bgra)
            {
                //for (int k = 0; k < pixelCount; k++)
                //{
                //    int index = k * 4;
                //    byte color = bwPointer[k] ? white : black;
                //    bgraPointer[index] = color;
                //    bgraPointer[index + 1] = color;
                //    bgraPointer[index + 2] = color;
                //    bgraPointer[index + 3] = 255;
                //}
                bool* src = bwPointer;
                byte* dst = bgraPointer;
                for (int k = 0; k < pixelCount; k++)
                {
                    byte color = *src++ ? white : black;
                    *dst++ = color; // B
                    *dst++ = color; // G
                    *dst++ = color; // R
                    *dst++ = white; // A
                }
            }
        }
        return bgra;
    }

    public void Dispose()
    {
        GC.SuppressFinalize(this);
    }
}
