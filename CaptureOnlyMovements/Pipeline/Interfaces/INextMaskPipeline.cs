

using CaptureOnlyMovements.Types;
using System;

namespace CaptureOnlyMovements.Pipeline.Interfaces;

public interface INextMaskPipeline : IMaskPipeline, IDisposable
{
    void ProcessMask(BwFrame frame);
    void Stop();
}