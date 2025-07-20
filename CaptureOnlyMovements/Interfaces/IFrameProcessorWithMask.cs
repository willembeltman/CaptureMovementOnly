using CaptureOnlyMovements.Types;

namespace CaptureOnlyMovements.Interfaces;

public interface IFrameProcessorWithMask
{
    (Frame?, BwFrame?) ProcessFrame(Frame frame, BwFrame? maskBuffer);
}
