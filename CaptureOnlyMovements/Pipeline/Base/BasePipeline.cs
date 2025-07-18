using System;
using System.Threading;

namespace CaptureOnlyMovements.Pipeline.Base;

public abstract class BasePipeline : IDisposable
{
    protected BasePipeline(BasePipeline? previousPipeline, string name)
    {
        PreviousPipeline = previousPipeline;
        Thread = new Thread(Kernel);
        Name = name;
    }

    private readonly BasePipeline? PreviousPipeline;
    protected readonly Thread Thread;
    protected readonly AutoResetEvent FrameReceived = new AutoResetEvent(false);
    protected readonly AutoResetEvent FrameDone = new AutoResetEvent(true);
    protected int FrameIndex;
    protected bool Disposing;

    public string Name { get; }

    protected abstract void Kernel(object? obj);

    public virtual void Dispose()
    {
        PreviousPipeline?.Dispose();
        Disposing = true;
        if (Thread != null && Thread.IsAlive)
        {
            Thread.Join();
        }
        GC.SuppressFinalize(this);
    }
}


