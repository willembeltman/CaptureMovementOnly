using CaptureOnlyMovements.Interfaces;
using CaptureOnlyMovements.Pipeline.Interfaces;
using CaptureOnlyMovements.Types;
using System;

namespace CaptureOnlyMovements.Pipeline.Base;

public abstract class BaseVideoPipeline 
    : BasePipeline, INextVideoPipeline
{
    public BaseVideoPipeline(
        BaseVideoPipeline? firstPipeline,
        BaseVideoPipeline? previousPipeline, 
        string name,
        IConsole? console) 
        : base(firstPipeline, previousPipeline, name, console)
    {
        FirstPipeline = firstPipeline ?? this;
        PreviousPipeline = previousPipeline;
    }

    public BaseVideoPipeline FirstPipeline { get; }
    public BaseVideoPipeline? PreviousPipeline { get; }

    public Frame?[]? Frames { get; protected set; }
    public BwFrame?[]? Masks { get; protected set; }

    int IPipeline.Start(IKillSwitch? cancellationToken, int count) 
        => StartVideo(cancellationToken, count);
    protected virtual int StartVideo(IKillSwitch? cancellationToken, int count)
    {
        count++;
        count = ((IPipeline?)PreviousPipeline)?.Start(cancellationToken, count) ?? count;

        Frames = new Frame[count];
        Console?.WriteLine($"{Name}, number of frames: {Frames.Length}x");
        Thread.Start(cancellationToken);

        return count;
    }
    void INextVideoPipeline.ProcessFrame(Frame frame)
    {
        if (Frames == null)
            throw new InvalidOperationException("Pipeline not initialized. Call Start first.");

        FrameDone.WaitOne();

        Frames[FrameIndex] = frame;
        FrameIndex++;
        if (FrameIndex >= Frames.Length)
            FrameIndex = 0;

        FrameReceived.Set();
    }
}


