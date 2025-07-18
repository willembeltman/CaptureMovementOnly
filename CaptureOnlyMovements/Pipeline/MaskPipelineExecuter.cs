using CaptureOnlyMovements.Interfaces;
using CaptureOnlyMovements.Pipeline.Base;
using CaptureOnlyMovements.Pipeline.Interfaces;
using System;

namespace CaptureOnlyMovements.Pipeline;

public class MaskPipelineExecuter : BaseMaskPipeline, IMaskWriter
{
    public MaskPipelineExecuter(INextMaskPipeline? firstPipeline, BaseMaskPipeline previousPipeline, IMaskWriter maskWriter)
        : base(firstPipeline, previousPipeline, maskWriter.GetType().Name)
    {
        Writer = maskWriter;
    }

    private readonly IMaskWriter Writer;

    protected override void Kernel(object? objCancellationToken)
    {
        var cancellationToken = (IKillSwitch?)objCancellationToken;
        while (!Disposing)
        {
            if (Masks == null)
                throw new Exception("How did you get here? What'd you do?");

            if (!FrameReceived.WaitOne(10_000)) continue;

            if (Disposing)
            {
            }
            else
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
}


