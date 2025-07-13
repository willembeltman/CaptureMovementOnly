using System.Collections.Concurrent;

namespace CaptureOnlyMovements.Helpers;

public class FpsCounter
{
    private readonly ConcurrentQueue<DateTime> Queue = new ConcurrentQueue<DateTime>();

    public int SampleLengthInSeconds = 10;

    public void Tick()
    {
        Queue.Enqueue(DateTime.Now);
    }

    public double CalculateFps()
    {
        var now = DateTime.Now.AddSeconds(-SampleLengthInSeconds);
        while (Queue.Count > 0 && Queue.Min() < now)
        {
            Queue.TryDequeue(out var value);
        }
        return Convert.ToDouble(Queue.Count) / SampleLengthInSeconds;
    }
}
