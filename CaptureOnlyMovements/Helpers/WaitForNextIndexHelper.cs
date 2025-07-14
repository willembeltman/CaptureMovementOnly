using CaptureOnlyMovements.Interfaces;
using CaptureOnlyMovements.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CaptureOnlyMovements.Helpers;

public class WaitForNextIndexHelper
{
    private double interval;
    private double lastTime;
    private double inputFps;

    public WaitForNextIndexHelper(FileConfigInfo fileConfigInfo, IApplication application)
    {
        FileConfigInfo = fileConfigInfo;
        Application = application; 
        
        inputFps = fileConfigInfo.FileConfig.Fps ?? throw new Exception($"Fps not detected in video {fileConfigInfo.FileConfig.FullName}");
        var targetFps = Application.Config.OutputFps;
        var targetSpeed = fileConfigInfo.FileConfig.MinPlaybackSpeed;
        interval = 1d / targetFps * targetSpeed;
        lastTime = 0d;
    }

    public FileConfigInfo FileConfigInfo { get; }
    public IApplication Application { get; }

    public bool NeedToSkip(int frameIndex)
    {
        var time = Convert.ToDouble(frameIndex) / inputFps;
        if (interval < time - lastTime)
        {
            lastTime = time;
            return false;
        }
        else
        {
            return true; // Skip frames that are not needed based on the target speed
        }
    }
}
