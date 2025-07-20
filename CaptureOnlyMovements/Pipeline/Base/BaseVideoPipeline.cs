using CaptureOnlyMovements.Interfaces;
using CaptureOnlyMovements.Pipeline.Interfaces;
using CaptureOnlyMovements.Types;
using System;

namespace CaptureOnlyMovements.Pipeline.Base;

public abstract class BaseVideoPipeline 
    : BasePipeline, IBaseVideoPipeline
{
    public BaseVideoPipeline(
        BaseVideoPipeline? firstPipeline,
        BaseVideoPipeline? previousPipeline,
        IFrameReader? reader,
        IFrameProcessor? processor,
        IFrameProcessorWithMask? processorWithMask,
        IFrameWriter? writer,
        string name,
        IConsole? console) 
        : base(firstPipeline, previousPipeline, name, console)
    {
        FirstPipeline = firstPipeline ?? this;
        PreviousPipeline = previousPipeline;
        Reader = reader;
        Processor = processor;
        ProcessorWithMask = processorWithMask;
        Writer = writer;
    }

    protected BaseVideoPipeline FirstPipeline { get; }
    protected BaseVideoPipeline? PreviousPipeline { get; }
    protected IFrameReader? Reader { get; }
    protected IFrameProcessor? Processor { get; }
    protected IFrameProcessorWithMask? ProcessorWithMask { get; }
    protected IFrameWriter? Writer { get; }
    protected Frame?[]? Frames { get; set; }
    protected BwFrame?[]? Masks { get; set; }


    IFrameReader? IBaseVideoPipeline.Reader => Reader;
    IFrameProcessor? IBaseVideoPipeline.Processor => Processor;
    IFrameProcessorWithMask? IBaseVideoPipeline.ProcessorWithMask => ProcessorWithMask;
    IFrameWriter? IBaseVideoPipeline.Writer => Writer;

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
    void INextVideoPipeline.HandleNextFrame(Frame frame)
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


