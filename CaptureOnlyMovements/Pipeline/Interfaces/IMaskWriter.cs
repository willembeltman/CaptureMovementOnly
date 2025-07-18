

using CaptureOnlyMovements.Types;

namespace CaptureOnlyMovements.Pipeline.Interfaces;

public interface IMaskWriter
{
    void WriteMask(BwFrame mask);
}