﻿using CaptureOnlyMovements.Helpers;
using CaptureOnlyMovements.Interfaces;
using CaptureOnlyMovements.Types;

namespace CaptureOnlyMovements.Pipeline.Tasks;

public class SkipInitialOrNotDifferentFrames : IFrameProcessorWithMask
{
    private int frameIndex;

    public SkipInitialOrNotDifferentFrames(SkipTillNext_Index skipTillNextIndex, IBgrComparer comparer, IPreview preview)
    {
        SkipTillNextIndex = skipTillNextIndex;
        Comparer = comparer;
        Preview = preview;
    }

    public SkipTillNext_Index SkipTillNextIndex { get; }
    public IBgrComparer Comparer { get; }
    public IPreview Preview { get; }

    public (Frame?, BwFrame?) ProcessFrame(Frame frame, BwFrame? bwFrame)
    {
        frameIndex++;

        if (SkipTillNextIndex.NeedToSkip(frameIndex)) return (null, null);

        var isDifferent = Comparer.IsDifferent(frame.Buffer);

        if (Preview.ShowMask)
        {
            bwFrame = new BwFrame(Comparer.MaskData, Comparer.Resolution);
            Preview.WriteMask(bwFrame);
        }

        if (!isDifferent) return (null, bwFrame);

        SkipTillNextIndex.Reset(frameIndex);

        return (frame, bwFrame);
    }
}