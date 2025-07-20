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
        IMaskProcessor? processor,
        IMaskWriter? maskWriter,
        string name,
        IConsole? console)
        : base(firstPipeline, previousPipeline, name, console)
    {
        FirstMaskPipeline = firstPipeline ?? this;
        PreviousMaskPipeline = previousPipeline;
        MaskProcessor = processor;
        MaskWriter = maskWriter;
    }

    public BaseMaskPipeline FirstMaskPipeline { get; }
    public BaseMaskPipeline? PreviousMaskPipeline { get; }
    public IMaskProcessor? MaskProcessor { get; }
    public IMaskWriter? MaskWriter { get; }
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

    void INextMaskPipeline.HandleNextMask(BwFrame mask)
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