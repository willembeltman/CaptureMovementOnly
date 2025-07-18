using CaptureOnlyMovements.Interfaces;
using CaptureOnlyMovements.Pipeline.Interfaces;
using CaptureOnlyMovements.Types;
using System;

namespace CaptureOnlyMovements.Pipeline.Base;

public abstract class BaseMaskPipeline : BasePipeline, INextMaskPipeline
{
    public BaseMaskPipeline(
        BaseMaskPipeline? firstPipeline,
        BaseMaskPipeline? previousPipeline, 
        string name,
        IConsole? console)
        : base(firstPipeline, previousPipeline, name, console)
    {
        FirstMaskPipeline = firstPipeline ?? this;
        PreviousMaskPipeline = previousPipeline;
    }

    public BaseMaskPipeline FirstMaskPipeline { get; }
    public BaseMaskPipeline? PreviousMaskPipeline { get; }
    protected BwFrame[]? Masks { get; set; }

    int IPipeline.Start(IKillSwitch? cancellationToken, int count) 
        => StartMask(cancellationToken, count);
    protected virtual int StartMask(IKillSwitch? cancellationToken, int count)
    {
        count++;
        count = ((IMaskPipeline?)PreviousMaskPipeline)?.Start(cancellationToken, count) ?? count;

        Masks = new BwFrame[count];
        Console?.WriteLine($"{Name}, number of masks: {Masks.Length}");
        Thread.Start(cancellationToken);

        return count;
    }

    public void WriteMask(BwFrame mask)
        => ((INextMaskPipeline)FirstMaskPipeline).ProcessMask(mask);

    void INextMaskPipeline.ProcessMask(BwFrame mask)
    {
        if (Masks == null)
            throw new InvalidOperationException("Pipeline not initialized. Call Start first.");

        FrameDone.WaitOne();

        Masks[FrameIndex] = mask;

        FrameIndex++;
        if (FrameIndex >= Masks.Length)
            FrameIndex = 0;

        FrameReceived.Set();
    }
}