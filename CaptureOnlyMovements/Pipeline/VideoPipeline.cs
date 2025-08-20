using CaptureOnlyMovements.Interfaces;
using CaptureOnlyMovements.Pipeline.Base;
using System;
using System.Diagnostics;

namespace CaptureOnlyMovements.Pipeline;

public class VideoPipeline : BaseVideoPipeline
{

    public VideoPipeline(
        IConsole? console = null)
        : base(null, null, null, null, null, null, "WriteFrame receiver", console) { }
    public VideoPipeline(
        IFrameReader reader,
        IConsole? console = null)
        : base(null, null, reader, null, null, null, reader.GetType().Name, console) { }
    public VideoPipeline(
        IFrameProcessor processor,
        IConsole? console = null)
        : base(null, null, null, processor, null, null, processor.GetType().Name, console) { }
    public VideoPipeline(
        BaseVideoPipeline firstPipeline,
        BaseVideoPipeline previousPipeline,
        IFrameProcessor processor,
        IConsole? console)
        : base(firstPipeline, previousPipeline, null, processor, null, null, processor.GetType().Name, console) { }

    public VideoPipeline Next(IFrameProcessor processor)
    {
        var nextPipeline = new VideoPipeline(FirstPipeline, this, processor, Console);
        NextVideoPipeline = nextPipeline;
        return nextPipeline;
    }
    public VideoPipelineWriter Next(IFrameWriter writer)
    {
        var nextPipeline = new VideoPipelineWriter(FirstPipeline, this, writer, Console);
        NextVideoPipeline = nextPipeline;
        return nextPipeline;
    }
    public VideoPipelineWithMaskOutput Next(IFrameProcessorWithMask processor)
    {
        var nextPipeline = new VideoPipelineWithMaskOutput(FirstPipeline, this, processor, Console);
        NextVideoPipeline = nextPipeline;
        return nextPipeline;
    }

    protected override void Kernel(object? objCancellationToken)
    {
        try
        {
            var cancellationToken = (IKillSwitch?)objCancellationToken;

            if (PreviousPipeline == null && Reader != null)
            {
                ReaderKernel(cancellationToken);
            }
            else 
            {
                ProcessorKernel();
            }
        }
        catch (Exception ex)
        {
            Exception = ex;
            Disposing = true;
            NextVideoPipeline?.Stop(ex);
            StopAll(ex);
            Console?.WriteLine($"{Name} crashed: {ex.Message}");
        }
    }
    private void ReaderKernel(IKillSwitch? cancellationToken)
    {
        if (Frames == null)
            throw new InvalidOperationException("Pipeline not initialized. Call Start first.");

        if (Reader == null)
            throw new Exception("Pipeline must either be a source or a processor.");

        while (!Disposing)
        {
            if (cancellationToken?.KillSwitch == true)
            {
                NextVideoPipeline?.Stop(Exception);
                Disposing = true;
            }
            else
            {
                using (Statistics.NewMeasurement())
                    Frames[FrameIndex] = Reader.ReadFrame(Frames[FrameIndex]);

                var frame = Frames[FrameIndex];
                if (frame == null)
                {
                    // Frame mag niet null zijn
                    // Dat betekend dat de reader is gestopt met readen(waarschijnlijk EOF is).
                    NextVideoPipeline?.Stop(Exception);
                    Disposing = true;
                }
                else
                    NextVideoPipeline?.HandleNextFrame(frame);

                FrameIndex++;
                if (FrameIndex >= Frames.Length)
                    FrameIndex = 0;
            }
        }
    }
    private void ProcessorKernel()
    {
        if (Frames == null)
            throw new InvalidOperationException("Pipeline not initialized. Call Start first.");

        //if (Processor == null)
        //    throw new Exception("Pipeline must either be a source or a processor.");

        while (!Disposing)
        {
            if (!FrameReceived.WaitOne(10_000))
                continue;

            if (Disposing)
                NextVideoPipeline?.Stop(Exception);
            else
            {
                var frameIndex = FrameIndex - 1;
                if (frameIndex < 0)
                    frameIndex = Frames.Length - 1;

                var frame = Frames[frameIndex];
                if (frame != null)
                {
                    if (Processor != null)
                        using (Statistics.NewMeasurement())
                            Frames[frameIndex] = Processor.ProcessFrame(frame);

                    frame = Frames[frameIndex];
                    if (frame != null)
                        NextVideoPipeline?.HandleNextFrame(frame);
                }
            }

            FrameDone.Set();
        }
    }
}


