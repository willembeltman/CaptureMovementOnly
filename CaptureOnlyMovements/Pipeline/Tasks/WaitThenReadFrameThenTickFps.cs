using CaptureOnlyMovements.DirectX;
using CaptureOnlyMovements.Helpers;
using CaptureOnlyMovements.Interfaces;
using CaptureOnlyMovements.Types;

namespace CaptureOnlyMovements.Pipeline.Tasks;

public class WaitThenReadFrameThenTickFps : IFrameReader
{
    public WaitThenReadFrameThenTickFps(WaitForNext_DateTime waitTillNextTime, ScreenshotCapturer reader, FpsCounter inputFps, IKillSwitch killSwitch)
    {
        WaitTillNextTime = waitTillNextTime;
        Reader = reader;
        InputFps = inputFps;
        KillSwitch = killSwitch;
    }

    public WaitForNext_DateTime WaitTillNextTime { get; }
    public ScreenshotCapturer Reader { get; }
    public FpsCounter InputFps { get; }
    public IKillSwitch KillSwitch { get; }

    public Frame? ReadFrame(Frame? frame = null)
    { 
        // Wait for the next frame time and save previous frame date (if needed)
        WaitTillNextTime.Wait();

        // Capture the next frame
        frame = Reader.CaptureFrame(KillSwitch, frame?.Buffer);
        InputFps.Tick();

        return frame;
    }
}