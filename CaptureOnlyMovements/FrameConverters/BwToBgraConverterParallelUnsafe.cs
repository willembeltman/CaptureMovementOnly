using CaptureOnlyMovements.Interfaces;
using System;
using System.Threading.Tasks;

namespace CaptureOnlyMovements.FrameConverters;

public class BwToBgraConverterParallelUnsafe : IBwToBgraConverter
{
    public byte[] BwToBgra(bool[] bw, byte[]? bgra = null)
    {
        if (bgra == null || bw.Length * 4 != bgra.Length)
        {
            bgra = new byte[bw.Length * 4];
        }

        unsafe
        {
            fixed (bool* bwPointerBase = bw)
            fixed (byte* bgraPointerBase = bgra)
            {
                var bwPointerTransfer = (nint)bwPointerBase;
                var bgraPointerTransfer = (nint)bgraPointerBase;

                var black = (byte)0;
                var white = (byte)255;
                Parallel.For(0, bw.Length, i =>
                {
                    var bwPointer = (bool*)(bwPointerTransfer);
                    var bgraPointer = (byte*)(bgraPointerTransfer);

                    int j = i * 4;
                    byte color = bwPointer[i] ? white : black;
                    bgraPointer[j] = color;
                    bgraPointer[j + 1] = color;
                    bgraPointer[j + 2] = color;
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
