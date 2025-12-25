using CaptureOnlyMovements.Interfaces;

namespace CaptureOnlyMovements.Pipeline.Interfaces;

public interface IBaseVideoPipeline : INextVideoPipeline
{
    IFrameReader? Reader { get; }
    IFrameProcessor? Processor { get; }
    IFrameProcessorWithMask? ProcessorWithMask { get; }
    IFrameWriter? Writer { get; }
}