using CaptureOnlyMovements.Interfaces;
using System;
using Vortice;

namespace CaptureOnlyMovements.FrameConverters;

public class BgrToBgraConverterUnsafe : IBgrToBgraConverter
{
    public byte[] ConvertBgrToBgra(byte[] bgr, byte[]? bgra = null)
    {
        if (bgra == null || bgr.Length / 3 * 4 != bgra.Length)
        {
            bgra = new byte[bgr.Length / 3 * 4];
        }

        int pixelCount = bgr.Length / 3;

        unsafe
        {
            fixed (byte* bgrPointer = bgr)
            fixed (byte* bgraPointer = bgra)
            {
                //for (int k = 0; k < pixelCount; k++)
                //{
                //    int bgrIndex = k * 3;
                //    int bgraIndex = k * 4;
                //    bgraPointer[bgraIndex] = bgrPointer[bgrIndex];
                //    bgraPointer[bgraIndex + 1] = bgrPointer[bgrIndex + 1];
                //    bgraPointer[bgraIndex + 2] = bgrPointer[bgrIndex + 2];
                //    bgraPointer[bgraIndex + 3] = 255;
                //}

                byte* src = bgrPointer;
                byte* dst = bgraPointer;
                for (int k = 0; k < pixelCount; k++)
                {
                    *dst++ = *src++; // B
                    *dst++ = *src++; // G
                    *dst++ = *src++; // R
                    dst++;           // skip A
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
