using CaptureOnlyMovements.Interfaces;
using System.Windows.Forms;

namespace CaptureOnlyMovements.Forms;

public partial class DebugForm : Form, IConsole
{
    public DebugForm()
    {
        InitializeComponent();
    }

    public void WriteLine(string line)
    {
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
}
