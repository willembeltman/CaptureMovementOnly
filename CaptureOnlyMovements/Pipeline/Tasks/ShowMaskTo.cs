using CaptureOnlyMovements.Interfaces;
using CaptureOnlyMovements.Pipeline.Interfaces;
using CaptureOnlyMovements.Types;

namespace CaptureOnlyMovements.Pipeline.Tasks;

public class ShowMaskTo : IMaskWriter, IFrameWriter
{
    public ShowMaskTo(IPreview? preview)
    {
        Preview = preview;
    }

    public IPreview? Preview { get; }

    public void WriteFrame(Frame frame)
    {
        if (Preview?.ShowPreview == true)
            Preview.WriteFrame(frame);
    }

    public void WriteMask(BwFrame mask)
    {
        if (Preview?.ShowMask == true)
            Preview.WriteMask(mask);
    }
}