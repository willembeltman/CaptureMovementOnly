using CaptureOnlyMovements.FFMpeg.Types;
using CaptureOnlyMovements.Pipeline.Interfaces;
using CaptureOnlyMovements.Types;

namespace CaptureOnlyMovements.Pipeline.Tasks
{
    internal class WriteFrameAndTickFps : IFrameWriter
    {
        public WriteFrameAndTickFps(VideoStreamWriter writer, FpsCounter outputFps)
        {
            Writer = writer;
            OutputFps = outputFps;
        }

        public VideoStreamWriter Writer { get; }
        public FpsCounter OutputFps { get; }

        public void WriteFrame(Frame frame)
        {
            Writer.WriteFrame(frame);
            OutputFps.Tick();
        }
    }
}