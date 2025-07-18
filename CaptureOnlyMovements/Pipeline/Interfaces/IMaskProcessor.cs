

using CaptureOnlyMovements.Types;

namespace CaptureOnlyMovements.Pipeline.Interfaces;

public interface IMaskProcessor
{
    BwFrame ProcessMask(BwFrame? mask);
}