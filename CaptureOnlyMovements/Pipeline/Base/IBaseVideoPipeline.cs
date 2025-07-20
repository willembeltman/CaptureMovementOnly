using CaptureOnlyMovements.Interfaces;
using CaptureOnlyMovements.Pipeline.Interfaces;

namespace CaptureOnlyMovements.Pipeline.Base;

public interface IBaseVideoPipeline : INextVideoPipeline
{
    IFrameReader? Reader { get; }
    IFrameProcessor? Processor { get; }
    IFrameProcessorWithMask? ProcessorWithMask { get; }
    IFrameWriter? Writer { get; }
}