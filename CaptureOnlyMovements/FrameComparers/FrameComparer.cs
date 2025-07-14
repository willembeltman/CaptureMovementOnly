using CaptureOnlyMovements.Interfaces;
using CaptureOnlyMovements.Types;

namespace CaptureOnlyMovements.FrameComparers;

public class FrameComparer(
    IComparerConfig config,
    Resolution resolution,
    IPreview? preview = null) : IFrameComparer
{
    private readonly byte[] PreviousFrameData = new byte[resolution.Width * resolution.Height * 3];

    public Resolution Resolution { get; } = resolution;
    public bool[] MaskData { get; } = new bool[resolution.Width * resolution.Height];
    public int Difference { get; private set; }

    public bool IsDifferent(byte[] newFrameData)
    {
        // Calculate stride
        var stride = Resolution.Width * 3;

        // Counter for total difference in frame 
        Difference = 0;

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
                var isDifferent = pixelColorDifference > config.MaximumPixelDifferenceValue;

                if (preview?.ShowMask == true)
                {
                    MaskData[y * Resolution.Width + x] = isDifferent;
                }

                if (isDifferent)
                {
                    // Add difference to total difference
                    Difference++;
                }

                // Do check if total difference hasn't exceeded threshold otherwise no need to continue iterating
                if (Difference > config.MaximumDifferentPixelCount)
                {
                    if (preview?.ShowMask != true)
                    {
                        Array.Copy(newFrameData, PreviousFrameData, newFrameData.Length);
                        return true;
                    }
                }
            }
        }

        // Do a final check for if CalculateFully is true
        if (Difference > config.MaximumDifferentPixelCount)
        {
            Array.Copy(newFrameData, PreviousFrameData, newFrameData.Length);
            return true;
        }

        return false;
    }

    public void Dispose()
    {
    }
}