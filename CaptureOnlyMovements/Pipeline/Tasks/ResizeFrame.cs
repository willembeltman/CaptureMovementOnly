using CaptureOnlyMovements.FrameResizers;
using CaptureOnlyMovements.Interfaces;
using CaptureOnlyMovements.Types;

namespace CaptureOnlyMovements.Pipeline.Tasks;

public class ResizeFrame : IFrameProcessor
{
    public ResizeFrame(BgrResizerUnsafe resizer)
    {
        Resizer = resizer;
    }

    public BgrResizerUnsafe Resizer { get; }

    public Frame? ProcessFrame(Frame frame)
    {
        return Resizer.Resize(frame);
    }
}