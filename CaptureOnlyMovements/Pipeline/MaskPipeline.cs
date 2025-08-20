using CaptureOnlyMovements.Interfaces;
using CaptureOnlyMovements.Pipeline.Base;
using CaptureOnlyMovements.Pipeline.Interfaces;
using System;

namespace CaptureOnlyMovements.Pipeline;

public class MaskPipeline : BaseMaskPipeline
{
    public MaskPipeline(
        IConsole? console = null)
        : base(null, null, null, null, "Basdfas", console) { }
    public MaskPipeline(
        IMaskProcessor maskProcessor,
        IConsole? console = null)
        : base(null, null, maskProcessor, null, maskProcessor.GetType().Name, console) { }
    public MaskPipeline(
        BaseMaskPipeline firstPipeline,
        BaseMaskPipeline previousPipeline,
        IMaskProcessor maskProcessor,
        IConsole? console)
        : base(firstPipeline, previousPipeline, maskProcessor, null, maskProcessor.GetType().Name, console) { }

    public MaskPipeline Next(IMaskProcessor maskProcessor)
    {
        var nextPipeline = new MaskPipeline(FirstMaskPipeline, this, maskProcessor, Console);
        NextMaskPipeline = nextPipeline;
        return nextPipeline;
    }
    public MaskPipelineWriter Next(IMaskWriter maskWriter)
    {
        var nextPipeline = new MaskPipelineWriter(FirstMaskPipeline, this, maskWriter, Console);
        NextMaskPipeline = nextPipeline;
        return nextPipeline;
    }

    protected override void Kernel(object? objCancellationToken)
    {
        try
        {
            if (Masks == null)
                throw new InvalidOperationException("Pipeline not initialized. Call Start first.");

            while (!Disposing)
            {
                if (!FrameReceived.WaitOne(10_000))
                    continue;

                if (Disposing)
                    NextMaskPipeline?.Stop(Exception);
                else
                {
                    var frameIndex = FrameIndex - 1;
                    if (frameIndex < 0)
                        frameIndex = Masks.Length - 1;

                    var mask = Masks[frameIndex];
                    if (mask != null && MaskProcessor != null)
                        using (Statistics.NewMeasurement())
                            Masks[frameIndex] = MaskProcessor.ProcessMask(mask);

                    mask = Masks[frameIndex];
                    if (mask != null)
                        NextMaskPipeline?.HandleNextMask(mask);
                }

                FrameDone.Set();
            }
        }
        catch (Exception ex)
        {
            Disposing = true;
            NextMaskPipeline?.Stop(ex);
            StopAll(ex);
            Console?.WriteLine($"{Name} crashed: {ex.Message}");
        }
    }
}


