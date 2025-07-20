

using CaptureOnlyMovements.Types;
using System;

namespace CaptureOnlyMovements.Pipeline.Interfaces;

public interface INextMaskPipeline : IMaskPipeline
{
    void HandleNextMask(BwFrame frame);
}