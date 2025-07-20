using CaptureOnlyMovements.FrameComparers;
using CaptureOnlyMovements.Interfaces;
using CaptureOnlyMovements.Types;

namespace CaptureOnlyMovements.Pipeline.Tasks
{
    internal class SkipNotDifferentFrames : IFrameProcessorWithMask
    {
        public SkipNotDifferentFrames(IBgrComparer comparer, IPreview? preview)
        {
            Comparer = comparer;
            Preview = preview;
        }

        public IBgrComparer Comparer { get; }
        public IPreview? Preview { get; }

        public (Frame?, BwFrame?) ProcessFrame(Frame frame, BwFrame? bwFrame)
        {
            var isDifferent = Comparer.IsDifferent(frame.Buffer);

            if (Preview?.ShowMask == true)
            {
                bwFrame = new BwFrame(Comparer.MaskData, Comparer.Resolution);
                Preview.WriteMask(bwFrame);
            }

            if (!isDifferent) return (null, bwFrame);

            return (frame, bwFrame);
        }
    }
}