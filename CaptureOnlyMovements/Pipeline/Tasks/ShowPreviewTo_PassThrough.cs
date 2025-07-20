using CaptureOnlyMovements.Interfaces;
using CaptureOnlyMovements.Pipeline.Interfaces;
using CaptureOnlyMovements.Types;

namespace CaptureOnlyMovements.Pipeline.Tasks;

public class ShowPreviewTo_PassThrough : IFrameProcessor
{
    public ShowPreviewTo_PassThrough(IPreview? preview)
    {
        Preview = preview;
    }

    public IPreview? Preview { get; }

    public Frame? ProcessFrame(Frame frame)
    {
        if (Preview?.ShowPreview == true)
            Preview.WriteFrame(frame);
        return frame;
    }
}