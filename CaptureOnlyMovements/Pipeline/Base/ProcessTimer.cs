using System;
using System.Diagnostics;

namespace CaptureOnlyMovements.Pipeline.Base;

public class ProcessTimer
{
    public ProcessTimer()
    {
        StartStopwatch = Stopwatch.StartNew();
        Stopwatch = new Stopwatch();
    }

    public Stopwatch StartStopwatch { get; }
    public Stopwatch Stopwatch { get; }
    public int Count { get; set; }
    public double TotalTimeSpend => StartStopwatch.Elapsed.TotalSeconds;
    public double AverageFPS => 1 / (TotalTimeSpend / Count);
    public double TimeSpend => Stopwatch.Elapsed.TotalSeconds;
    public double AverageMS => TimeSpend * 1000 / Count;

    public Measurement NewMeasurement()
    {
        return new Measurement(this);
    }

    public class Measurement : IDisposable
    {
        public Measurement(ProcessTimer processTimer)
        {
            ProcessTimer = processTimer;
            ProcessTimer.Stopwatch.Start();
        }

        public ProcessTimer ProcessTimer { get; }

        public void Dispose()
        {
            ProcessTimer.Stopwatch.Stop();
            ProcessTimer.Count++;
        }
    }
}