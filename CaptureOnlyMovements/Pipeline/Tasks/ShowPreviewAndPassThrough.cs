using CaptureOnlyMovements.Interfaces;
using CaptureOnlyMovements.Pipeline.Interfaces;
using CaptureOnlyMovements.Types;

namespace CaptureOnlyMovements.Pipeline.Tasks;

public class ShowPreviewAndPassThrough : IFrameProcessor
{
    public ShowPreviewAndPassThrough(IPreview preview)
    {
        Preview = preview;
    }

    public IPreview Preview { get; }

    public Frame? ProcessFrame(Frame frame)
    {
        if (Preview.ShowPreview)
            Preview.WriteFrame(frame);
        return frame;
    }
}