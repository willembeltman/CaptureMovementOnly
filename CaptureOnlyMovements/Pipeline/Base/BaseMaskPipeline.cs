using CaptureOnlyMovements.Interfaces;
using CaptureOnlyMovements.Pipeline.Interfaces;
using CaptureOnlyMovements.Types;
using System;

namespace CaptureOnlyMovements.Pipeline.Base;

public abstract class BaseMaskPipeline : BasePipeline, INextMaskPipeline
{
    public BaseMaskPipeline(INextMaskPipeline? firstPipeline, BaseMaskPipeline? previousPipeline, string name) : base(previousPipeline, name)
    {
        FirstMaskPipeline = firstPipeline ?? this;
        PreviousMaskPipeline = previousPipeline;
    }

    public INextMaskPipeline FirstMaskPipeline { get; }
    protected IMaskPipeline? PreviousMaskPipeline { get; }
    protected BwFrame[]? Masks { get; set; }

    int IPipeline.Start(IKillSwitch? cancellationToken, int count) => StartMask(cancellationToken, count);
    protected virtual int StartMask(IKillSwitch? cancellationToken, int count)
    {
        count++;
        count = PreviousMaskPipeline?.Start(cancellationToken, count) ?? count;

        Masks = new BwFrame[count];
        Console.WriteLine($"{Name} Masks: {Masks.Length}x");
        Thread.Start(cancellationToken);

        return count;
    }
    public void WriteMask(BwFrame mask)
    {
        // Deze functie wordt aangeroepen op de Excecuter(laatste pipeline in de chain), maar die
        // moet natuurlijk naar de eerste pipeline in de chain gaan. 
        if (PreviousMaskPipeline == null)
        {
            // Als de previous pipeline null is, dan betekent dit dat dit de eerste pipeline is.
            // Dus de processMask mag worden aangeroepen, zodat de mask door de chain gaat.
            ((INextMaskPipeline)this).ProcessMask(mask);
        }
        else
        {
            // Als de previous pipeline niet null is, dan betekent dit dat er een chain is en 
            // moet het mask naar de eerste pipeline in de chain worden gestuurd.
            PreviousMaskPipeline.WriteMask(mask);
        }
    }
    void INextMaskPipeline.ProcessMask(BwFrame mask)
    {
        if (Masks == null) throw new Exception("How did you get here? What'd you do?");
        FrameDone.WaitOne();

        Masks[FrameIndex] = mask;

        FrameIndex++;
        if (FrameIndex >= Masks.Length)
        {
            FrameIndex = 0;
        }

        FrameReceived.Set();
    }
    void INextMaskPipeline.Stop()
    {
        if (Disposing) return;

        FrameDone.WaitOne();

        Disposing = true;

        FrameReceived.Set();
    }
}


