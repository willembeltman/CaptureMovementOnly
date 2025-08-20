using CaptureOnlyMovements.Interfaces;
using CaptureOnlyMovements.Pipeline.Base;
using CaptureOnlyMovements.Pipeline.Interfaces;
using CaptureOnlyMovements.Types;
using System;

namespace CaptureOnlyMovements.Pipeline;

public class MaskPipelineWriter : BaseMaskPipeline, IMaskWriter
{
    public MaskPipelineWriter(
        IMaskWriter maskWriter,
        IConsole? console = null)
        : base(null, null, null, maskWriter, maskWriter.GetType().Name, console) { }

    public MaskPipelineWriter(
        BaseMaskPipeline? firstPipeline,
        BaseMaskPipeline previousPipeline,
        IMaskWriter maskWriter,
        IConsole? console)
        : base(firstPipeline, previousPipeline, null, maskWriter, maskWriter.GetType().Name, console) { }

    public void WriteMask(BwFrame mask)
        => ((INextMaskPipeline)FirstMaskPipeline).HandleNextMask(mask);

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
                    if (mask != null && MaskWriter != null)
                        using (Statistics.NewMeasurement())
                            MaskWriter.WriteMask(mask);
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
    }
}


