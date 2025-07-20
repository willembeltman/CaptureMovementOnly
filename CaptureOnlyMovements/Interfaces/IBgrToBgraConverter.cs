using System;

namespace CaptureOnlyMovements.Interfaces
{
    public interface IBgrToBgraConverter : IDisposable
    {
        byte[] ConvertBgrToBgra(byte[] bgr, byte[]? bgra = null);
    }
}