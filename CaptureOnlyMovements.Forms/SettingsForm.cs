using System;
using System.Windows.Forms;

namespace CaptureOnlyMovements.Forms;

public partial class SettingsForm : Form
{
    public SettingsForm(Recorder recorder)
    {
        Recorder = recorder;
        InitializeComponent();

        recorder.StateUpdated += Recorder_StateUpdated;
    }

    public Recorder Recorder { get; }

    private void Recorder_StateUpdated(bool recording)
    {
        if (InvokeRequired)
        {
            Invoke(new Action(() => Recorder_StateUpdated(recording)));
            return;
        }

        if (recording)
        {
            Fps.Enabled = false;
            Quality.Enabled = false;
            Preset.Enabled = false;
            UseGpu.Enabled = false;
        }
        else
        {
            Fps.Enabled = true;
            Quality.Enabled = true;
            Preset.Enabled = true;
            UseGpu.Enabled = true;
        }
    }

    private void Settings_Load(object sender, EventArgs e)
    {
        Recorder_StateUpdated(Recorder.Running);
        var config = Recorder.Config;
        MaximumPixelDifferenceValue.Text = config.MaximumPixelDifferenceValue.ToString();
        MaximumDifferentPixelCount.Text = config.MaximumDifferentPixelCount.ToString();
        MinPlaybackSpeed.Text = config.MinPlaybackSpeed.ToString();
        MaxLinesInDebug.Text = config.MaxLinesInDebug.ToString();
        Fps.SelectedValue = config.OutputFps.ToString();
        Quality.SelectedValue = config.OutputQuality.ToString();
        Preset.SelectedValue = config.OutputPreset.ToString();
        UseGpu.Checked = config.UseGpu;
    }

    private void SaveButton_Click(object sender, EventArgs e)
    {
        var config = Recorder.Config;
        config.MaximumPixelDifferenceValue = Convert.ToInt32(MaximumPixelDifferenceValue.Text);
        config.MaximumDifferentPixelCount = Convert.ToInt32(MaximumDifferentPixelCount.Text);
        config.MinPlaybackSpeed = Convert.ToInt32(MinPlaybackSpeed.Text);
        config.MaxLinesInDebug = Convert.ToInt32(MaxLinesInDebug.Text);
        config.OutputFps = Convert.ToInt32(Fps.Text);
        config.OutputQuality = Quality.Text;
        config.OutputPreset = Preset.Text;
        config.UseGpu = UseGpu.Checked;
        config.Save();
        Hide();
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
            Recorder.StateUpdated -= Recorder_StateUpdated;
            components.Dispose();
        }
        base.Dispose(disposing);
    }
}
