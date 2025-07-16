using CaptureOnlyMovements.Types;

namespace CaptureOnlyMovements.Interfaces;

public interface IPreview
{
    bool ShowMask { get; }
    bool ShowPreview { get; }
    void SetMask(BwFrame frame);
    void SetPreview(Frame frame);
}