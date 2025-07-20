using CaptureOnlyMovements.Types;

namespace CaptureOnlyMovements.Interfaces;

public interface IFrameWriter
{
    void WriteFrame(Frame frame);
}
