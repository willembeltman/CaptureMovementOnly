using CaptureOnlyMovements.Interfaces;
using CaptureOnlyMovements.Pipeline.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading;

namespace CaptureOnlyMovements.Pipeline.Base;

public abstract class BasePipeline : IDisposable
{
    protected BasePipeline(
        BasePipeline? firstPipeline,
        BasePipeline? previousPipeline, 
        string name,
        IConsole? console)
    {
        FirstPipeline = firstPipeline ?? this;
        PreviousPipeline = previousPipeline;
        Thread = new Thread(Kernel);
        Name = name;
        Console = console;
    }

    private readonly BasePipeline FirstPipeline;
    private readonly BasePipeline? PreviousPipeline;
    protected readonly Thread Thread;
    protected readonly AutoResetEvent FrameReceived = new(false);
    protected readonly AutoResetEvent FrameDone = new(true);
    protected readonly IConsole? Console;
    protected int FrameIndex;
    protected bool Disposing;

    public INextVideoPipeline? NextPipeline { get; protected set; }
    public IMaskWriter? NextMaskWriter { get; protected set; }
    public INextMaskPipeline? NextMaskPipeline { get; protected set; }

    public string Name { get; }

    protected abstract void Kernel(object? obj);

    public void Stop()
    {
        if (Disposing)
            return;

        FrameDone.WaitOne();
        Disposing = true;
        FrameReceived.Set();
    }
    public void StopAll()
    {
        foreach (var p in AllPipelines)
            ((INextVideoPipeline)p).Stop();
    }
    public IEnumerable<BasePipeline> AllPipelines
    {
        get
        {
            var item = FirstPipeline;
            yield return item;
            while (item?.NextPipeline != null)
            {
                item = item.NextPipeline as BaseVideoPipeline;
                yield return item!;
            }
        }
    }

    public virtual void Dispose()
    {
        PreviousPipeline?.Dispose();
        Disposing = true;
        if (Thread != null && Thread.IsAlive)
            Thread.Join();
        GC.SuppressFinalize(this);
    }
}


