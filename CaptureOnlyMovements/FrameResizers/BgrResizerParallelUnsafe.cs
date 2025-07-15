using System;
using System.Threading.Tasks;
using CaptureOnlyMovements.Types;

namespace CaptureOnlyMovements.FrameResizers;

/// <summary>
/// Resize BGR‑24 naar een andere resolutie met Parallel.For + unsafe pointers.
/// </summary>
public class BgrResizerParallelUnsafe(Resolution targetResolution) : IDisposable
{
    private readonly byte[] _buffer = new byte[targetResolution.PixelCount * 3];

    public Frame Resize(Frame frame)
    {
        var Resolution = frame.Resolution;
        if (Resolution == targetResolution)
            return frame;                       // niks te doen

        int srcStride = Resolution.Width * 3;
        int dstStride = targetResolution.Width * 3;

        byte[] dst = _buffer;                   // alias leesbaarder

        // Paralleliseer over Y‑coördinaten.
        Parallel.For(0, targetResolution.Height, y =>
        {
            unsafe
            {
                fixed (byte* pSrcStart = frame.Buffer)
                fixed (byte* pDstStart = dst)
                {
                    byte* pSrcLine = pSrcStart + y * srcStride;
                    byte* pDstLine = pDstStart + y * dstStride;

                    // Als deze scanlijn buiten de originele hoogte valt: vul hele lijn zwart.
                    if (y >= Resolution.Height)
                    {
                        BufferUtil.Memset(pDstLine, 0, dstStride);
                        return;
                    }

                    int copyWidth = Math.Min(Resolution.Width, targetResolution.Width);

                    // --- kopieer gedeelde breedte ---
                    BufferUtil.Memcpy(pDstLine, pSrcLine, copyWidth * 3);

                    // --- rechts aanvullen met zwart indien output breder is ---
                    if (copyWidth < targetResolution.Width)
                    {
                        byte* pFill = pDstLine + copyWidth * 3;
                        int fillBytes = (targetResolution.Width - copyWidth) * 3;
                        BufferUtil.Memset(pFill, 0, fillBytes);
                    }
                }
            }
        });

        return new Frame(dst, targetResolution);
    }

    public void Dispose() => GC.SuppressFinalize(this);

    // Kleine helper‑static voor snelle memset/memcpy met Unsafe.CopyBlock.
    private static class BufferUtil
    {
        public static unsafe void Memcpy(byte* dst, byte* src, int bytes)
            => System.Runtime.CompilerServices.Unsafe.CopyBlockUnaligned(dst, src, (uint)bytes);

        public static unsafe void Memset(byte* dst, byte value, int bytes)
            => System.Runtime.CompilerServices.Unsafe.InitBlockUnaligned(dst, value, (uint)bytes);
    }
}
