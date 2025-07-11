using CaptureOnlyMovements.Comparer;
using CaptureOnlyMovements.DirectX;
using CaptureOnlyMovements.FFMpeg;

namespace CaptureOnlyMovements;

internal class Program
{
    private static void Main(string[] args)
    {
        var config = Config.Read();

        var FFMpegDirectory = new DirectoryInfo(Environment.CurrentDirectory);
        var OutputFullName = $"{DateTime.Now:yyyy-MM-dd HH-mm}.mp4";

        var minWait = TimeSpan.FromMilliseconds(1000.0 / config.OutputFPS * config.MinPlaybackSpeed);

        if (File.Exists(OutputFullName))
            File.Delete(OutputFullName); // Delete existing file

        Console.WriteLine($"Opening '{OutputFullName}' to write video to.");
        Console.WriteLine($"Press Escape to stop capturing at any time.");

        using var capturer = new ScreenshotCapturer();

        // Get first frame for the resolution
        var frame = capturer.CaptureFrame();
        var outputMediaContainer = new MediaContainer(FFMpegDirectory, OutputFullName);
        var comparer = new FrameComparer(config.MaximumPixelDifferenceValue, config.MaximumDifferentPixelCount, frame.Resolution);

        using var outputVideoStreamWriter = outputMediaContainer
            .OpenVideoStreamWriter(frame.Resolution, config.OutputFPS, config.OutputCRF, config.Preset, config.UseQuickSync);

        outputVideoStreamWriter.WriteFrame(frame.Buffer);
        Console.WriteLine($"Captured frame at {DateTime.Now:HH:mm:ss.fff}");

        while (true)
        {
            if (Console.KeyAvailable)
            {
                var key = Console.ReadKey(true);
                if (key.Key == ConsoleKey.Escape)
                    break; // Exit on Escape key
            }

            var timespanSinceLastFrame = DateTime.Now - frame.CaptureDate;
            if (timespanSinceLastFrame < minWait)
            {
                var sleep = minWait - timespanSinceLastFrame;
                Thread.Sleep(sleep);
            }

            frame = capturer.CaptureFrame(frame.Buffer);

            if (!comparer.IsDifferent(frame.Buffer))
                continue;

            outputVideoStreamWriter.WriteFrame(frame.Buffer);

            Console.WriteLine($"Captured frame at {DateTime.Now:HH:mm:ss.fff}");
        }
    }
}