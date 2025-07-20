using CaptureOnlyMovements.FrameComparers;
using CaptureOnlyMovements.Helpers;
using CaptureOnlyMovements.Interfaces;
using CaptureOnlyMovements.Pipeline.Interfaces;
using CaptureOnlyMovements.Types;

namespace CaptureOnlyMovements.Pipeline.Tasks;

public class SkipInitialOrNotDifferentFrames : IFrameProcessorWithMaskOutput
{
    private int frameIndex;

    public SkipInitialOrNotDifferentFrames(skipTillNextIndexHelper skipTillNextIndex, IBgrComparer comparer, IPreview preview)
    {
        SkipTillNextIndex = skipTillNextIndex;
        Comparer = comparer;
        Preview = preview;
    }

    public skipTillNextIndexHelper SkipTillNextIndex { get; }
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

        if (!isDifferent) return (null, null);

        SkipTillNextIndex.Reset(frameIndex);

        return (frame, bwFrame);
    }
}