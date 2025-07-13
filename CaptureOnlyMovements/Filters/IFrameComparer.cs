using CaptureOnlyMovements.Types;

namespace CaptureOnlyMovements.Filters
{
    public interface IFrameComparer
    {
        bool[] CalculationFrameData { get; }
        int Result_Difference { get; }
        Resolution Resolution { get; }

        bool IsDifferent(byte[] newFrameData);
    }
}