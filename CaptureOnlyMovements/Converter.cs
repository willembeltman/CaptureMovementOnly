using CaptureOnlyMovements.Comparer;
using CaptureOnlyMovements.FFMpeg;
using CaptureOnlyMovements.Interfaces;
using CaptureOnlyMovements.Types;
using System.ComponentModel;

namespace CaptureOnlyMovements;

public delegate void ChangeStateDelegate(bool running);
public class Converter(IApplication Application, BindingList<FileConfig> Files) : IKillSwitch, IDisposable
{
    private Thread? WriterThread;

    public bool Converting { get; private set; }
    public bool KillSwitch { get; private set; }

    public void Start()
    {
        if (!Converting)
        {
            Application.DebugWriteLine("Starting the recorder...");
            WriterThread = new Thread(Kernel);
            WriterThread.Start();
        }
    }
    public void Stop()
    {
        if (Converting)
        {
            Application.DebugWriteLine("Stopping the recorder...");
            KillSwitch = true;
        }
    }

    private void Kernel(object? obj)
    {
        Converting = true;
        KillSwitch = false;
        Application.Config.OnChangedState();

        try
        {
            // Get the path to the current user's Videos folder
            string videosFolderPath = Environment.GetFolderPath(Environment.SpecialFolder.MyVideos);

            // Define your desired output filename
            var outputName = $"Converted {DateTime.Now:yyyy-MM-dd HH-mm-ss}.mp4";

            // Combine the path and the filename to get the full output path
            string outputFullName = Path.Combine(videosFolderPath, outputName);

            if (File.Exists(outputFullName))
                File.Delete(outputFullName); // Delete existing file

            DebugWriteLine($"Opening '{outputFullName}' to write video to.");

            var width = Files.Max(a => a.Width) ?? throw new Exception("Could not determine resolution");
            var height = Files.Max(a => a.Height) ?? throw new Exception("Could not determine resolution");
            var resolution = new Resolution(width, height);

            var writercontainer = new MediaContainer(outputFullName);
            using var writer = writercontainer.OpenVideoWriter(Application, this, resolution, Application.Config);

            var fileItemMediaContainers = Files
                .Where(a => a.FullName != null)
                .Select(a => new FileItemMediaContainer(a));

            foreach (var fileItemMediaContainer in fileItemMediaContainers)
            {
                DebugWriteLine($"Opening '{fileItemMediaContainer.FileConfig.FullName}' to read video from.");

                using var reader = fileItemMediaContainer.MediaContainer.OpenVideoReader(Application, this);
                var frame = reader.ReadFrame();
                if (frame == null) continue;

                var comparer = new FrameComparer(fileItemMediaContainer.FileConfig, frame.Resolution);
                var resizer = new RgbResizer(resolution);

                // Pre-compare frame
                comparer.IsDifferent(frame.Buffer);

                // Resize frame
                frame = resizer.Resize(frame);

                // Write frame
                writer.WriteFrame(frame.Buffer);
                Application.InputFps.Tick();
                DebugWriteLine($"Captured frame at {DateTime.Now:HH:mm:ss.fff}   -");

                while (!KillSwitch)
                {
                    // Read frame
                    frame = reader.ReadFrame(frame.Buffer);
                    if (frame == null) break;

                    Application.InputFps.Tick();

                    // Compare frame
                    if (!comparer.IsDifferent(frame.Buffer)) continue;

                    // Resize frame
                    frame = resizer.Resize(frame);

                    // Write frame
                    writer.WriteFrame(frame.Buffer);
                    Application.OutputFps.Tick();

                    // Set screenshot
                    Application.SetPreview(comparer.CalculationFrameData, comparer.Resolution);

                    DebugWriteLine($"Captured frame at {DateTime.Now:HH:mm:ss.fff}   {comparer.Result_Difference}");
                }

                if (KillSwitch) break;
            }

            DebugWriteLine($"Closed '{outputFullName}' and stopped capturing.");
        }
        catch (Exception ex)
        {
            Application.FatalException(ex.Message, "Error while recording");
        }
        finally
        {
            Converting = false;
            Application.Config.OnChangedState();
        }
    }

    public void FatalException(Exception exception) => Application.FatalException(exception.Message, "Fatal exception");
    public void DebugWriteLine(string message) => Application.DebugWriteLine(message);
    public void FFMpegDebugWriteLine(string message) => Application.FFMpegDebugWriteLine(message);

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