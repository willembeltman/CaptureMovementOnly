using CaptureOnlyMovements.Helpers;
using CaptureOnlyMovements.Types;

namespace CaptureOnlyMovements.Interfaces;

public interface IApplication : IFFMpegDebugWriter, IDebugWriter, IShowDifference
{
    Config Config { get; }
    bool IsBusy { get; }
    FpsCounter InputFps { get; }
    FpsCounter OutputFps { get; }

    void FatalException(string message, string title);
    void FatalException(Exception exception);
    void SetMask(bool[] frameData, Resolution frameResolution);
    void SetPreview(Frame frame);
}