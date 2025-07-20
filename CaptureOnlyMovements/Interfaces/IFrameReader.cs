using CaptureOnlyMovements.Types;

namespace CaptureOnlyMovements.Interfaces;

public interface IFrameReader
{
    Frame? ReadFrame(Frame? frame = null);
}
