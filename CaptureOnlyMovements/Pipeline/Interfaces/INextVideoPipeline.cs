

using CaptureOnlyMovements.Types;
using System;

namespace CaptureOnlyMovements.Pipeline.Interfaces;

public interface INextVideoPipeline : IPipeline
{
    void ProcessFrame(Frame frame);
}


