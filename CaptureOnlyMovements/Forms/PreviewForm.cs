using CaptureOnlyMovements.Interfaces;
using CaptureOnlyMovements.Types;
using System;
using System.Windows.Forms;

namespace CaptureOnlyMovements.Forms;

public partial class MaskForm : Form, IPreview
{
    public MaskForm(IApplication application)
    {
        InitializeComponent();
        Application = application;
        Config.StateChanged += StateChanged;
        Icon = new System.Drawing.Icon("Computer.ico");
    }

    public IApplication Application { get; }
    public Config Config => Application.Config;
    public bool ShowMask => Visible;
    public bool ShowPreview => false;

    private void StateChanged()
    {
        if (InvokeRequired)
        {
            Invoke(new Action(() => StateChanged()));
            return;
        }

        if (Application.IsBusy)
        {
            Icon = new System.Drawing.Icon("ComputerRecording.ico");
        }
        else
        {
            Icon = new System.Drawing.Icon("Computer.ico");
        }
    }

    public void WriteMask(BwFrame mask)
    {
        displayControl1.WriteMask(mask);
    }
    public void WriteFrame(Frame frame)
    {
        throw new Exception("Should not be called because we return false in ShowPreview");
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
            components.Dispose();
        }
        if (disposing)
        {
            Config.StateChanged -= StateChanged;
        }
        base.Dispose(disposing);
    }

}
