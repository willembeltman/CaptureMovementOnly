using CaptureOnlyMovements.Pipeline.Base;
using CaptureOnlyMovements.Types;


namespace CaptureOnlyMovements.Pipeline.Interfaces
{
    public interface IMaskPipeline : IPipeline
    {
        BaseMaskPipeline FirstMaskPipeline { get; }

        void WriteMask(BwFrame mask);
    }
}