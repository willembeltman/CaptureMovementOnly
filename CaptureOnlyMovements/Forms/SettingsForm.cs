using CaptureOnlyMovements.Enums;
using CaptureOnlyMovements.Interfaces;
using CaptureOnlyMovements.Types;
using System;
using System.Linq;
using System.Windows.Forms;

namespace CaptureOnlyMovements.Forms;

public partial class ConfigForm : Form
{
    public ConfigForm(IApplication application)
    {
        InitializeComponent();
        Application = application;
        Load += ConfigForm_Load;
        VisibleChanged += ConfigForm_VisibleChanged;
        Config.StateChanged += StateChanged;
        SaveButton.Click += SaveButton_Click;
        Icon = new System.Drawing.Icon("Computer.ico");
    }

    public IApplication Application { get; }
    public Config Config => Application.Config;

    private void ConfigForm_Load(object? sender, EventArgs e)
    {
        Quality.DataSource = Enum.GetNames<QualityEnum>();
        Preset.DataSource = Enum.GetNames<PresetEnum>();
        Encoder.DataSource = Application.WorkingEncoders.List.Select(a => a.ToString()).ToArray();
    }

    private void StateChanged()
    {
        if (InvokeRequired)
        {
            Invoke(new Action(() => StateChanged()));
            return;
        }

        Fps.Enabled = !Application.IsBusy;
        Quality.Enabled = !Application.IsBusy;
        Preset.Enabled = !Application.IsBusy;
        Encoder.Enabled = !Application.IsBusy;
        FilenamePrefix.Enabled = !Application.IsBusy;

        if (Application.IsBusy)
        {
            Icon = new System.Drawing.Icon("ComputerRecording.ico");
        }
        else
        {
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
            FilenamePrefix.Text = Config.OutputFileNamePrefix.ToString();
            Quality.SelectedItem = Config.OutputQuality.ToString();
            Preset.SelectedItem = Config.OutputPreset.ToString();
            Encoder.SelectedItem = Config.OutputEncoder.ToString();
        }
    }

    private void SaveButton_Click(object? sender, EventArgs e)
    {
        if (!int.TryParse(MaximumPixelDifferenceValue.Text, out var maxPixelDiff))
        {
            MessageBox.Show("You have to specify a number");
            MaximumPixelDifferenceValue.Focus();
            return;
        }
        if (!int.TryParse(MaximumDifferentPixelCount.Text, out var maxDifferentPixel))
        {
            MessageBox.Show("You have to specify a number");
            MaximumDifferentPixelCount.Focus();
            return;
        }
        if (!int.TryParse(MinPlaybackSpeed.Text, out var minPlaybackSpeed))
        {
            MessageBox.Show("You have to specify a number");
            MinPlaybackSpeed.Focus();
            return;
        }
        if (!int.TryParse(Fps.Text, out var fps))
        {
            MessageBox.Show("You have to specify a number");
            Fps.Focus();
            return;
        }

        Config.MaximumPixelDifferenceValue = maxPixelDiff;
        Config.MaximumDifferentPixelCount = maxDifferentPixel;
        Config.MinPlaybackSpeed = minPlaybackSpeed;
        Config.OutputFps = fps;
        Config.OutputFileNamePrefix = FilenamePrefix.Text;
        Config.OutputQuality = Enum.Parse<QualityEnum>(Quality.Text);
        Config.OutputPreset = Enum.Parse<PresetEnum>(Preset.Text);
        Config.OutputEncoder = Enum.Parse<EncoderEnum>(Encoder.Text);
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
