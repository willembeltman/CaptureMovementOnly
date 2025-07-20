using CaptureOnlyMovements.Types;
using System;

namespace CaptureOnlyMovements.Interfaces;

public interface IFrameResizer : IDisposable
{
    Frame Resize(Frame frame);
}