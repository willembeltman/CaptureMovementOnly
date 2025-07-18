using CaptureOnlyMovements.Types;


namespace CaptureOnlyMovements.Pipeline.Interfaces
{
    public interface IMaskPipeline : IPipeline
    {
        INextMaskPipeline FirstMaskPipeline { get; }

        void WriteMask(BwFrame mask);
    }
}