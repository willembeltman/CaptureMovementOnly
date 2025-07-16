using CaptureOnlyMovements.Types;
using System;
using System.Threading;

namespace CaptureOnlyMovements.Helpers;

public static class WaitForNextDateTimeHelper
{
    public static DateTime Wait(Config config, DateTime previousDate)
    {
        var minWait = TimeSpan.FromMilliseconds(1000.0 / config.OutputFps * config.MinPlaybackSpeed);
        var timespanSinceLastFrame = DateTime.Now - previousDate;
        if (timespanSinceLastFrame < minWait)
        {
            var sleep = minWait - timespanSinceLastFrame;
            Thread.Sleep(sleep);
        }
        previousDate = DateTime.Now;
        return previousDate;
    }
}