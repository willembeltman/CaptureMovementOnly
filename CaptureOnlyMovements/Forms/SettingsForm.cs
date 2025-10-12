using CaptureOnlyMovements.Enums;
using CaptureOnlyMovements.Interfaces;
using CaptureOnlyMovements.Types;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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

    public List<ConfigPrefixPreset> PrefixList { get; private set; } = [];

    private void ConfigForm_Load(object? sender, EventArgs e)
    {
        QualityCombo.DataSource = Enum.GetNames<QualityEnum>();
        PresetCombo.DataSource = Enum.GetNames<PresetEnum>();
        EncoderCombo.DataSource = Application.WorkingEncoders.List.Select(a => a.ToString()).ToArray();
    }

    private void StateChanged()
    {
        if (InvokeRequired)
        {
            Invoke(new Action(() => StateChanged()));
            return;
        }

        FpsCombo.Enabled = !Application.IsBusy;
        QualityCombo.Enabled = !Application.IsBusy;
        PresetCombo.Enabled = !Application.IsBusy;
        EncoderCombo.Enabled = !Application.IsBusy;
        PrefixPresetList.Enabled = !Application.IsBusy;

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

            PrefixList = Config.PrefixPresets.Select(a => new ConfigPrefixPreset(a)).ToList();
            PrefixPresetList.DataSource = new BindingList<ConfigPrefixPreset>(PrefixList);
            PrefixPresetList.Columns[0]!.Width = 200;
            PrefixPresetList.Columns[1]!.Width = 600;
            PrefixPresetList.Columns[2]!.Width = 120;
            PrefixPresetList.Columns[3]!.Visible = false;
            MaximumPixelDifferenceValue.Text = Config.MaximumPixelDifferenceValue.ToString();
            MaximumDifferentPixelCount.Text = Config.MaximumDifferentPixelCount.ToString();
            MinPlaybackSpeed.Text = Config.MinPlaybackSpeed.ToString();
            FpsCombo.SelectedItem = Config.OutputFps.ToString();
            QualityCombo.SelectedItem = Config.OutputQuality.ToString();
            PresetCombo.SelectedItem = Config.OutputPreset.ToString();
            EncoderCombo.SelectedItem = Config.OutputEncoder.ToString();
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
        if (!int.TryParse(FpsCombo.Text, out var fps))
        {
            MessageBox.Show("You have to specify a number");
            FpsCombo.Focus();
            return;
        }

        Config.PrefixPresets = PrefixList.ToArray();
        Config.MaximumPixelDifferenceValue = maxPixelDiff;
        Config.MaximumDifferentPixelCount = maxDifferentPixel;
        Config.MinPlaybackSpeed = minPlaybackSpeed;
        Config.OutputFps = fps;
        Config.OutputQuality = Enum.Parse<QualityEnum>(QualityCombo.Text);
        Config.OutputPreset = Enum.Parse<PresetEnum>(PresetCombo.Text);
        Config.OutputEncoder = Enum.Parse<EncoderEnum>(EncoderCombo.Text);
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
