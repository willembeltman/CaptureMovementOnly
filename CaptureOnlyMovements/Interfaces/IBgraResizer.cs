using CaptureOnlyMovements.Types;
using System;

namespace CaptureOnlyMovements.Interfaces;

public interface IBgraResizer : IDisposable
{
    Frame Resize(Frame frame);
}