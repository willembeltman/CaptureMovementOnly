using System;
using Vortice.Direct3D11;

namespace CaptureOnlyMovements.Interfaces
{
    public interface IBgraToBgrConverter : IDisposable
    {
        byte[] ConvertBgraToBgr(byte[] bgr, byte[]? bgra = null);
        byte[] ConvertBgraToBgr(MappedSubresource bgr, int width, int height, byte[]? bgra = null);
    }
}