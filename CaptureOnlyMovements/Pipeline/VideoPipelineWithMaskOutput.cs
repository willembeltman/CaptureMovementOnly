using CaptureOnlyMovements.Interfaces;
using CaptureOnlyMovements.Pipeline.Base;
using CaptureOnlyMovements.Pipeline.Interfaces;
using CaptureOnlyMovements.Types;
using System;


namespace CaptureOnlyMovements.Pipeline;

public class VideoPipelineWithMaskOutput : BaseVideoPipeline
{
    public VideoPipelineWithMaskOutput(BaseVideoPipeline firstPipeline, BaseVideoPipeline previousPipeline, IFrameProcessorWithMaskOutput processor)
        : base(firstPipeline, previousPipeline, processor.GetType().Name)
    {
        Processor = processor;
    }

    private readonly IFrameProcessorWithMaskOutput Processor;

    protected BwFrame?[]? Masks;

    private INextVideoPipeline? NextPipeline;
    private IMaskWriter? NextMaskWriter;
    private INextMaskPipeline? NextMaskPipeline;

    protected override int StartVideo(IKillSwitch? cancellationToken, int count)
    {
        count++;
        count = ((IPipeline?)PreviousPipeline)!.Start(cancellationToken, count);
        var c2 = NextMaskPipeline?.Start(cancellationToken, 2);

        Frames = new Frame[count];
        Masks = new BwFrame[count];
        Console.WriteLine($"{Name} Masks: {Masks.Length}x");
        Thread.Start(cancellationToken);

        return count;
    }

    public VideoPipeline Next(IFrameProcessor frameProcessor, IMaskWriter? maskWriter = null)
    {
        var nextPipeline = new VideoPipeline(FirstPipeline, this, frameProcessor);
        NextPipeline = nextPipeline;
        NextMaskWriter = maskWriter;
        return nextPipeline;
    }
    public VideoPipelineExecuter Next(IFrameWriter frameWriter, IMaskWriter? maskWriter = null)
    {
        var nextPipeline = new VideoPipelineExecuter(FirstPipeline, this, frameWriter);
        NextPipeline = nextPipeline;
        NextMaskWriter = maskWriter;
        return nextPipeline;
    }
    public VideoPipeline Next(IFrameProcessor frameProcessor, MaskPipelineExecuter? maskPipelineExecuter)
    {
        var nextPipeline = new VideoPipeline(FirstPipeline, this, frameProcessor);
        NextPipeline = nextPipeline;
        NextMaskPipeline = maskPipelineExecuter;
        return nextPipeline;
    }
    public VideoPipelineExecuter Next(IFrameWriter frameWriter, MaskPipelineExecuter? maskPipelineExecuter)
    {
        var nextPipeline = new VideoPipelineExecuter(FirstPipeline, this, frameWriter);
        NextPipeline = nextPipeline;
        NextMaskPipeline = maskPipelineExecuter;
        return nextPipeline;
    }

    protected override void Kernel(object? objCancellationToken)
    {
        var cancellationToken = (IKillSwitch?)objCancellationToken;
        while (!Disposing)
        {
            if (Processor == null || Frames == null || Masks == null)
                throw new Exception("How did you get here? What'd you do?");

            if (!FrameReceived.WaitOne(10_000)) continue;

            if (Disposing)
            {
                NextMaskPipeline?.FirstMaskPipeline?.Stop();
                NextPipeline?.Stop();
            }
            else
            {
                var frameIndex = FrameIndex - 1;
                if (frameIndex < 0)
                {
                    frameIndex = Frames.Length - 1;
                }

                var frame = Frames[frameIndex];
                if (frame == null) throw new Exception("THIS DOES NOT COMPUTE :)");
                (Frames[frameIndex], Masks[frameIndex]) = Processor.ProcessFrame(frame, Masks[frameIndex]);
                frame = Frames[frameIndex];
                if (frame != null)
                {
                    NextPipeline?.ProcessFrame(frame);
                }
                var mask = Masks[frameIndex];
                if (mask != null)
                {
                    NextMaskWriter?.WriteMask(mask);
                    NextMaskPipeline?.ProcessMask(mask);
                }
            }

            FrameDone.Set();
        }
    }

    public override void Dispose()
    {
        base.Dispose();
        NextMaskPipeline?.Dispose();
        GC.SuppressFinalize(this);
    }
}


