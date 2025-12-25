using CaptureOnlyMovements.Pipeline.Base;

namespace CaptureOnlyMovements.Pipeline.Interfaces;

public interface IMaskPipeline : IPipeline
{
    BaseMaskPipeline FirstMaskPipeline { get; }

}