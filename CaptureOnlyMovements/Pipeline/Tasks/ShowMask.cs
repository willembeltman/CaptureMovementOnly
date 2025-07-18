using CaptureOnlyMovements.Interfaces;
using CaptureOnlyMovements.Pipeline.Interfaces;
using CaptureOnlyMovements.Types;

namespace CaptureOnlyMovements.Pipeline.Tasks;

public class ShowMask : IMaskWriter
{
    public ShowMask(IPreview preview)
    {
        Preview = preview;
    }

    public IPreview Preview { get; }

    public void WriteMask(BwFrame mask)
    {
        if (Preview.ShowMask)
            Preview.WriteMask(mask);
    }
}