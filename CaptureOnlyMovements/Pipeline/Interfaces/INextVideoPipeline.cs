

using CaptureOnlyMovements.Types;

namespace CaptureOnlyMovements.Pipeline.Interfaces;

public interface INextVideoPipeline : IPipeline
{
    void HandleNextFrame(Frame frame);
}


