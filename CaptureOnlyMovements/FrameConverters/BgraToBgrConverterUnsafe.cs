using CaptureOnlyMovements.Interfaces;
using System;
using Vortice.Direct3D11;

namespace CaptureOnlyMovements.FrameConverters;

public class BgraToBgrConverterUnsafe : IBgraToBgrConverter
{
    public byte[] ConvertBgraToBgr(MappedSubresource bgr, int width, int height, byte[]? bgra = null)
    {
        // Converteer BGRA → BGR
        var srcStride = Convert.ToInt32(bgr.RowPitch);
        int dstStride = width * 3;
        int bgrSize = dstStride * height;

        if (bgra == null || bgra.Length != bgrSize)
            bgra = new byte[bgrSize];

        unsafe
        {
            byte* srcPtr = (byte*)bgr.DataPointer;
            fixed (byte* dstPtr = bgra)
            {
                byte* src = srcPtr;
                byte* dst = dstPtr;
                for (int k = 0; k < width * height; k++)
                {
                    *dst++ = *src++; // B
                    *dst++ = *src++; // G
                    *dst++ = *src++; // R
                    src++;           // skip A
                }
            }
        }

        return bgra;
    }

    public byte[] ConvertBgraToBgr(byte[] bgr, byte[]? bgra = null)
    {
        var bgrSize = bgr.Length / 3;

        if (bgra == null || bgra.Length != bgrSize)
            bgra = new byte[bgrSize];

        unsafe
        {
            fixed (byte* srcPtr = bgr)
            fixed (byte* dstPtr = bgra)
            {
                byte* src = srcPtr;
                byte* dst = dstPtr;
                for (int k = 0; k < bgrSize; k++)
                {
                    *dst++ = *src++; // B
                    *dst++ = *src++; // G
                    *dst++ = *src++; // R
                    src++;           // skip A
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
