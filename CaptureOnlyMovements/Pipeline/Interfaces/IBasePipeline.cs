using System;

namespace CaptureOnlyMovements.Pipeline.Interfaces;

public interface IBasePipeline : IDisposable
{
    void Stop(Exception? ex);
}