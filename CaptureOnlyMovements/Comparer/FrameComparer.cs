using CaptureOnlyMovements.Types;

namespace CaptureOnlyMovements.Comparer;

public class FrameComparer
{
    public FrameComparer(
        int maximumPixelDifferenceValue, 
        long maximumDifferentPixelCount,
        Resolution resolution, 
        bool calculateFully = false)
    {
        MaximumPixelDifferenceValue = maximumPixelDifferenceValue;
        MaximumDifferentPixelCount = maximumDifferentPixelCount;
        Resolution = resolution;
        CalculationFrameData = new bool[resolution.Width * resolution.Height];
        PreviousFrameData = new byte[resolution.Width * resolution.Height * 3];
        CalculateFully = calculateFully;
    }

    public int MaximumPixelDifferenceValue { get; }
    public long MaximumDifferentPixelCount { get; }
    public Resolution Resolution { get; }
    public bool[] CalculationFrameData { get; }
    public byte[] PreviousFrameData { get; }
    public bool CalculateFully { get; }

    public bool IsDifferent(byte[] newFrameData)
    {
        // Calculate stride
        var stride = Resolution.Width * 3;

        // Counter for total difference in frame 
        var Result_Difference = 0;

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
                var isDifferent = pixelColorDifference > MaximumPixelDifferenceValue;

                if (CalculateFully)
                    CalculationFrameData[y * Resolution.Width + x] = isDifferent;

                if (isDifferent)
                {
                    // Add difference to total difference
                    Result_Difference++;
                }

                // Do check if total difference hasn't exceeded threshold otherwise no need to continue iterating
                if (Result_Difference > MaximumDifferentPixelCount)
                {
                    if (!CalculateFully)
                    {
                        Array.Copy(newFrameData, PreviousFrameData, newFrameData.Length);
                        return true;
                    }
                }
            }
        }

        // Do a final check for if CalculateFully is true
        if (Result_Difference > MaximumDifferentPixelCount)
        {
            Array.Copy(newFrameData, PreviousFrameData, newFrameData.Length);
            return true;
        }

        return false;
    }
}