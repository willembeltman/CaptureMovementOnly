using CaptureOnlyMovements.Interfaces;
using CaptureOnlyMovements.Pipeline.Base;
using CaptureOnlyMovements.Pipeline.Interfaces;
using System;
using System.Threading;

namespace CaptureOnlyMovements.Pipeline;

public class VideoPipelineExecuter : BaseVideoPipeline
{
    public VideoPipelineExecuter(BaseVideoPipeline firstPipeline, BaseVideoPipeline previousPipeline, IFrameWriter writer)
        : base(firstPipeline, previousPipeline, writer.GetType().Name)
    {
        Writer = writer;
    }

    private readonly IFrameWriter Writer;
    private readonly AutoResetEvent Stopped = new AutoResetEvent(false);

    public void Start(IKillSwitch? cancellationToken)
    {
        base.StartVideo(cancellationToken, 1);
        Stopped.WaitOne();
    }

    protected override void Kernel(object? objCancellationToken)
    {
        var cancellationToken = (IKillSwitch?)objCancellationToken;
        while (!Disposing)
        {
            if (Frames == null) throw new Exception("How did you get here? What'd you do?");
            if (!FrameReceived.WaitOne(10_000)) continue;

            if (!Disposing)
            {
                var frameIndex = FrameIndex - 1;
                if (frameIndex < 0)
                {
                    frameIndex = Frames.Length - 1;
                }

                var frame = Frames[frameIndex];
                if (frame != null)
                {
                    Writer.WriteFrame(frame);
                }
            }

            FrameDone.Set();
        }
        Stopped.Set();
    }
}


