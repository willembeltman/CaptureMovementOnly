using CaptureOnlyMovements.Pipeline.Interfaces;
using CaptureOnlyMovements.Types;

namespace CaptureOnlyMovements.Pipeline.Tasks
{
    internal class PassThrough : IMaskProcessor
    {
        public BwFrame ProcessMask(BwFrame? mask)
        {
            return mask!;
        }
    }
}