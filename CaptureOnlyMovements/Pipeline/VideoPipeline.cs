using CaptureOnlyMovements.Interfaces;
using CaptureOnlyMovements.Pipeline.Base;
using CaptureOnlyMovements.Pipeline.Interfaces;
using System;

namespace CaptureOnlyMovements.Pipeline;

public class VideoPipeline : BaseVideoPipeline
{
    public VideoPipeline(
        IFrameReader reader,
        IConsole? console = null)
        : base(null, null, reader.GetType().Name, console)
        => Reader = reader;
    public VideoPipeline(
        BaseVideoPipeline firstPipeline,
        BaseVideoPipeline previousPipeline,
        IFrameProcessor processor,
        IConsole? console)
        : base(firstPipeline, previousPipeline, processor.GetType().Name, console) 
        => Processor = processor;

    private readonly IFrameReader? Reader;
    private readonly IFrameProcessor? Processor;

    public VideoPipeline Next(IFrameProcessor processor)
    {
        var nextPipeline = new VideoPipeline(FirstPipeline, this, processor, Console);
        NextPipeline = nextPipeline;
        return nextPipeline;
    }
    public VideoPipelineExecuter Next(IFrameWriter writer)
    {
        var nextPipeline = new VideoPipelineExecuter(FirstPipeline, this, writer, Console);
        NextPipeline = nextPipeline;
        return nextPipeline;
    }
    public VideoPipelineWithMaskOutput Next(IFrameProcessorWithMaskOutput processor)
    {
        var nextPipeline = new VideoPipelineWithMaskOutput(FirstPipeline, this, processor, Console);
        NextPipeline = nextPipeline;
        return nextPipeline;
    }

    protected override void Kernel(object? objCancellationToken)
    {
        try
        {
            var cancellationToken = (IKillSwitch?)objCancellationToken;
            while (!Disposing)
            {
                if (Frames == null)
                    throw new InvalidOperationException("Pipeline not initialized. Call Start first.");

                if (PreviousPipeline == null && Reader != null)
                {
                    // De reader loop (Eerste):
                    if (cancellationToken?.KillSwitch == true)
                    {
                        NextPipeline?.Stop();
                        Disposing = true;
                    }
                    else
                    {
                        Frames[FrameIndex] = Reader.ReadFrame(Frames[FrameIndex]);
                        var frame = Frames[FrameIndex];
                        if (frame == null)
                        {
                            NextPipeline?.Stop();
                            Disposing = true;
                        }
                        else
                            NextPipeline?.ProcessFrame(frame);

                        FrameIndex++;
                        if (FrameIndex >= Frames.Length)
                            FrameIndex = 0;
                    }
                }
                else if (PreviousPipeline != null && Processor != null)
                {
                    // De processor loop:
                    if (!FrameReceived.WaitOne(10_000))
                        continue;

                    if (Disposing)
                        NextPipeline?.Stop();
                    else
                    {
                        var frameIndex = FrameIndex - 1;
                        if (frameIndex < 0)
                            frameIndex = Frames.Length - 1;

                        var frame = Frames[frameIndex];
                        if (frame != null)
                        {
                            Frames[frameIndex] = Processor.ProcessFrame(frame);
                            frame = Frames[frameIndex];
                            if (frame != null)
                                NextPipeline?.ProcessFrame(frame);
                        }
                    }

                    FrameDone.Set();
                }
                else
                {
                    throw new Exception("Pipeline must either be a source or a processor.");
                }
            }
        }
        catch (Exception ex)
        {
            Disposing = true;
            NextPipeline?.Stop();
            StopAll();
            Console?.WriteLine($"{Name} crashed: {ex.Message}");
        }
    }
}


