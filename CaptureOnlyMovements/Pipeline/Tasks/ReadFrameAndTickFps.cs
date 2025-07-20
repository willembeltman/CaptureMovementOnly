using CaptureOnlyMovements.FFMpeg.Types;
using CaptureOnlyMovements.Interfaces;
using CaptureOnlyMovements.Types;

namespace CaptureOnlyMovements.Pipeline.Tasks;

public class ReadFrameAndTickFps : IFrameReader
{
    public ReadFrameAndTickFps(VideoStreamReader reader, FpsCounter inputFps)
    {
        Reader = reader;
        Fps = inputFps;
    }

    public VideoStreamReader Reader { get; }
    public FpsCounter Fps { get; }

    public Frame? ReadFrame(Frame? frame = null)
    {
        frame = Reader.ReadFrame(frame);
        Fps.Tick();
        return frame;
    }
}