using CaptureOnlyMovements.DirectX;
using CaptureOnlyMovements.FFMpeg;
using CaptureOnlyMovements.FrameComparers;
using CaptureOnlyMovements.Helpers;
using CaptureOnlyMovements.Interfaces;
using CaptureOnlyMovements.Types;
using System;
using System.IO;
using System.Threading;

namespace CaptureOnlyMovements;

public class Recorder(
    IApplication Application,
    IConsole Console,
    IConsole FFMpegWriterConsole)
    : IDisposable, IKillSwitch
{
    private Thread? WriterThread;

    public bool Recording { get; private set; }
    public bool KillSwitch { get; private set; }
    public Config Config => Application.Config;

    public void Start()
    {
        if (!Recording)
        {
            Console.WriteLine("Starting the recorder...");
            WriterThread = new Thread(Kernel) { IsBackground = true };
            WriterThread.Start();
        }
    }
    public void Stop()
    {
        if (Recording)
        {
            Console.WriteLine("Stopping the recorder...");
            KillSwitch = true;
        }
    }
    private void Kernel()
    {
        Recording = true;
        KillSwitch = false;
        Application.Config.OnChangedState();

        try
        {
            // Get the filename for the output video
            string videosFolderPath = Environment.GetFolderPath(Environment.SpecialFolder.MyVideos);
            var outputName = $"{DateTime.Now:yyyy-MM-dd HH-mm-ss}.mkv";
            string outputFullName = Path.Combine(videosFolderPath, outputName);

            if (File.Exists(outputFullName))
                File.Delete(outputFullName); // Delete existing file

            Console.WriteLine($"Opening '{outputFullName}' to write video to.");

            // Get first frame for the resolution
            using var capturer = new ScreenshotCapturer();
            var frame = capturer.CaptureFrame();
            Application.InputFps.Tick();

            // Create the comparer
            var resolution = frame.Resolution;
            using var comparer = new FrameComparerUnsafe(Config, resolution);
            comparer.IsDifferent(frame.Buffer); // Initialize comparer with the first frame

            // Create the writer
            var writerInfo = new MediaInfo(outputFullName);
            using var writer = writerInfo.OpenVideoWriter(this, resolution, Config, FFMpegWriterConsole);

            // Write the first frame to the video
            writer.WriteFrame(frame.Buffer);
            Application.OutputFps.Tick();
            Console.WriteLine($"Captured frame at {DateTime.Now:HH:mm:ss.fff}   -");

            // Remember the previous frame date for timing
            var previousFrameDate = DateTime.Now;

            // Iterate through next frames until the kill switch is activated
            while (!KillSwitch)
            {
                // Capture the next frame
                frame = capturer.CaptureFrame(frame.Buffer);
                Application.InputFps.Tick();

                // Check if the frame is different from the previous one
                if (comparer.IsDifferent(frame.Buffer))
                {
                    // If so write it to the video
                    writer.WriteFrame(frame.Buffer);
                    Application.OutputFps.Tick();
                    Console.WriteLine($"Captured frame at {DateTime.Now:HH:mm:ss.fff}   {comparer.Difference}");

                    // Wait for the next frame time and save previous frame date
                    previousFrameDate = WaitForNextDateTimeHelper.Wait(Config, previousFrameDate);
                }
            }

            Console.WriteLine($"Closed '{outputFullName}' and stopped capturing.");
        }
        catch (Exception ex)
        {
            Application.FatalException(ex.Message, "Error while recording");
        }
        finally
        {
            Recording = false;
            Application.Config.OnChangedState();
        }
    }

    public void FatalException(Exception exception) => Application.FatalException(exception);

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
