using CaptureOnlyMovements.Interfaces;
using CaptureOnlyMovements.Pipeline.Interfaces;
using CaptureOnlyMovements.Types;
using System;


namespace CaptureOnlyMovements.Pipeline.Base;

public abstract class BaseVideoPipeline : BasePipeline, INextVideoPipeline
{
    public BaseVideoPipeline(BaseVideoPipeline? firstPipeline, BaseVideoPipeline? previousPipeline, string name) : base(previousPipeline, name)
    {
        FirstPipeline = firstPipeline ?? this;
        PreviousPipeline = previousPipeline;
    }

    public BaseVideoPipeline FirstPipeline { get; }
    protected BaseVideoPipeline? PreviousPipeline { get; }
    protected Frame?[]? Frames { get; set; }

    int IPipeline.Start(IKillSwitch? cancellationToken, int count) => StartVideo(cancellationToken, count);
    protected virtual int StartVideo(IKillSwitch? cancellationToken, int count)
    {
        count++;
        count = ((IPipeline?)PreviousPipeline)?.Start(cancellationToken, count) ?? count;

        Frames = new Frame[count];
        Console.WriteLine($"{Name} Frames: {Frames.Length}x");
        Thread.Start(cancellationToken);

        return count;
    }
    void INextVideoPipeline.ProcessFrame(Frame frame)
    {
        if (Frames == null) throw new Exception("How did you get here? What'd you do?");
        FrameDone.WaitOne();

        Frames[FrameIndex] = frame;

        FrameIndex++;
        if (FrameIndex >= Frames.Length)
        {
            FrameIndex = 0;
        }

        FrameReceived.Set();
    }
    void INextVideoPipeline.Stop()
    {
        if (Disposing) return;

        FrameDone.WaitOne();

        Disposing = true;

        FrameReceived.Set();
    }
}


