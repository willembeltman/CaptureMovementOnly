using CaptureOnlyMovements;
using System;
using System.Collections.Generic;
using System.Threading;

public static class WaitForNextExtender
{
    public static IEnumerable<byte[]> WaitForNext(this IEnumerable<byte[]> enumerable, Config config)
    {
        var date = DateTime.Now;
        foreach (var frame in enumerable)
        {
            var minWait = TimeSpan.FromMilliseconds(1000.0 / config.OutputFps * config.MinPlaybackSpeed);
            var timespanSinceLastFrame = DateTime.Now - date;
            if (timespanSinceLastFrame < minWait)
            {
                var sleep = minWait - timespanSinceLastFrame;
                Thread.Sleep(sleep);
            }
            date = DateTime.Now;
            yield return frame;
        }
    }
}