using System.Collections.Concurrent;

namespace CaptureOnlyMovements.Helpers;

public class FpsCounter
{
    private readonly ConcurrentQueue<DateTime> Queue = new();

    public int SampleLengthInSeconds = 2;

    public void Tick()
    {
        Queue.Enqueue(DateTime.Now);
    }

    public double CalculateFps()
    {
        var now = DateTime.Now.AddSeconds(-SampleLengthInSeconds);
        while (!Queue.IsEmpty && Queue.Min() < now)
        {
            Queue.TryDequeue(out _);
        }
        return Convert.ToDouble(Queue.Count) / SampleLengthInSeconds;
    }
}
