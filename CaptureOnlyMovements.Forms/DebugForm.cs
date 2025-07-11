using System;
using System.Windows.Forms;

namespace CaptureOnlyMovements.Forms;

public partial class DebugForm : Form
{
    public Recorder Recorder { get; }

    public DebugForm(Recorder recorder)
    {
        Recorder = recorder;
        InitializeComponent();

        Recorder.DebugUpdated += Recorder_DebugUpdated;
    }

    private void Recorder_DebugUpdated(string line)
    {
        if (InvokeRequired)
        {
            Invoke(new Action(() => Recorder_DebugUpdated(line)));
            return;
        }

        DebugList.Items.Add(line);
    }

    private void DebugForm_Load(object sender, EventArgs e)
    {
        DebugList.Items.Clear();
        foreach (var line in Recorder.DebugLines)
        {
            DebugList.Items.Add(line);
        }   
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
        {
            Recorder.DebugUpdated -= Recorder_DebugUpdated;
            components.Dispose();
        }
        base.Dispose(disposing);
    }
}
