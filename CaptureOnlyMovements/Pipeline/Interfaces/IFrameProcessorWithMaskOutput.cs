

using CaptureOnlyMovements.Types;

namespace CaptureOnlyMovements.Pipeline.Interfaces;

public interface IFrameProcessorWithMaskOutput
{
    (Frame?, BwFrame?) ProcessFrame(Frame frame, BwFrame? maskBuffer);
}
