using CaptureOnlyMovements.Comparer;
using CaptureOnlyMovements.DirectX;
using CaptureOnlyMovements.FFMpeg;
using CaptureOnlyMovements.Interfaces;
using System;
using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace CaptureOnlyMovements.Forms;

public delegate void StateUpdated(bool recording);
public delegate void DebugUpdated(string line);

public class Recorder : IDisposable, IKillSwitch, IDebugWriter, IFFMpegDebugWriter
{
    public Recorder(IApplication application)
    {
        Application = application;
        Config = Config.Read();
    }

    private readonly IApplication Application;
    private Thread? WriterThread;

    public Config Config { get; }
    public bool Running { get; private set; }
    public bool KillSwitch { get; private set; }

    public event StateUpdated? StateUpdated;
    public event DebugUpdated? DebugUpdated;
    public event DebugUpdated? FFMpegDebugUpdated;

    public void Start()
    {
        if (!Running)
        {
            DebugWriteLine("Starting the recorder...");
            WriterThread = new Thread(Kernel) { IsBackground = true };
            WriterThread.Start();
        }
    }
    public void Stop()
    {
        if (Running)
        {
            DebugWriteLine("Stopping the recorder...");
            KillSwitch = true;
        }
    }
    private void Kernel()
    {
        Running = true;
        KillSwitch = false;

        try
        {
            // Get the path to the current user's Videos folder
            string videosFolderPath = Environment.GetFolderPath(Environment.SpecialFolder.MyVideos);

            // Define your desired output filename
            var outputName = $"{DateTime.Now:yyyy-MM-dd HH-mm-ss}.mp4";

            // Combine the path and the filename to get the full output path
            string outputFullName = Path.Combine(videosFolderPath, outputName);

            if (File.Exists(outputFullName))
                File.Delete(outputFullName); // Delete existing file

            DebugWriteLine($"Opening '{outputFullName}' to write video to.");
            DebugWriteLine($"Press Escape to stop capturing at any time.");

            // Get first frame for the resolution
            using var capturer = new ScreenshotCapturer();
            var frame = capturer.CaptureFrame();
            var resolution = frame.Resolution;

            var container = new MediaContainer(outputFullName);
            using var writer = container.OpenWriter(this, this, resolution, Config);
            writer.WriteFrame(frame.Buffer);

            StateUpdated?.Invoke(Running);

            var comparer = new FrameComparer(Config, resolution);
            var previousDate = DateTime.Now;

            while (!KillSwitch)
            {
                frame = capturer.CaptureFrame(frame.Buffer);
                if (comparer.IsDifferent(frame.Buffer))
                {
                    writer.WriteFrame(frame.Buffer);
                    DebugWriteLine($"Captured frame at {DateTime.Now:HH:mm:ss.fff}   {comparer.Result_Difference}");
                    previousDate = WaitForNextHelper.Wait(Config, previousDate);
                }
            }

            DebugWriteLine($"Closed '{outputFullName}' and stopped capturing.");

            Running = false;
            StateUpdated?.Invoke(Running);
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Error while recording", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    public void FatalException(string error)
    {
        MessageBox.Show(error, "Fatal exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
        Application.Exit();
    }
    public void DebugWriteLine(string message) => DebugUpdated?.Invoke(message);
    public void FFMpegDebugWriteLine(string message) => FFMpegDebugUpdated?.Invoke(message);

    public void Dispose()
    {
        KillSwitch = true;
        // De form thread roept dit aan, die laat de thread vervolgens netjes afsluiten,
        // Maar tijdens het afsluiten wil de recorder thread het form aanroepen om de status te updaten.
        // Deze staat dan hieronder te wachten tot de recorder thread klaar is. Dus DEADLOCK.
        // Programma mag zich gewoon afsluiten terwijl de recorder thread zichzelf apart afsluit.
        //if (Running && WriterThread != null && WriterThread != Thread.CurrentThread)
        //    WriterThread.Join();
        GC.SuppressFinalize(this);
    }

}
