using CaptureOnlyMovements.Pipeline.Interfaces;

namespace CaptureOnlyMovements.Interfaces;

public interface IPreview : IMaskWriter, IFrameWriter
{
    bool ShowMask { get; }
    bool ShowPreview { get; }
}