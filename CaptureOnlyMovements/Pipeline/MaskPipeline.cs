using CaptureOnlyMovements.Interfaces;
using CaptureOnlyMovements.Pipeline.Base;
using CaptureOnlyMovements.Pipeline.Interfaces;
using CaptureOnlyMovements.Types;
using System;


namespace CaptureOnlyMovements.Pipeline;

public class MaskPipeline : BaseMaskPipeline
{
    public MaskPipeline(IMaskProcessor maskProcessor)
        : base(null, null, maskProcessor.GetType().Name)
    {
        MaskProcessor = maskProcessor;
    }
    public MaskPipeline(INextMaskPipeline firstPipeline, BaseMaskPipeline previousPipeline, IMaskProcessor maskProcessor)
        : base(firstPipeline, previousPipeline, maskProcessor.GetType().Name)
    {
        MaskProcessor = maskProcessor;
    }

    private readonly IMaskProcessor MaskProcessor;
    private INextMaskPipeline? NextMaskPipeline;

    protected override int StartMask(IKillSwitch? cancellationToken, int count)
    {
        count++;
        count = ((IPipeline?)PreviousMaskPipeline)?.Start(cancellationToken, count) ?? count;

        Masks = new BwFrame[count];
        Console.WriteLine($"{Name} Masks: {Masks.Length}x");
        Thread.Start(cancellationToken);

        return count;
    }

    public MaskPipeline Next(IMaskProcessor maskProcessor)
    {
        var nextPipeline = new MaskPipeline(FirstMaskPipeline, this, maskProcessor);
        NextMaskPipeline = nextPipeline;
        return nextPipeline;
    }
    public MaskPipelineExecuter Next(IMaskWriter maskWriter)
    {
        var nextPipeline = new MaskPipelineExecuter(FirstMaskPipeline, this, maskWriter);
        NextMaskPipeline = nextPipeline;
        return nextPipeline;
    }

    protected override void Kernel(object? objCancellationToken)
    {
        var cancellationToken = (IKillSwitch?)objCancellationToken;
        while (!Disposing)
        {
            if (MaskProcessor == null || Masks == null)
                throw new Exception("How did you get here? What'd you do?");

            if (!FrameReceived.WaitOne(10_000)) continue;

            if (Disposing)
            {
                NextMaskPipeline?.Stop();
            }
            else
            {
                var frameIndex = FrameIndex - 1;
                if (frameIndex < 0)
                {
                    frameIndex = Masks.Length - 1;
                }

                Masks[frameIndex] = MaskProcessor.ProcessMask(Masks[frameIndex]);
                if (Masks[frameIndex] != null)
                {
                    NextMaskPipeline?.ProcessMask(Masks[frameIndex]);
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


