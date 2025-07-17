using CaptureOnlyMovements.FFMpeg;
using CaptureOnlyMovements.FrameComparers;
using CaptureOnlyMovements.FrameResizers;
using CaptureOnlyMovements.Helpers;
using CaptureOnlyMovements.Interfaces;
using CaptureOnlyMovements.Types;
using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Threading;

namespace CaptureOnlyMovements;

public delegate void ChangeStateDelegate(bool running);
public class Converter(
    IApplication Application,
    IPreview Preview,
    IConsole? Console,
    IConsole? FFMpegReaderConsole,
    IConsole? FFMpegWriterConsole,
    BindingList<FileConfig> FileConfigs) : IKillSwitch, IDisposable
{
    private Thread? WriterThread;

    public bool Converting { get; private set; }
    public bool KillSwitch { get; private set; }

    public void Start()
    {
        if (!Converting)
        {
            Console?.WriteLine("Starting the recorder...");
            WriterThread = new Thread(Kernel);
            WriterThread.Start();
        }
    }
    public void Stop()
    {
        if (Converting)
        {
            Console?.WriteLine("Stopping the recorder...");
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
            // Get the filename for the output video
            string videosFolderPath = Environment.GetFolderPath(Environment.SpecialFolder.MyVideos);
            var outputName = $"Converted {DateTime.Now:yyyy-MM-dd HH-mm-ss}.mkv";
            string outputFullName = Path.Combine(videosFolderPath, outputName);

            if (File.Exists(outputFullName))
                File.Delete(outputFullName); // Delete existing file

            Console?.WriteLine($"Opening '{outputFullName}' to write video to.");

            // Determine the resolution from the file configurations
            var width = FileConfigs.Max(a => a.Width) ?? throw new Exception("Could not determine resolution");
            var height = FileConfigs.Max(a => a.Height) ?? throw new Exception("Could not determine resolution");
            var resolution = new Resolution(width, height);

            // Open the writer with the determined resolution
            var writerInfo = new MediaInfo(outputFullName);
            using var writer = writerInfo.OpenVideoWriter(this, resolution, Application.Config, FFMpegWriterConsole);

            foreach (var fileConfig in FileConfigs)
            {
                if (fileConfig.FullName == null) continue;

                Console?.WriteLine($"Opening '{fileConfig.FullName}' to read video from.");

                var frameIndex = 0;

                // Open the video reader for the current file configuration
                var readerInfo = new MediaInfo(fileConfig);
                using var reader = readerInfo.OpenVideoReader(this, FFMpegReaderConsole);

                // Read the first frame to initialize the comparer
                var frame = reader.ReadFrame();
                if (frame == null) continue;
                Application.InputFps.Tick();
                frameIndex++;

                // Create the frame comparer and resizer
                using var comparer = new FrameComparerTasks(fileConfig, frame.Resolution, Preview);
                using var resizer = new BgrResizerUnsafe(resolution);

                comparer.IsDifferent(frame.Buffer);

                frame = resizer.Resize(frame);

                writer.WriteFrame(frame.Buffer);
                Application.OutputFps.Tick();
                Console?.WriteLine($"Captured frame #0   -");
                Preview.SetPreview(frame);

                var skipTillNextIndex = new skipTillNextIndexHelper(fileConfig, Application);

                while (!KillSwitch)
                {
                    frame = reader.ReadFrame(frame.Buffer);
                    if (frame == null) break;
                    Application.InputFps.Tick();
                    frameIndex++;

                    if (skipTillNextIndex.NeedToSkip(frameIndex)) continue;

                    var isDifferent = comparer.IsDifferent(frame.Buffer);

                    if (Preview.ShowMask)
                    {
                        var bwFrame = new BwFrame(comparer.MaskData, comparer.Resolution);
                        Preview.SetMask(bwFrame);
                    }

                    if (!isDifferent) continue;

                    skipTillNextIndex.Reset(frameIndex);

                    frame = resizer.Resize(frame);

                    writer.WriteFrame(frame.Buffer);
                    Application.OutputFps.Tick();

                    if (Preview.ShowPreview)
                    {
                        Preview.SetPreview(frame);
                    }

                    Console?.WriteLine($"Captured frame {frameIndex}   {comparer.Difference}");
                }

                if (KillSwitch) break;
            }

            Console?.WriteLine($"Closed '{outputFullName}' and stopped capturing.");
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