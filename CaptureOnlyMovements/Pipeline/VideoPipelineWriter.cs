using CaptureOnlyMovements.Interfaces;
using CaptureOnlyMovements.Pipeline.Base;
using CaptureOnlyMovements.Pipeline.Interfaces;
using CaptureOnlyMovements.Types;
using System;
using System.Threading;

namespace CaptureOnlyMovements.Pipeline;

public class VideoPipelineWriter(
    BaseVideoPipeline firstPipeline,
    BaseVideoPipeline previousPipeline,
    IFrameWriter writer,
    IConsole? console) 
    : BaseVideoPipeline(
        firstPipeline, 
        previousPipeline,
        null, null, null, writer,
        writer.GetType().Name, 
        console), 
    IFrameWriter
{
    private readonly AutoResetEvent Stopped = new(false);
    public bool Started { get; private set; }

    public void Start(IKillSwitch? cancellationToken)
    {
        base.StartVideo(cancellationToken, 1);
        Started = true;
    }
    public void Stop()
    {
        ((IBasePipeline)FirstPipeline).Stop(Exception);
    }

    public void WaitForExit()
    {
        if (((IBaseVideoPipeline)FirstPipeline).Reader == null)
            throw new Exception("You cannot WaitForExit a pipeline without reader.");

        if (!Started) return;
        Stopped.WaitOne();
    }

    public void WriteFrame(Frame frame)
    {
        if (((IBaseVideoPipeline)FirstPipeline).Reader != null)
            throw new Exception($"You cannot write to a pipeline with a reader, please remove the frame reader from the first pipeline.");
        
        ((INextVideoPipeline)FirstPipeline).HandleNextFrame(frame);
    }

    protected override void Kernel(object? objCancellationToken)
    {
        try
        {
            if (Frames == null)
                throw new InvalidOperationException("Pipeline not initialized. Call Start first.");

            while (!Disposing)
            {
                if (!FrameReceived.WaitOne(10_000))
                    continue;

                if (!Disposing)
                {
                    var frameIndex = FrameIndex - 1;
                    if (frameIndex < 0)
                        frameIndex = Frames.Length - 1;

                    var frame = Frames[frameIndex];
                    if (frame != null && Writer != null)
                        using (Statistics.NewMeasurement())
                            Writer.WriteFrame(frame);
                }

                FrameDone.Set();
            }
        }
        catch (Exception ex)
        {
            Disposing = true;
            Console?.WriteLine($"{Name} crashed: {ex.Message}");
            StopAll(ex);
        }

        Console?.WriteLine($"");
        Console?.WriteLine($"All done, statistics:");
        foreach (var pipeline in AllPipelines)
        {
            Console?.WriteLine($"{pipeline.Name}: {pipeline.Statistics.TimeSpend:F2}s / {pipeline.Statistics.Count} = {pipeline.Statistics.AverageMS:F2}ms each frame");
        }
        Console?.WriteLine($"Average FPS: {FirstPipeline.Statistics.AverageFPS:F2}fps");
        Console?.WriteLine($"");

        Stopped.Set();
    }
}


