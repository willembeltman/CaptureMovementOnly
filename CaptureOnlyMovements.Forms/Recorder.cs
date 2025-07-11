using CaptureOnlyMovements;
using CaptureOnlyMovements.Comparer;
using CaptureOnlyMovements.DirectX;
using CaptureOnlyMovements.FFMpeg;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

public delegate void StateUpdated(bool recording);
public delegate void DebugUpdated(string line);

public class Recorder
{
    public Recorder()
    {
        Config = Config.Read();
    }

    public List<string> DebugLines = [];

    public Config Config { get; }
    public bool Running {get; private set; } = false;
    public bool KillSwitch { get; set; }
    public event StateUpdated? StateUpdated;
    public event DebugUpdated? DebugUpdated;

    public void Start()
    {
        WriteLine("Starting the recorder...");
        (new Thread(Kernel) { IsBackground = true }).Start();
    }

    public void Stop()
    {
        WriteLine("Stopping the recorder...");
        KillSwitch = true;
    }

    private void Kernel()
    {
        Running = true;
        KillSwitch = false;
        StateUpdated?.Invoke(Running);

        var FFMpegDirectory = new DirectoryInfo(Environment.CurrentDirectory);

        // Get the path to the current user's Videos folder
        string videosFolderPath = Environment.GetFolderPath(Environment.SpecialFolder.MyVideos);

        // Define your desired output filename
        var OutputName = $"{DateTime.Now:yyyy-MM-dd HH-mm-ss}.mp4";

        // Combine the path and the filename to get the full output path
        string OutputFullName = Path.Combine(videosFolderPath, OutputName);

        if (File.Exists(OutputFullName))
            File.Delete(OutputFullName); // Delete existing file

        WriteLine($"Opening '{OutputFullName}' to write video to.");
        WriteLine($"Press Escape to stop capturing at any time.");

        using var capturer = new ScreenshotCapturer();

        // Get first frame for the resolution
        var frame = capturer.CaptureFrame();
        var outputMediaContainer = new MediaContainer(FFMpegDirectory, OutputFullName);
        var comparer = new FrameComparer(Config.MaximumPixelDifferenceValue, Config.MaximumDifferentPixelCount, frame.Resolution);

        using var outputVideoStreamWriter = outputMediaContainer
            .OpenVideoStreamWriter(frame.Resolution, Config.OutputFPS, Config.OutputCRF, Config.Preset, Config.UseQuickSync);

        outputVideoStreamWriter.WriteFrame(frame.Buffer);
        WriteLine($"Captured frame at {DateTime.Now:HH:mm:ss.fff}");

        while (!KillSwitch)
        {
            var minWait = TimeSpan.FromMilliseconds(1000.0 / Config.OutputFPS * Config.MinPlaybackSpeed);
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

            WriteLine($"Captured frame at {DateTime.Now:HH:mm:ss.fff}");
        }

        WriteLine($"Closed '{OutputFullName}' and stopped capturing.");

        Running = false;
        StateUpdated?.Invoke(Running);
    }

    private void WriteLine(string message)
    {
        DebugLines.Add(message);
        while (DebugLines.Count > Config.MaxLinesInDebug)
            DebugLines.RemoveAt(0); // Keep the list size manageable
        DebugUpdated?.Invoke(message);
    }
}