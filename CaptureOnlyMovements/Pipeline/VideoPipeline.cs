using CaptureOnlyMovements.Interfaces;
using CaptureOnlyMovements.Pipeline.Base;
using CaptureOnlyMovements.Pipeline.Interfaces;
using System;

namespace CaptureOnlyMovements.Pipeline;

public class VideoPipeline : BaseVideoPipeline
{
    public VideoPipeline(IFrameReader reader)
        : base(null, null, reader.GetType().Name)
    {
        Reader = reader;
    }
    public VideoPipeline(BaseVideoPipeline firstPipeline, BaseVideoPipeline previousPipeline, IFrameProcessor processor)
        : base(firstPipeline, previousPipeline, processor.GetType().Name)
    {
        Processor = processor;
    }

    private readonly IFrameReader? Reader;
    private readonly IFrameProcessor? Processor;

    private INextVideoPipeline? NextPipeline;

    public VideoPipeline Next(IFrameProcessor processor)
    {
        var nextPipeline = new VideoPipeline(FirstPipeline, this, processor);
        NextPipeline = nextPipeline;
        return nextPipeline;
    }
    public VideoPipelineExecuter Next(IFrameWriter writer)
    {
        var nextPipeline = new VideoPipelineExecuter(FirstPipeline, this, writer);
        NextPipeline = nextPipeline;
        return nextPipeline;
    }
    public VideoPipelineWithMaskOutput Next(IFrameProcessorWithMaskOutput processor)
    {
        var nextPipeline = new VideoPipelineWithMaskOutput(FirstPipeline, this, processor);
        NextPipeline = nextPipeline;
        return nextPipeline;
    }

    protected override void Kernel(object? objCancellationToken)
    {
        var cancellationToken = (IKillSwitch?)objCancellationToken;
        while (!Disposing)
        {
            if (Frames == null)
                throw new InvalidOperationException("Pipeline not initialized. Call StartVideo first.");

            if (PreviousPipeline == null && Reader != null)
            {
                if (cancellationToken?.KillSwitch == true)
                {
                    NextPipeline?.Stop();
                    Disposing = true;
                }
                else
                {
                    // De reader loop (Eerste):
                    Frames[FrameIndex] = Reader.ReadFrame(Frames[FrameIndex]);
                    if (Frames[FrameIndex] == null)
                    {
                        NextPipeline?.Stop();
                        Disposing = true;
                    }
                    else
                    {
                        NextPipeline?.ProcessFrame(Frames[FrameIndex]);
                    }

                    FrameIndex++;
                    if (FrameIndex >= Frames.Length)
                    {
                        FrameIndex = 0;
                    }
                }
            }
            else if (PreviousPipeline != null && Processor != null)
            {
                // De processor loop:

                if (!FrameReceived.WaitOne(10_000)) continue;

                if (Disposing)
                {
                    NextPipeline?.Stop();
                }
                else
                {
                    var frameIndex = FrameIndex - 1;
                    if (frameIndex < 0)
                    {
                        frameIndex = Frames.Length - 1;
                    }

                    Frames[frameIndex] = Processor.ProcessFrame(Frames[frameIndex]);
                    if (Frames[frameIndex] != null)
                    {
                        NextPipeline?.ProcessFrame(Frames[frameIndex]);
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
}


