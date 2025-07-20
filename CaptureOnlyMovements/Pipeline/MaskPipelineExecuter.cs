using CaptureOnlyMovements.Interfaces;
using CaptureOnlyMovements.Pipeline.Base;
using CaptureOnlyMovements.Pipeline.Interfaces;
using System;

namespace CaptureOnlyMovements.Pipeline;

public class MaskPipelineExecuter : BaseMaskPipeline, IMaskWriter
{
    public MaskPipelineExecuter(
        IMaskWriter maskWriter,
        IConsole? console = null)
        : base(null, null, maskWriter.GetType().Name, console)
        => MaskWriter = maskWriter;

    public MaskPipelineExecuter(
        BaseMaskPipeline? firstPipeline,
        BaseMaskPipeline previousPipeline,
        IMaskWriter maskWriter,
        IConsole? console)
        : base(firstPipeline, previousPipeline, maskWriter.GetType().Name, console)
        => MaskWriter = maskWriter;

    private readonly IMaskWriter MaskWriter;

    protected override void Kernel(object? objCancellationToken)
    {
        try
        {
            if (Masks == null)
                throw new InvalidOperationException("Pipeline not initialized. Call Start first.");

            while (!Disposing)
            {
                if (!FrameReceived.WaitOne(10_000)) 
                    continue;

                if (!Disposing)
                {
                    var frameIndex = FrameIndex - 1;
                    if (frameIndex < 0)
                    {
                        frameIndex = Masks.Length - 1;
                    }

                    var mask = Masks[frameIndex];
                    if (mask != null)
                        using (ProcessStopwatch.NewMeasurement())
                            MaskWriter.WriteMask(mask);
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


