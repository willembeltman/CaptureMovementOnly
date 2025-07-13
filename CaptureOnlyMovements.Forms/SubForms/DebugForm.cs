using System;
using System.Windows.Forms;

namespace CaptureOnlyMovements.Forms;

public partial class DebugForm : Form
{
    public ApplicationForm Application { get; }

    public DebugForm(ApplicationForm application)
    {
        InitializeComponent();

        Application = application;
            Application.DebugUpdated += Application_DebugUpdated;
    }

    private void Application_DebugUpdated(string line)
    {
        if (InvokeRequired)
        {
            Invoke(new Action(() => Application_DebugUpdated(line)));
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
            Application.DebugUpdated -= Application_DebugUpdated;
            components.Dispose();
        }
        base.Dispose(disposing);
    }
}
