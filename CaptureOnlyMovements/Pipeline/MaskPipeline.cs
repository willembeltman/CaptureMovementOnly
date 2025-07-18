﻿using CaptureOnlyMovements.Interfaces;
using CaptureOnlyMovements.Pipeline.Base;
using CaptureOnlyMovements.Pipeline.Interfaces;
using System;

namespace CaptureOnlyMovements.Pipeline;

public class MaskPipeline : BaseMaskPipeline
{
    public MaskPipeline(
        IMaskProcessor maskProcessor,
        IConsole? console = null)
        : base(null, null, maskProcessor.GetType().Name, console)
        => MaskProcessor = maskProcessor;
    public MaskPipeline(
        BaseMaskPipeline firstPipeline,
        BaseMaskPipeline previousPipeline,
        IMaskProcessor maskProcessor,
        IConsole? console)
        : base(firstPipeline, previousPipeline, maskProcessor.GetType().Name, console)
        => MaskProcessor = maskProcessor;

    private readonly IMaskProcessor? MaskProcessor;

    public MaskPipeline Next(IMaskProcessor maskProcessor)
    {
        var nextPipeline = new MaskPipeline(FirstMaskPipeline, this, maskProcessor, Console);
        NextMaskPipeline = nextPipeline;
        return nextPipeline;
    }
    public MaskPipelineExecuter Next(IMaskWriter maskWriter)
    {
        var nextPipeline = new MaskPipelineExecuter(FirstMaskPipeline, this, maskWriter, Console);
        NextMaskPipeline = nextPipeline;
        return nextPipeline;
    }

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

                if (Disposing)
                    NextMaskPipeline?.Stop();
                else
                {
                    var frameIndex = FrameIndex - 1;
                    if (frameIndex < 0)
                        frameIndex = Masks.Length - 1;

                    Masks[frameIndex] = MaskProcessor.ProcessMask(Masks[frameIndex]);
                    if (Masks[frameIndex] != null)
                        NextMaskPipeline?.ProcessMask(Masks[frameIndex]);
                }

                FrameDone.Set();
            }
        }
        catch (Exception ex)
        {
            Disposing = true;
            NextMaskPipeline?.Stop();
            StopAll();
            Console?.WriteLine($"{Name} crashed: {ex.Message}");
        }
    }
}


