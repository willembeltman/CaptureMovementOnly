using CaptureOnlyMovements.Helpers;
using CaptureOnlyMovements.Types;

namespace CaptureOnlyMovements.Interfaces;

public interface IApplication
{
    Config Config { get; }
    bool IsBusy { get; }
    FpsCounter InputFps { get; }
    FpsCounter OutputFps { get; }

    void FatalException(string message, string title);
    void FatalException(Exception exception);
}
public interface IPreview
{
    bool ShowMask { get; }
    bool ShowPreview { get; }
    void SetMask(BwFrame frame);
    void SetPreview(Frame frame);
}