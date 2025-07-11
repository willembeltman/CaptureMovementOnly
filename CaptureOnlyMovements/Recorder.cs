using CaptureOnlyMovements;
using CaptureOnlyMovements.Comparer;
using CaptureOnlyMovements.DirectX;
using CaptureOnlyMovements.FFMpeg;
using CaptureOnlyMovements.Interfaces;
using System;
using System.IO;
using System.Linq;
using System.Threading;

namespace CaptureOnlyMovements;

public delegate void StateUpdated(bool recording);
public delegate void DebugUpdated(string line);

public class Recorder : IDisposable, IKillSwitch, IDebugWriter
{
    public Recorder()
    {
        Config = Config.Read();
    }

    public Config Config;

    public bool Running { get; private set; }
    public bool KillSwitch { get; private set; }

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

        // Get the path to the current user's Videos folder
        string videosFolderPath = Environment.GetFolderPath(Environment.SpecialFolder.MyVideos);

        // Define your desired output filename
        var outputName = $"{DateTime.Now:yyyy-MM-dd HH-mm-ss}.mp4";

        // Combine the path and the filename to get the full output path
        string outputFullName = Path.Combine(videosFolderPath, outputName);

        if (File.Exists(outputFullName))
            File.Delete(outputFullName); // Delete existing file

        WriteLine($"Opening '{outputFullName}' to write video to.");
        WriteLine($"Press Escape to stop capturing at any time.");

        // Get first frame for the resolution
        using var capturer = new ScreenshotCapturer();
        var outputMediaContainer = new MediaContainer(outputFullName);
        var frame = capturer.CaptureFrame();

        using var outputVideoStreamWriter = outputMediaContainer
            .OpenVideoStreamWriter(
                frame.Resolution, 
                Config.OutputFps, 
                Config.OutputQuality,
                Config.OutputPreset, 
                Config.UseGpu);
        outputVideoStreamWriter.WriteFrame(frame.Buffer, this);

        var comparer = new FrameComparer(Config, frame.Resolution);
        var pipeline = capturer.ReadEnumerable(this)
            .WaitForNext(Config)
            .Where(comparer.IsDifferent);
        outputVideoStreamWriter.WriteEnumerable(pipeline, this);

        WriteLine($"Closed '{outputFullName}' and stopped capturing.");

        Running = false;
        StateUpdated?.Invoke(Running);
    }

    public void WriteLine(string message)
    {
        DebugUpdated?.Invoke(message);
    }

    public void Dispose()
    {
        KillSwitch = true;
        GC.SuppressFinalize(this);
    }
}
