using CaptureOnlyMovements.Types;
using System;

namespace CaptureOnlyMovements.Interfaces;

public interface IFrameComparer : IDisposable
{
    bool[] MaskData { get; }
    int Difference { get; }
    Resolution Resolution { get; }

    bool IsDifferent(byte[] newFrameData);
}