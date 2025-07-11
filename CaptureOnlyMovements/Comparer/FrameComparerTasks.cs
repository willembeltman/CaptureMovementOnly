using CaptureOnlyMovements.Types;

namespace CaptureOnlyMovements.Comparer;

public class FrameComparerTasks : IDisposable
{
    static int linesPerTask = 16;


    public FrameComparerTasks(
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
        DrawCalculationFrame = calculateFully;
    }

    public int MaximumPixelDifferenceValue { get; }
    public long MaximumDifferentPixelCount { get; }
    public Resolution Resolution { get; }
    public bool[] CalculationFrameData { get; }
    public byte[] PreviousFrameData { get; }
    public bool DrawCalculationFrame { get; }

    public bool IsDifferent(byte[] newFrameData)
    {
        // Calculate stride
        var stride = Resolution.Width * 3;

        // Counter for total difference in frame 
        var Result_Difference = 0;

        var tasksCount = (int)Math.Ceiling(Convert.ToDouble(Resolution.Height / linesPerTask));
        var tasks = new Task[tasksCount];

        var result = false;

        for (int taskIndex = 0; taskIndex < tasksCount; taskIndex++)
        {
            tasks[taskIndex] = Task.Run(() =>
            {
                int endY = Math.Min(taskIndex + linesPerTask, Resolution.Height);
                for (int y = taskIndex; y < endY; y++)
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
                        
                        int pixelColorDifference1 = Math.Abs(currentRed - previousRed);
                        int pixelColorDifference2 = Math.Abs(currentGreen - previousGreen);
                        int pixelColorDifference3 = Math.Abs(currentBlue - previousBlue);
                        var pixelColorDifference = pixelColorDifference1 + pixelColorDifference2 + pixelColorDifference3;
                        var isDifferent = pixelColorDifference > MaximumPixelDifferenceValue;
                        
                        if (DrawCalculationFrame)
                            CalculationFrameData[y * Resolution.Width + x] = isDifferent;
                        
                        if (isDifferent)
                            Result_Difference++;

                        if (Result_Difference > MaximumDifferentPixelCount)
                            result = true;

                        if (result && !DrawCalculationFrame)
                            return true;
                    }
                }
                return false;
            });
        }

        Task.WaitAll(tasks);

        if (result)
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