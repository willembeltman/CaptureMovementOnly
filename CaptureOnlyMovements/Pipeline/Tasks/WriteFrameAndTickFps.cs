using CaptureOnlyMovements.FFMpeg.Types;
using CaptureOnlyMovements.Interfaces;
using CaptureOnlyMovements.Pipeline.Interfaces;
using CaptureOnlyMovements.Types;
using System;

namespace CaptureOnlyMovements.Pipeline.Tasks;

public class WriteFrameAndTickFps : IFrameWriter
{
    public WriteFrameAndTickFps(VideoStreamWriter writer, FpsCounter outputFps, IConsole? console = null)
    {
        Writer = writer;
        OutputFps = outputFps;
        Console = console;
    }

    public VideoStreamWriter Writer { get; }
    public FpsCounter OutputFps { get; }
    public IConsole? Console { get; }

    public void WriteFrame(Frame frame)
    {
        Writer.WriteFrame(frame);
        OutputFps.Tick();
        Console?.WriteLine($"Captured frame at {DateTime.Now:HH:mm:ss.fff}");
    }
}