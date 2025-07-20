using CaptureOnlyMovements.Interfaces;
using System;
using System.Threading.Tasks;

namespace CaptureOnlyMovements.FrameConverters;

public class BgrToBgraConverterParallelUnsafe : IBgrToBgraConverter
{
    public byte[] ConvertBgrToBgra(byte[] bgr, byte[]? bgra = null)
    {
        if (bgra == null || bgr.Length / 3 * 4 != bgra.Length)
        {
            bgra = new byte[bgr.Length / 3 * 4];
        }

        unsafe
        {
            fixed (byte* bgrPointerBase = bgr)
            fixed (byte* bgraPointerBase = bgra)
            {
                var bgrPointerTransfer = (nint)bgrPointerBase;
                var bgraPointerTransfer = (nint)bgraPointerBase;

                int pixelCount = bgr.Length / 3;
                Parallel.For(0, pixelCount, k =>
                {
                    var bgrPointer = (byte*)(bgrPointerTransfer);
                    var bgraPointer = (byte*)(bgraPointerTransfer);

                    int i = k * 3;
                    int j = k * 4;
                    bgraPointer[j] = bgrPointer[i];
                    bgraPointer[j + 1] = bgrPointer[i + 1];
                    bgraPointer[j + 2] = bgrPointer[i + 2];
                    bgraPointer[j + 3] = 255;
                });
            }
        }

        return bgra;
    }

    public void Dispose()
    {
        GC.SuppressFinalize(this);
    }
}
