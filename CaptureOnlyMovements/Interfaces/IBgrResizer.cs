using CaptureOnlyMovements.Types;
using System;

namespace CaptureOnlyMovements.Interfaces;

public interface IBgrResizer : IDisposable
{
    Frame Resize(Frame frame);
}