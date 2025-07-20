using CaptureOnlyMovements.Pipeline.Interfaces;
using CaptureOnlyMovements.Types;

namespace CaptureOnlyMovements
{
    internal class PassThrough : IMaskProcessor
    {
        public BwFrame ProcessMask(BwFrame? mask)
        {
            return mask!;
        }
    }
}