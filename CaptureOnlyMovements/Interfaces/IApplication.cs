using CaptureOnlyMovements.Types;
using System;

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
