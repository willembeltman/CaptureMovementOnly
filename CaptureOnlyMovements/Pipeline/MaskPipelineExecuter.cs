using CaptureOnlyMovements.Interfaces;
using CaptureOnlyMovements.Pipeline.Base;
using CaptureOnlyMovements.Pipeline.Interfaces;
using System;

namespace CaptureOnlyMovements.Pipeline;

public class MaskPipelineExecuter : BaseMaskPipeline, IMaskWriter
{
    public MaskPipelineExecuter(
        BaseMaskPipeline? firstPipeline, 
        BaseMaskPipeline previousPipeline,
        IMaskWriter maskWriter,
        IConsole? console)
        : base(firstPipeline, previousPipeline, maskWriter.GetType().Name, console)
    {
        Writer = maskWriter;
    }

    private readonly IMaskWriter Writer;

    protected override void Kernel(object? objCancellationToken)
    {
        try
        {
            while (!Disposing)
            {
                if (Masks == null)
                    throw new InvalidOperationException("Pipeline not initialized. Call Start first.");

                if (!FrameReceived.WaitOne(10_000)) 
                    continue;

                if (!Disposing)
                {
                    var frameIndex = FrameIndex - 1;
                    if (frameIndex < 0)
                    {
                        frameIndex = Masks.Length - 1;
                    }

                    if (Masks[frameIndex] != null)
                    {
                        Writer.WriteMask(Masks[frameIndex]);
                    }
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
    }
}


