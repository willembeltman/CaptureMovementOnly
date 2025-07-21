using CaptureOnlyMovements.Interfaces;
using CaptureOnlyMovements.Types;
using System;

namespace CaptureOnlyMovements.Helpers;

public class SkipTillNext_Index
{
    private double interval;
    private double lastTime;
    private double inputFps;

    public SkipTillNext_Index(FileConfig fileConfig, IApplication application)
    {
        FileConfig = fileConfig;
        Application = application;

        inputFps = fileConfig.Fps ?? throw new Exception($"Fps not detected in video {fileConfig.FullName}");
        var targetFps = Application.Config.OutputFps;
        var targetSpeed = fileConfig.MinPlaybackSpeed;
        interval = 1d / targetFps * targetSpeed;
        lastTime = 0d;
    }

    public FileConfig FileConfig { get; }
    public IApplication Application { get; }

    public bool NeedToSkip(int frameIndex)
    {
        var time = Convert.ToDouble(frameIndex) / inputFps;
        if (interval < time - lastTime)
        {
            return false;
        }
        else
        {
            return true; // Skip frames that are not needed based on the target speed
        }
    }

    internal void Reset(int frameIndex)
    {
        var time = Convert.ToDouble(frameIndex) / inputFps;
        lastTime = time;
    }
}
