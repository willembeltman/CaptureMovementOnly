using CaptureOnlyMovements.Interfaces;
using CaptureOnlyMovements.Types;
using System.Threading;

namespace CaptureOnlyMovements.Comparer;

public class FrameComparerTasks : IDisposable
{
    public FrameComparerTasks(
        IComparerConfig config,
        IShowDifference showDifference,
        Resolution resolution)
    {
        Config = config;
        ShowDifference = showDifference;
        Resolution = resolution;
        CalculationFrameData = new bool[resolution.Width * resolution.Height];
        PreviousFrameData = new byte[resolution.Width * resolution.Height * 3];
    }

    public IComparerConfig Config { get; }
    public IShowDifference ShowDifference { get; }
    public Resolution Resolution { get; }
    public bool[] CalculationFrameData { get; }
    public byte[] PreviousFrameData { get; }
    public int Result_Difference { get; private set; }

    public bool IsDifferent(byte[] newFrameData)
    {
        int stride = Resolution.Width * 3;
        int totalDifferent = 0;
        bool differenceExceeded = false;

        Parallel.For(0, Resolution.Height, (y, state) =>
        {
            for (int x = 0; x < Resolution.Width; x++)
            {
                int index = y * stride + x * 3;

                int diff =
                    Math.Abs(newFrameData[index] - PreviousFrameData[index]) +
                    Math.Abs(newFrameData[index + 1] - PreviousFrameData[index + 1]) +
                    Math.Abs(newFrameData[index + 2] - PreviousFrameData[index + 2]);

                bool isDifferent = diff > Config.MaximumPixelDifferenceValue;

                if (ShowDifference.ShowDifference)
                {
                    CalculationFrameData[y * Resolution.Width + x] = isDifferent;
                }

                if (isDifferent)
                {
                    int current = Interlocked.Increment(ref totalDifferent);
                    if (current > Config.MaximumDifferentPixelCount)
                    {
                        differenceExceeded = true;
                        if (!ShowDifference.ShowDifference)
                        {
                            state.Stop(); // Abort parallel execution early
                            return;
                        }
                    }
                }
            }
        });

        Result_Difference = totalDifferent;

        if (differenceExceeded)
        {
            Array.Copy(newFrameData, PreviousFrameData, newFrameData.Length);
            return true;
        }

        return false;
    }

    public void Dispose() { }
}
