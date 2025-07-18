

using CaptureOnlyMovements.Types;

namespace CaptureOnlyMovements.Pipeline.Interfaces;

public interface IFrameProcessor
{
    Frame? ProcessFrame(Frame frame);
}
