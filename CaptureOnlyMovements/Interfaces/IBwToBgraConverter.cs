using System;

namespace CaptureOnlyMovements.Interfaces
{
    public interface IBwToBgraConverter : IDisposable
    {
        byte[] BwToBgra(bool[] bw, byte[]? bgra = null);
    }
}