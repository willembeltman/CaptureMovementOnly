using CaptureOnlyMovements.Interfaces;
using CaptureOnlyMovements.Types;
using System;
using System.Runtime.CompilerServices;

namespace CaptureOnlyMovements.FrameResizers;

public sealed class BgrResizerUnsafe2(Resolution outputResolution) : IFrameResizer
{
    private readonly byte[] _buffer = new byte[outputResolution.PixelCount * 3];

    public Frame Resize(Frame frame)
    {
        if (frame.Resolution == outputResolution)
            return frame;

        var inRes = frame.Resolution;
        int inStride = inRes.Width * 3;
        int outStride = outputResolution.Width * 3;

        int copyWidth = Math.Min(inRes.Width, outputResolution.Width) * 3;
        int copyHeight = Math.Min(inRes.Height, outputResolution.Height);

        byte[] src = frame.Buffer;
        byte[] dst = _buffer;

        // ---- 1. Kopieer overlappende regio lijn‑voor‑lijn ----
        unsafe
        {
            fixed (byte* pSrc = src)
            fixed (byte* pDst = dst)
            {
                byte* s = pSrc;
                byte* d = pDst;

                for (int y = 0; y < copyHeight; y++)
                {
                    Unsafe.CopyBlockUnaligned(
                        destination: d + y * outStride,
                        source: s + y * inStride,
                        byteCount: (uint)copyWidth);
                }
            }
        }

        // ---- 2. Pad rechts (input smaller) ----
        if (inRes.Width < outputResolution.Width)
        {
            int padBytes = outStride - copyWidth;

            for (int y = 0; y < copyHeight; y++)
            {
                Array.Clear(dst, y * outStride + copyWidth, padBytes);
            }
        }

        // ---- 3. Pad onder (input lager) ----
        if (inRes.Height < outputResolution.Height)
        {
            int start = copyHeight * outStride;
            Array.Clear(dst, start, dst.Length - start);
        }

        return new Frame(dst, outputResolution);
    }

    public void Dispose()
    {
    }
}
