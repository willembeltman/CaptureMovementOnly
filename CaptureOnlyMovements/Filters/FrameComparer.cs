using CaptureOnlyMovements.Filters;
using CaptureOnlyMovements.Interfaces;
using CaptureOnlyMovements.Types;

namespace CaptureOnlyMovements.Comparer;

public class FrameComparer : IFrameComparer
{
    public FrameComparer(
        IComparerConfig config,
        Resolution resolution,
        IPreview? preview = null)
    {
        Config = config; 
        Preview = preview;
        Resolution = resolution;
        CalculationFrameData = new bool[resolution.Width * resolution.Height];
        PreviousFrameData = new byte[resolution.Width * resolution.Height * 3];
    }

    private readonly IComparerConfig Config;
    private readonly IPreview? Preview;
    private readonly byte[] PreviousFrameData;

    public Resolution Resolution { get; }
    public bool[] CalculationFrameData { get; }
    public int Result_Difference { get; private set; }

    public bool IsDifferent(byte[] newFrameData)
    {
        // Calculate stride
        var stride = Resolution.Width * 3;

        // Counter for total difference in frame 
        Result_Difference = 0;

        // Iterate throug each pixel/color of the frame
        for (int y = 0; y < Resolution.Height; y++)
        {
            for (int x = 0; x < Resolution.Width; x++)
            {
                int index = y * stride + x * 3;

                byte currentRed = newFrameData[index];
                byte previousRed = PreviousFrameData[index];
                byte currentGreen = newFrameData[index + 1];
                byte previousGreen = PreviousFrameData[index + 1];
                byte currentBlue = newFrameData[index + 2];
                byte previousBlue = PreviousFrameData[index + 2];

                // Difference in color for each pixel, each color
                int pixelColorDifference1 = Math.Abs(currentRed - previousRed);
                int pixelColorDifference2 = Math.Abs(currentGreen - previousGreen);
                int pixelColorDifference3 = Math.Abs(currentBlue - previousBlue);

                var pixelColorDifference = pixelColorDifference1 + pixelColorDifference2 + pixelColorDifference3;
                var isDifferent = pixelColorDifference > Config.MaximumPixelDifferenceValue;

                if (Preview?.ShowMask == true)
                {
                    CalculationFrameData[y * Resolution.Width + x] = isDifferent;
                }

                if (isDifferent)
                {
                    // Add difference to total difference
                    Result_Difference++;
                }

                // Do check if total difference hasn't exceeded threshold otherwise no need to continue iterating
                if (Result_Difference > Config.MaximumDifferentPixelCount)
                {
                    if (Preview?.ShowMask != true)
                    {
                        Array.Copy(newFrameData, PreviousFrameData, newFrameData.Length);
                        return true;
                    }
                }
            }
        }

        // Do a final check for if CalculateFully is true
        if (Result_Difference > Config.MaximumDifferentPixelCount)
        {
            Array.Copy(newFrameData, PreviousFrameData, newFrameData.Length);
            return true;
        }

        return false;
    }
}