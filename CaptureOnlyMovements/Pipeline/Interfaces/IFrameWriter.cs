

using CaptureOnlyMovements.Types;

namespace CaptureOnlyMovements.Pipeline.Interfaces;

public interface IFrameWriter
{
    void WriteFrame(Frame frame);
}
