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
            OutputFPS.Enabled = false;
            OutputCRF.Enabled = false;
            Preset.Enabled = false;
            UseQuickSync.Enabled = false;
        }
        else
        {
            OutputFPS.Enabled = true;
            OutputCRF.Enabled = true;
            Preset.Enabled = true;
            UseQuickSync.Enabled = true;
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
        OutputFPS.SelectedValue = config.OutputFPS.ToString();
        OutputCRF.Text = config.OutputCRF.ToString();
        Preset.SelectedValue = config.Preset.ToString();
        UseQuickSync.Checked = config.UseQuickSync;
    }

    private void SaveButton_Click(object sender, EventArgs e)
    {
        var config = Recorder.Config;
        config.MaximumPixelDifferenceValue = Convert.ToInt32(MaximumPixelDifferenceValue.Text);
        config.MaximumDifferentPixelCount = Convert.ToInt32(MaximumDifferentPixelCount.Text);
        config.MinPlaybackSpeed = Convert.ToInt32(MinPlaybackSpeed.Text);
        config.MaxLinesInDebug = Convert.ToInt32(MaxLinesInDebug.Text);
        config.OutputFPS = Convert.ToInt32(OutputFPS.Text);
        config.OutputCRF = Convert.ToInt32(OutputCRF.Text);
        config.Preset = Preset.Text;
        config.UseQuickSync = UseQuickSync.Checked;
        config.Save();
        Close();
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
