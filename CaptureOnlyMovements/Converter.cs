using CaptureOnlyMovements.FFMpeg;
using CaptureOnlyMovements.FrameComparers;
using CaptureOnlyMovements.FrameResizers;
using CaptureOnlyMovements.Helpers;
using CaptureOnlyMovements.Interfaces;
using CaptureOnlyMovements.Pipeline;
using CaptureOnlyMovements.Pipeline.Tasks;
using CaptureOnlyMovements.Types;
using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Threading;

namespace CaptureOnlyMovements;

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
                using var resizer = new BgrResizerUnsafe(resolution);
                using var comparer = new FrameComparerTasks(frame.Resolution, fileConfig,  Preview);

                // Then initialize them
                comparer.IsDifferent(frame.Buffer);
                frame = resizer.Resize(frame);

                writer.WriteFrame(frame);
                Application.OutputFps.Tick();
                Preview.WriteFrame(frame);

                var skipTillNextIndex = new SkipTillNext_Index(fileConfig, Application);

                Console?.WriteLine($"Setting up pipeline.");

                // ## New correct method

                // Setup pipeline
                using var pipeline =
                    new VideoPipeline(new ReadFrameAndTickFps(reader, Application.InputFps), Console) // Start pipeline with reader,
                                .Next(new SkipInitialOrNotDifferentFrames(skipTillNextIndex, comparer, Preview)) // This step has 2 outputs: frames and masks
                                .Next(// Frame output:
                                      new ResizeFrame(resizer), // Will output to the next step of the pipeline
                                      // Mask output to a new pipeline:
                                      new MaskPipeline(Console) // Parent video pipeline will be the frame reader of the mask pipeline
                                                 .Next(new ShowMaskTo(Preview)))
                                .Next(new ShowPreviewTo_PassThrough(Preview))
                                .Next(new WriteFrameAndTickFps(writer, Application.OutputFps));

                Console?.WriteLine($"Starting the pipeline.");

                pipeline.Start(this);
                pipeline.WaitForExit();

                if (pipeline.Exception != null)
                    Application.Exception(pipeline.Exception);

                //// ## New "feed the pipeline yourself" method:
                //// (Again, no clue why this works, although I haven't even tested it I think)

                //// Setup pipeline
                //using var maskPipeline =
                //     new MaskPipeline(Console)
                //                .Next(new ShowMaskTo(Preview));
                //using var pipeline =
                //    new VideoPipeline(Console) 
                //                .Next(new SkipInitialOrNotDifferentFrames(skipTillNextIndex, comparer, Preview))
                //                .Next(new ResizeFrame(resizer), maskPipeline)
                //                .Next(new ShowPreviewTo_PassThrough(Preview))
                //                .Next(new WriteFrameAndTickFps(writer, Application.OutputFps));


                //pipeline.Start(this);

                //// Then capture frames from the reader, until the kill switch is activated
                //while (!KillSwitch)
                //{
                //    // Capture the next frame
                //    frame = reader.ReadFrame(frame);
                //    Application.InputFps.Tick();

                //    // Feed it into the pipeline
                //    pipeline.WriteFrame(frame);
                //}

                //// Then tell the pipeline we have stopped.
                //pipeline.Stop();


                //// ## Old single threaded / single buffer code
                //while (!KillSwitch)
                //{
                //    frame = reader.ReadFrame(frame);
                //    if (frame == null) break;
                //    Application.InputFps.Tick();
                //    frameIndex++;

                //    if (skipTillNextIndex.NeedToSkip(frameIndex)) continue;

                //    var isDifferent = comparer.IsDifferent(frame.Buffer);

                //    if (Preview.ShowMask)
                //    {
                //        var bwFrame = new BwFrame(comparer.MaskData, comparer.Resolution);
                //        Preview.WriteMask(bwFrame);
                //    }

                //    if (!isDifferent) continue;

                //    skipTillNextIndex.Reset(frameIndex);

                //    frame = resizer.Resize(frame);

                //    if (Preview.ShowPreview)
                //    {
                //        Preview.WriteFrame(frame);
                //    }

                //    writer.WriteFrame(frame);
                //    Application.OutputFps.Tick();

                //    Console?.WriteLine($"Captured frame {frameIndex}   {comparer.Difference}");
                //}

                if (KillSwitch) break;
            }

            Console?.WriteLine($"Closed '{outputFullName}' and stopped capturing.");
        }
        catch (Exception ex)
        {
            Application.FatalException(ex);
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