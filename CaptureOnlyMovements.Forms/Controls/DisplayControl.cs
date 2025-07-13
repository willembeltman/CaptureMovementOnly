using CaptureOnlyMovements.Types;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace CaptureOnlyMovements.Forms.Controls;

public class DisplayControl : UserControl
{
    public Resolution Resolution { get; private set; }
    public Resizer? Resizer { get; private set; }

    private Frame? FrameBuffer1;
    private Frame? FrameBuffer2;
    private bool FrameBufferSwitch;

    public bool Dirty { get; private set; }

    private Frame? RenderFrame => FrameBufferSwitch ? FrameBuffer1 : FrameBuffer2;
    private void SetInputFrame(Frame value)
    {
        if (!FrameBufferSwitch)
        {
            FrameBuffer1 = value;
        }
        else
        {
            FrameBuffer2 = value;
        }
        FrameBufferSwitch = !FrameBufferSwitch;
        Dirty = true;
    }

    public DisplayControl()
    {
        BackColor = Color.Black;
        Resize += DisplayControl_Resize;
        Load += DisplayControl_Load;
    }

    private void DisplayControl_Load(object? sender, EventArgs e)
    {
        DisplayControl_Resize(this, new System.EventArgs()); // Let op als eerste
        FrameBuffer1 = new Frame(Resolution);
        FrameBuffer2 = new Frame(Resolution);
    }

    private void DisplayControl_Resize(object? sender, System.EventArgs e)
    {
        Resolution = new Resolution(Width, Height);
        Resizer = new Resizer(Resolution);
    }

    public void SetFrame(Frame frame)
    {
        if (Resizer == null) return;
        if (frame.Resolution != Resolution)
        {
            frame = Resizer.Resize(frame);
        }
        SetInputFrame(frame);
    }

    public void RenderLoop()
    {
        // Make a local copy of RenderFrame to avoid potential threading issues
        var renderFrame = RenderFrame;
        if (renderFrame == null || !Dirty) return;

        // Add GPU logic here to render the frame

        Dirty = false;
    }
}
