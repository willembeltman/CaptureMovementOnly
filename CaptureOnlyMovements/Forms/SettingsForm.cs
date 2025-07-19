using CaptureOnlyMovements.Interfaces;
using CaptureOnlyMovements.Types;
using System;
using System.Windows.Forms;

namespace CaptureOnlyMovements.Forms;

public partial class ConfigForm : Form
{
    public ConfigForm(IApplication application)
    {
        InitializeComponent();
        Application = application;
        VisibleChanged += ConfigForm_VisibleChanged;
        Config.StateChanged += StateChanged;
        SaveButton.Click += SaveButton_Click;
        Icon = new System.Drawing.Icon("Computer.ico");
    }

    public IApplication Application { get; }
    public Config Config => Application.Config;


    private void StateChanged()
    {
        if (InvokeRequired)
        {
            Invoke(new Action(() => StateChanged()));
            return;
        }

        if (Application.IsBusy)
        {
            Fps.Enabled = false;
            Quality.Enabled = false;
            Preset.Enabled = false;
            UseGpu.Enabled = false;
            Icon = new System.Drawing.Icon("ComputerRecording.ico");
        }
        else
        {
            Fps.Enabled = true;
            Quality.Enabled = true;
            Preset.Enabled = true;
            UseGpu.Enabled = true;
            Icon = new System.Drawing.Icon("Computer.ico");
        }
    }


    private void ConfigForm_VisibleChanged(object? sender, EventArgs e)
    {
        if (Visible)
        {
            StateChanged();
            MaximumPixelDifferenceValue.Text = Config.MaximumPixelDifferenceValue.ToString();
            MaximumDifferentPixelCount.Text = Config.MaximumDifferentPixelCount.ToString();
            MinPlaybackSpeed.Text = Config.MinPlaybackSpeed.ToString();
            Fps.SelectedItem = Config.OutputFps.ToString();
            Quality.SelectedItem = Config.OutputQuality.ToString();
            Preset.SelectedItem = Config.OutputPreset.ToString();
            UseGpu.Checked = Config.UseGpu;
        }
    }

    private void SaveButton_Click(object? sender, EventArgs e)
    {
        Config.MaximumPixelDifferenceValue = Convert.ToInt32(MaximumPixelDifferenceValue.Text);
        Config.MaximumDifferentPixelCount = Convert.ToInt32(MaximumDifferentPixelCount.Text);
        Config.MinPlaybackSpeed = Convert.ToInt32(MinPlaybackSpeed.Text);
        Config.OutputFps = Convert.ToInt32(Fps.Text);
        Config.OutputQuality = Quality.Text;
        Config.OutputPreset = Preset.Text;
        Config.UseGpu = UseGpu.Checked;
        Config.Save();
        Hide();
    }

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
