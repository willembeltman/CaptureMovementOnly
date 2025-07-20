using CaptureOnlyMovements.Interfaces;
using CaptureOnlyMovements.Pipeline.Base;
using CaptureOnlyMovements.Pipeline.Interfaces;
using System;
using System.Threading;

namespace CaptureOnlyMovements.Pipeline;

public class VideoPipelineExecuter : BaseVideoPipeline
{
    public VideoPipelineExecuter(
        BaseVideoPipeline firstPipeline, 
        BaseVideoPipeline previousPipeline,
        IFrameWriter writer,
        IConsole? console)
        : base(firstPipeline, previousPipeline, writer.GetType().Name, console) 
        => Writer = writer;

    private readonly IFrameWriter Writer;
    private readonly AutoResetEvent Stopped = new(false);
    public bool Started { get; private set; }

    public void Start(IKillSwitch? cancellationToken)
    {
        base.StartVideo(cancellationToken, 1);
        Started = true;
    }

    public void WaitForExit()
    {
        if (!Started) return;
        Stopped.WaitOne();
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
                    if (frame != null)
                        using (ProcessStopwatch.NewMeasurement())
                            Writer.WriteFrame(frame);
                }

                FrameDone.Set();
            }
        }
        catch (Exception ex)
        {
            Disposing = true;
            Console?.WriteLine($"{Name} crashed: {ex.Message}");
            StopAll();
        }

        Console?.WriteLine($"");
        Console?.WriteLine($"All done, statistics:");
        foreach (var pipeline in AllPipelines)
        {
            Console?.WriteLine($"{pipeline.Name}: {pipeline.ProcessStopwatch.TimeSpend:F2}s / {pipeline.ProcessStopwatch.Count} = {pipeline.ProcessStopwatch.AverageMS:F2}ms each task");
        }
        Console?.WriteLine($"Average FPS: {FirstPipeline.ProcessStopwatch.AverageFPS:F2}fps");
        Console?.WriteLine($"");

        Stopped.Set();
    }
}


