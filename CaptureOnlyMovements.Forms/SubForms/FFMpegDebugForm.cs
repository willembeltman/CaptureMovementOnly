using System;
using System.Windows.Forms;

namespace CaptureOnlyMovements.Forms;

public partial class FFMpegDebugForm : Form
{
    public ApplicationForm Application { get; }

    public FFMpegDebugForm(ApplicationForm recorder)
    {
        InitializeComponent();

        Application = recorder;
        Application.FFMpegDebugUpdated += Recorder_DebugUpdated;
    }

    private void Recorder_DebugUpdated(string line)
    {
        if (InvokeRequired)
        {
            Invoke(new Action(() => Recorder_DebugUpdated(line)));
            return;
        }

        Console.WriteLine(line);
    }

    // Belangrijk: Overrides om te zorgen dat de applicatie niet sluit als het formulier gesloten wordt
    protected override void OnFormClosing(FormClosingEventArgs e)
    {
        // Voorkom dat het formulier direct sluit als de gebruiker op X klikt
        e.Cancel = true;
        this.Hide();
        base.OnFormClosing(e);
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
        {
            Application.DebugUpdated -= Recorder_DebugUpdated;
            components.Dispose();
        }
        base.Dispose(disposing);
    }
}
