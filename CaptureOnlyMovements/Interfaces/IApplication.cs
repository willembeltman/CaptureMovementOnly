using CaptureOnlyMovements.Helpers;

namespace CaptureOnlyMovements.Interfaces;

public interface IApplication : IFFMpegDebugWriter, IDebugWriter
{
    Config Config { get; }
    bool IsBusy { get; }
    FpsCounter FpsCounter { get; }

    void FatalException(string message, string title);
    void FatalException(Exception exception);
}