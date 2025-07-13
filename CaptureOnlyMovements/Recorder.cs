using CaptureOnlyMovements.FrameComparers;
using CaptureOnlyMovements.DirectX;
using CaptureOnlyMovements.FFMpeg;
using CaptureOnlyMovements.Interfaces;
using CaptureOnlyMovements.Types;
using CaptureOnlyMovements.Helpers;

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
            // Get the path to the current user's Videos folder
            string videosFolderPath = Environment.GetFolderPath(Environment.SpecialFolder.MyVideos);

            // Define your desired output filename
            var outputName = $"{DateTime.Now:yyyy-MM-dd HH-mm-ss}.mkv";

            // Combine the path and the filename to get the full output path
            string outputFullName = Path.Combine(videosFolderPath, outputName);

            if (File.Exists(outputFullName))
                File.Delete(outputFullName); // Delete existing file

            Console.WriteLine($"Opening '{outputFullName}' to write video to.");

            // Get first frame for the resolution
            using var capturer = new ScreenshotCapturer();
            var frame = capturer.CaptureFrame();
            var resolution = frame.Resolution;

            var comparer = new FrameComparer(Config, resolution);

            var container = new MediaContainer(outputFullName);
            using var writer = container.OpenVideoWriter(this, resolution, Config, FFMpegWriterConsole);
            
            writer.WriteFrame(frame.Buffer);
            Application.InputFps.Tick();
            Application.OutputFps.Tick();
            Console.WriteLine($"Captured frame at {DateTime.Now:HH:mm:ss.fff}   -");

            comparer.IsDifferent(frame.Buffer); // Initialize comparer with the first frame

            var previousDate = DateTime.Now;
            while (!KillSwitch)
            {
                frame = capturer.CaptureFrame(frame.Buffer);
                Application.InputFps.Tick();

                if (comparer.IsDifferent(frame.Buffer))
                {
                    writer.WriteFrame(frame.Buffer);
                    Application.OutputFps.Tick();
                    Console.WriteLine($"Captured frame at {DateTime.Now:HH:mm:ss.fff}   {comparer.Result_Difference}");

                    previousDate = WaitForNextHelper.Wait(Config, previousDate);
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
