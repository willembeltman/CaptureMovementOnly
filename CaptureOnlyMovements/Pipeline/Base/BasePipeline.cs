using CaptureOnlyMovements.Interfaces;
using CaptureOnlyMovements.Pipeline.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading;

namespace CaptureOnlyMovements.Pipeline.Base;

public abstract class BasePipeline : IBasePipeline
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
        Statistics = new PipelineStatistics();
    }

    private readonly BasePipeline FirstPipeline;
    private readonly BasePipeline? PreviousPipeline;
    protected readonly Thread Thread;
    protected readonly AutoResetEvent FrameReceived = new(false);
    protected readonly AutoResetEvent FrameDone = new(true);
    protected readonly IConsole? Console;
    protected int FrameIndex;
    protected bool Disposing;

    public Exception? Exception { get; protected set; }
    public PipelineStatistics Statistics { get; }
    protected INextVideoPipeline? NextVideoPipeline { get; set; }
    protected INextMaskPipeline? NextMaskPipeline { get; set; }

    public string Name { get; }

    protected abstract void Kernel(object? obj);

    void IBasePipeline.Stop(Exception? ex)
    {
        Exception = ex;

        if (Disposing)
            return;

        FrameDone.WaitOne();
        Disposing = true;
        FrameReceived.Set();
    }
    public void StopAll(Exception? ex)
    {
        Exception = ex;

        foreach (var p in AllPipelines)
            ((INextVideoPipeline)p).Stop(ex);
    }
    public IEnumerable<BasePipeline> AllPipelines
    {
        get
        {
            var item = FirstPipeline;
            yield return item;
            while (item?.NextVideoPipeline != null)
            {
                item = item.NextVideoPipeline as BasePipeline;
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


