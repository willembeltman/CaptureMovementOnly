using CaptureOnlyMovements.Types;

namespace CaptureOnlyMovements.Interfaces;

public interface IFrameProcessor
{
    Frame? ProcessFrame(Frame frame);
}
