﻿using CaptureOnlyMovements.Interfaces;
using CaptureOnlyMovements.Types;
using System;
using System.Threading;

namespace CaptureOnlyMovements.Helpers;

public class WaitForNext_DateTime
{
    public WaitForNext_DateTime(Config config, IApplication application)
    {
        Config = config;
        Application = application;
        previousDate = DateTime.Now;
    }

    public Config Config { get; }
    public IApplication Application { get; }

    private DateTime previousDate;

    public void Wait()
    {
        var minWait = TimeSpan.FromMilliseconds(1000.0 / Config.OutputFps * Config.MinPlaybackSpeed);
        var timespanSinceLastFrame = DateTime.Now - previousDate;
        if (timespanSinceLastFrame < minWait)
        {
            var sleep = minWait - timespanSinceLastFrame;
            Thread.Sleep((int)sleep.TotalMilliseconds);
        }
        previousDate = DateTime.Now;
    }
}