using System;

namespace CaptureOnlyMovements.FrameConverters;

public class BwToBgraConverterUnsafe : IDisposable
{
    public byte[] ConvertBwToBgra(bool[] bw, byte[]? bgra = null)
    {
        if (bgra == null || bw.Length * 4 != bgra.Length)
        {
            bgra = new byte[bw.Length * 4];
        }

        unsafe
        {
            fixed (bool* bwPointer = bw)
            fixed (byte* bgraPointer = bgra)
            {
                var black = (byte)0;
                var white = (byte)255;

                var pixelCount = bw.Length;
                for (int k = 0; k < pixelCount; k++)
                {
                    int index = k * 4;
                    byte color = bwPointer[k] ? white : black;
                    bgraPointer[index] = color;
                    bgraPointer[index + 1] = color;
                    bgraPointer[index + 2] = color;
                    bgraPointer[index + 3] = 255;
                }
            }
        }
        return bgra;
    }

    public void Dispose()
    {
    }
}
