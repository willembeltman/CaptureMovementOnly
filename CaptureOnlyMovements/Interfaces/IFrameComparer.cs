using CaptureOnlyMovements.Types;

namespace CaptureOnlyMovements.Interfaces
{
    public interface IFrameComparer
    {
        bool[] CalculationFrameData { get; }
        int Result_Difference { get; }
        Resolution Resolution { get; }

        bool IsDifferent(byte[] newFrameData);
    }
}