

using CaptureOnlyMovements.Types;
using System;

namespace CaptureOnlyMovements.Pipeline.Interfaces;

public interface INextMaskPipeline : IMaskPipeline
{
    void ProcessMask(BwFrame frame);
}