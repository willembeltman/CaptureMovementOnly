using CaptureOnlyMovements.DirectX;
using CaptureOnlyMovements.FFMpeg;
using CaptureOnlyMovements.FrameComparers;
using CaptureOnlyMovements.Helpers;
using CaptureOnlyMovements.Interfaces;
using CaptureOnlyMovements.Pipeline;
using CaptureOnlyMovements.Pipeline.Tasks;
using CaptureOnlyMovements.Types;
using System;
using System.IO;
using System.Threading;

namespace CaptureOnlyMovements;

public class Recorder(
    IApplication Application,
    IConsole Console,
    IConsole FFMpegWriterConsole,
    IPreview? Preview = null)
    : IDisposable, IKillSwitch
{
    private Thread? WriterThread;

    public bool Recording { get; private set; }
    public bool KillSwitch { get; private set; }
    public Config Config => Application.Config;
    public ConfigPrefixPreset? ConfigPrefix { get; private set; }

    public void Start(ConfigPrefixPreset configPrefix)
    {
        if (Recording) return;

        ConfigPrefix = configPrefix;
        Console.WriteLine("Starting the recorder...");
        WriterThread = new Thread(Kernel) { IsBackground = true };
        WriterThread.Start();
    }
    public void Stop()
    {
        if (!Recording) return;

        Console.WriteLine("Stopping the recorder...");
        KillSwitch = true;
    }
    private void Kernel()
    {
        Recording = true;
        KillSwitch = false;
        Application.Config.OnChangedState();

        try
        {
            if (ConfigPrefix == null) throw new Exception("No config chosen");

            // Get the filename for the output video
            string videosFolderPath = Environment.GetFolderPath(Environment.SpecialFolder.MyVideos);
            var outputName = $"{ConfigPrefix.OutputFileNamePrefix}{DateTime.Now:yyyy-MM-dd HH-mm-ss}.mkv";
            string outputFullName = Path.Combine(videosFolderPath, outputName);

            Console.WriteLine($"Starting screen capture.");

            // Get first frame for the resolution
            using var reader = new ScreenshotCapturer(Console);
            var frame = reader.CaptureFrame(this);
            var resolution = frame!.Resolution;
            Application.InputFps.Tick();

            // Create the comparer
            using var comparer = new FrameComparerTasks(resolution, Config, Preview);
            comparer.IsDifferent(frame.Buffer); // Initialize comparer with the first frame

            // Create the writer
            Console.WriteLine($"Opening '{outputFullName}' to write video to.");
            var writerInfo = new MediaInfo(outputFullName);
            using var writer = writerInfo.OpenVideoWriter(this, resolution, Config, FFMpegWriterConsole);
            Console.WriteLine($"Writing first frame to the video.");

            // Write the first frame to the video
            writer.WriteFrame(frame);
            Application.OutputFps.Tick();
            Console.WriteLine($"Captured frame at {DateTime.Now:HH:mm:ss.fff}");

            // Create additional wait helpers
            var waitTillNextTime = new WaitForNext_DateTime(Config, Application);
            var waitTillVisualStudioHasForcus = new WaitTillVisualStudioHasForcus(ConfigPrefix, this);

            Console.WriteLine($"Setting up pipeline.");

            // Setup pipeline
            using var pipeline =
                new VideoPipeline(new WaitThenReadFrameThenTickFps(waitTillVisualStudioHasForcus, waitTillNextTime, reader, Application.InputFps, this), Console)
                            .Next(new SkipNotDifferentFrames(comparer, Preview)) // This step has 2 outputs: frames and masks
                            .Next(// Frame output: 
                                  new ShowPreviewTo_PassThrough(Preview),
                                  // Mask output pipeline:
                                  new MaskPipeline(Console) // Parent video pipeline will be the frame reader
                                             .Next(new ShowMaskTo(Preview)))
                            .Next(new WriteFrameAndTickFps(writer, Application.OutputFps));

            Console.WriteLine($"Starting the pipeline.");

            // ## New correct method

            // Then start it
            pipeline.Start(this);

            // Sleep this thread until the pipeline is done
            pipeline.WaitForExit();

            // Then check if there were exceptions
            if (pipeline.Exception != null)
                Application.Exception(pipeline.Exception);


            //// ## New "feed the pipeline yourself" method 
            //// (I don't know how it is possible this works, maybe because of the wait?)

            //pipeline.Start(this); // Start it

            //// Then capture frames from the reader, until the kill switch is activated
            //while (!KillSwitch)
            //{
            //    // Wait for the next frame time, this call blocks the thread till wait-time
            //    // is timed out (if it is fast enough).
            //    waitTillNextTime.Wait();

            //    // Capture the next frame
            //    frame = reader.CaptureFrame(frame.Buffer);
            //    Application.InputFps.Tick();

            //    // Feed it into the pipeline
            //    pipeline.WriteFrame(frame);
            //}

            //// Then tell the pipeline we have stopped.
            //pipeline.Stop();


            //// ## Old single threaded / single buffer code
            //// Iterate through next frames until the kill switch is activated
            //while (!KillSwitch)
            //{
            //    // Wait for the next frame time and save previous frame date (if needed)
            //    waitTillNextTime.Wait();

            //    // Capture the next frame
            //    frame = reader.CaptureFrame(frame.Buffer);
            //    Application.InputFps.Tick();

            //    // Check if the frame is different from the previous one
            //    if (comparer.IsDifferent(frame.Buffer))
            //    {
            //        if (Preview?.ShowMask == true)
            //            Preview.WriteMask(new BwFrame(comparer.MaskData, comparer.Resolution));

            //        // If so write it to the video
            //        writer.WriteFrame(frame);
            //        Application.OutputFps.Tick();
            //        Console.WriteLine($"Captured frame at {DateTime.Now:HH:mm:ss.fff}   {comparer.Difference}");
            //    }
            //}

            Console.WriteLine($"Closed '{outputFullName}' and stopped capturing.");
        }
        catch (Exception ex)
        {
            Application.FatalException(ex);
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
