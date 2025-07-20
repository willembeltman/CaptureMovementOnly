using CaptureOnlyMovements.Interfaces;
using CaptureOnlyMovements.Pipeline.Base;
using CaptureOnlyMovements.Pipeline.Interfaces;
using CaptureOnlyMovements.Types;
using System;

namespace CaptureOnlyMovements.Pipeline;

public class VideoPipelineWithMaskOutput : BaseVideoPipeline
{
    public VideoPipelineWithMaskOutput(
        BaseVideoPipeline firstPipeline,
        BaseVideoPipeline previousPipeline,
        IFrameProcessorWithMaskOutput processor,
        IConsole? console)
        : base(firstPipeline, previousPipeline, processor.GetType().Name, console)
        => Processor = processor;

    private readonly IFrameProcessorWithMaskOutput Processor;

    protected override int StartVideo(IKillSwitch? cancellationToken, int count)
    {
        count++;
        var previousPipeline = (IPipeline?)PreviousPipeline;
        if (previousPipeline == null)
            throw new Exception(
                @"This should not be possible! 

You can only create a `VideoPipelineWithMaskOutput` with the Next() command, 
thus it always has a previous pipeline. This does not, so, IDK anymore.

I hope everything shuts down responsibly because who knows which threads are started, 
if the previous pipeline is null and you somehow got here. 

This is buggy.

If you are a user reading this, IDK, this is very very buggy. 
So maybe just throw this copy away and re-download it.");

        count = previousPipeline.Start(cancellationToken, count);
        NextMaskPipeline?.Start(cancellationToken, 2);

        Frames = new Frame[count];
        Masks = new BwFrame[count];
        Console?.WriteLine($"{Name}, number of frames: {Frames.Length}, number of masks: {Masks.Length}");
        Thread.Start(cancellationToken);

        return count;
    }

    public VideoPipeline Next(IFrameProcessor frameProcessor, MaskPipelineExecuter? maskPipelineExecuter)
    {
        var nextPipeline = new VideoPipeline(FirstPipeline, this, frameProcessor, Console);
        NextVideoPipeline = nextPipeline;
        NextMaskPipeline = maskPipelineExecuter;
        return nextPipeline;
    }
    public VideoPipelineExecuter Next(IFrameWriter frameWriter, MaskPipelineExecuter? maskPipelineExecuter)
    {
        var nextPipeline = new VideoPipelineExecuter(FirstPipeline, this, frameWriter, Console);
        NextVideoPipeline = nextPipeline;
        NextMaskPipeline = maskPipelineExecuter;
        return nextPipeline;
    }

    protected override void Kernel(object? objCancellationToken)
    {
        try
        {
            if (Processor == null || Frames == null || Masks == null)
                throw new InvalidOperationException("Pipeline not initialized. Call Start first.");

            while (!Disposing)
            {
                if (!FrameReceived.WaitOne(10_000))
                    continue;

                if (Disposing)
                {
                    ((INextMaskPipeline?)NextMaskPipeline?.FirstMaskPipeline)?.Stop();
                    NextVideoPipeline?.Stop();
                }
                else
                {
                    var frameIndex = FrameIndex - 1;
                    if (frameIndex < 0)
                        frameIndex = Frames.Length - 1;

                    var frame = Frames[frameIndex]
                        ?? throw new Exception("Something goes terribly wrong, this frame should be filled because this pipeline alsways has a previous pipeline.");

                    using (ProcessStopwatch.NewMeasurement())
                        (Frames[frameIndex], Masks[frameIndex]) = Processor.ProcessFrame(frame, Masks[frameIndex]);

                    frame = Frames[frameIndex];
                    if (frame != null)
                        NextVideoPipeline?.ProcessFrame(frame);

                    var mask = Masks[frameIndex];
                    if (mask != null)
                        NextMaskPipeline?.ProcessMask(mask);
                }

                FrameDone.Set();
            }
        }
        catch (Exception ex)
        {
            Disposing = true;
            StopAll();
            ((INextMaskPipeline?)NextMaskPipeline?.FirstMaskPipeline)?.Stop();
            NextVideoPipeline?.Stop();
            Console?.WriteLine($"{Name} crashed: {ex.Message}");
        }
    }

    public override void Dispose()
    {
        base.Dispose();
        NextMaskPipeline?.Dispose();
        GC.SuppressFinalize(this);
    }
}
