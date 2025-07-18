using CaptureOnlyMovements.Types;

namespace CaptureOnlyMovements.Pipeline.Interfaces;

public interface IFrameReader
{
    Frame? ReadFrame(Frame? frame = null);
}
