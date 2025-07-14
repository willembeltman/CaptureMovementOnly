#define USE_INTRINSICS
using CaptureOnlyMovements.Interfaces;
using CaptureOnlyMovements.Types;
using System.Numerics;
using System.Runtime.Intrinsics;
using System.Runtime.Intrinsics.X86;

namespace CaptureOnlyMovements.FrameComparers;

public unsafe class FrameComparerUnsafe(
    IComparerConfig config,
    Resolution resolution,
    IPreview? preview = null) : IFrameComparer
{
    private readonly byte[] _prev = new byte[resolution.Width * resolution.Height * 3];

    public Resolution Resolution { get; } = resolution;
    public bool[] MaskData { get; } = new bool[resolution.Width * resolution.Height];
    public int Difference { get; private set; }

    public bool IsDifferent(byte[] current)
    {
        var w = Resolution.Width;
        var h = Resolution.Height;
        nint stride = w * 3;
        Difference = 0;

        fixed (byte* pCurr = current, pPrev = _prev)
        {
#if USE_INTRINSICS
            int vecSize = Vector128<byte>.Count;               // 16 bytes
            nint vecStep = vecSize;
            nint totalBytes = (nint)current.Length;
            nint i = 0;

            // vector‑loop
            for (; i + vecStep <= totalBytes; i += vecStep)
            {
                var vCurr = Vector128.LoadUnsafe(ref pCurr[i]);
                var vPrev = Vector128.LoadUnsafe(ref pPrev[i]);

                // |curr - prev|
                var diff = Sse2.SumAbsoluteDifferences(vCurr, vPrev).AsByte();

                // horizontaal optellen (rode+groene+blauwe component)
                // We doen dat hier grofweg door per 3 bytes te sommeren.
                // Simpele manier: we schuiven & sommeren; dit is nog steeds sneller dan scalar.
                Vector128<ushort> part1 = Sse2.UnpackLow(diff.AsUInt16(), Vector128<ushort>.Zero);
                Vector128<ushort> part2 = Sse2.UnpackHigh(diff.AsUInt16(), Vector128<ushort>.Zero);
                var sum = Sse2.Add(part1, part2);

                // threshold‑mask
                var over = Sse2.CompareGreaterThan(sum.AsInt16(), Vector128.Create((short)config.MaximumPixelDifferenceValue));

                // tel bits
                int changed = Sse2.MoveMask(over.AsByte());   // 16‑bit mask
                Difference += BitOperations.PopCount((uint)changed);

                if (preview?.ShowMask == true)
                {
                    // map mask per pixel (3 bytes → 1 bit); hier scalar
                    for (int b = 0; b < vecSize; b += 3)
                    {
                        bool diffPix = (changed & (1 << b)) != 0;
                        int pixelIndex = (int)((i + b) / 3);
                        MaskData[pixelIndex] = diffPix;
                    }
                }

                if (Difference > config.MaximumDifferentPixelCount)
                {
                    if (preview?.ShowMask != true)
                    {
                        Buffer.MemoryCopy(pCurr, pPrev, _prev.Length, _prev.Length);
                        return true;
                    }
                }
            }
#else
            // pure pointer‑lus, geen SIMD
            for (nint pos = 0; pos < stride * h; pos += 3)
            {
                int dR = pCurr[pos]     - pPrev[pos];
                int dG = pCurr[pos + 1] - pPrev[pos + 1];
                int dB = pCurr[pos + 2] - pPrev[pos + 2];

                int pixDiff = Math.Abs(dR) + Math.Abs(dG) + Math.Abs(dB);
                bool diffPix = pixDiff > config.MaximumPixelDifferenceValue;

                if (diffPix)
                    Result_Difference++;

                if (preview?.ShowMask == true)
                    CalculationFrameData[pos / 3] = diffPix;

                if (Result_Difference > config.MaximumDifferentPixelCount)
                {
                    if (preview?.ShowMask != true)
                    {
                        Buffer.MemoryCopy(pCurr, pPrev, _prev.Length, _prev.Length);
                        return true;
                    }
                }
            }
#endif
            // Als we hier komen is threshold niet overschreden
            if (Difference > config.MaximumDifferentPixelCount)
            {
                Buffer.MemoryCopy(pCurr, pPrev, _prev.Length, _prev.Length);
                return true;
            }
        }
        return false;
    }

    public void Dispose()
    {
    }
}

