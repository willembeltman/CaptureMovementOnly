

using CaptureOnlyMovements.Types;

namespace CaptureOnlyMovements.Pipeline.Interfaces;

public interface INextMaskPipeline : IMaskPipeline
{
    void HandleNextMask(BwFrame frame);
}