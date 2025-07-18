

using CaptureOnlyMovements.Types;
using System;

namespace CaptureOnlyMovements.Pipeline.Interfaces;

public interface INextVideoPipeline : IPipeline, IDisposable
{
    void ProcessFrame(Frame frame);
    void Stop();
}


