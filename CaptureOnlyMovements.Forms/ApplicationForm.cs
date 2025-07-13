using CaptureOnlyMovements.Delegates;
using CaptureOnlyMovements.Forms.SubForms;
using CaptureOnlyMovements.Helpers;
using CaptureOnlyMovements.Interfaces;
using CaptureOnlyMovements.Types;
using System;
using System.Windows.Forms;

namespace CaptureOnlyMovements.Forms;

public class ApplicationForm : Form, IApplication
{
    private readonly NotifyIcon NotificationIcon;
    private readonly ContextMenuStrip NewContextMenuStrip;
    private readonly ToolStripMenuItem StartRecordingButton;
    private readonly ToolStripMenuItem StopRecordingButton;

    private readonly Recorder Recorder;
    private readonly DebugForm DebugForm;
    private readonly ConfigForm ConfigForm;
    private readonly ConverterForm ConverterForm;
    private readonly FFMpegDebugForm FFMpegDebugForm;

    public Config Config { get; }
    public FpsCounter InputFps { get; } = new FpsCounter();
    public FpsCounter OutputFps { get; } = new FpsCounter();

    public bool IsBusy => Recorder.Recording || ConverterForm.IsBusy;

    public bool ShowDifference => false;

    public event DebugMessage? DebugUpdated;
    public event DebugMessage? FFMpegDebugUpdated;

    public ApplicationForm()
    {
        NotificationIcon = new()
        {
            // Zorg ervoor dat je een geldig icoonpad hebt!
            Icon = new System.Drawing.Icon("Computer.ico"), // Vervang dit pad
            Visible = true,
            Text = "Mijn Systeemvak Applicatie"
        };

        // Initialiseer ContextMenuStrip
        NewContextMenuStrip = new();

        // Voeg menu-items toe aan het contextmenu

        StartRecordingButton = new ToolStripMenuItem("Start recording");
        StartRecordingButton.Click += StartRecording_Click;
        NewContextMenuStrip.Items.Add(StartRecordingButton);

        StopRecordingButton = new ToolStripMenuItem("Stop recording")
        {
            Visible = false
        };
        StopRecordingButton.Click += StopRecording_Click;
        NewContextMenuStrip.Items.Add(StopRecordingButton);

        var OpenConverterButton = new ToolStripMenuItem("Convert video(s)");
        OpenConverterButton.Click += OpenConverter_Click;
        NewContextMenuStrip.Items.Add(OpenConverterButton);

        var OpenSettingsButton = new ToolStripMenuItem("Open settings window");
        OpenSettingsButton.Click += OpenSettings_Click;
        NewContextMenuStrip.Items.Add(OpenSettingsButton);

        var OpenDebugButton = new ToolStripMenuItem("Open debug window");
        OpenDebugButton.Click += OpenDebug_Click;
        NewContextMenuStrip.Items.Add(OpenDebugButton);

        var OpenFFMpegDebugButton = new ToolStripMenuItem("Open ffmpeg debug window");
        OpenFFMpegDebugButton.Click += OpenFFMpegDebug_Click;
        NewContextMenuStrip.Items.Add(OpenFFMpegDebugButton);

        var ExitMenuItem = new ToolStripMenuItem("Close");
        ExitMenuItem.Click += ExitMenuItem_Click;
        NewContextMenuStrip.Items.Add(ExitMenuItem);

        // Koppel het contextmenu aan de NotifyIcon
        NotificationIcon.ContextMenuStrip = NewContextMenuStrip;

        // Verberg het hoofdformulier bij het opstarten
        Load += Form1_Load;
        ShowInTaskbar = false; // Verberg de applicatie van de taakbalk
        WindowState = FormWindowState.Minimized; // Minimaliseer het venster
        Hide(); // Verberg het venster

        Config = Config.Load();
        Config.StateChanged += ConfigChanged;

        ConverterForm = new ConverterForm(this);
        ConfigForm = new ConfigForm(this);
        DebugForm = new DebugForm(this);
        FFMpegDebugForm = new FFMpegDebugForm(this);
        Recorder = new Recorder(this, DebugForm, FFMpegDebugForm);
    }

    private void ConfigChanged()
    {
        if (InvokeRequired)
        {
            Invoke(new Action(() => ConfigChanged()));
            return;
        }

        StartRecordingButton.Visible = !Recorder.Recording; // Verberg de startknop
        StopRecordingButton.Visible = Recorder.Recording; // Toon de stopknop

        if (Recorder.Recording)
        {
            NotificationIcon.Icon = new System.Drawing.Icon("ComputerRecording.ico"); // Vervang dit pad
        }
        else
        {
            NotificationIcon.Icon = new System.Drawing.Icon("Computer.ico"); // Vervang dit pad

        }
    }
    private void Form1_Load(object? sender, EventArgs e)
    {
        Hide(); // Zorg ervoor dat het formulier verborgen is bij het laden
    }

    private void StartRecording_Click(object? sender, EventArgs e) => Recorder.Start();
    private void StopRecording_Click(object? sender, EventArgs e) => Recorder.Stop();
    private void OpenConverter_Click(object? sender, EventArgs e) => ConverterForm.Show();
    private void OpenSettings_Click(object? sender, EventArgs e) => ConfigForm.Show();
    private void OpenDebug_Click(object? sender, EventArgs e) => DebugForm.Show();
    private void OpenFFMpegDebug_Click(object? sender, EventArgs e) => FFMpegDebugForm.Show();
    private void ExitMenuItem_Click(object? sender, EventArgs e) => Exit();

    public void DebugWriteLine(string line) => DebugUpdated?.Invoke(line);
    public void FFMpegDebugWriteLine(string line) => FFMpegDebugUpdated?.Invoke(line);

    private void Exit()
    {
        Recorder.Dispose();
        DebugForm.Dispose();
        ConfigForm.Dispose();
        ConverterForm.Dispose();
        FFMpegDebugForm.Dispose();

        // Opruimen van de NotifyIcon voordat de applicatie wordt afgesloten
        if (NotificationIcon != null)
        {
            NotificationIcon.Visible = false;
            NotificationIcon.Dispose();
        }
        Application.Exit();
    }

    public void FatalException(Exception exception)
    {
        FatalException(exception.Message, "Fatal exception");
    }
    public void FatalException(string message, string title)
    {
        MessageBox.Show(message, title, MessageBoxButtons.OK, MessageBoxIcon.Error);
        Exit();
    }
    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            Recorder.Dispose();
            DebugForm.Dispose();
            ConfigForm.Dispose();
            ConverterForm.Dispose();
            FFMpegDebugForm.Dispose();
            NotificationIcon.Dispose();
        }
        base.Dispose(disposing);
    }

    public void SetMask(bool[] frameData, Resolution frameResolution)
    {
    }

    public void SetPreview(Frame frame)
    {
    }
}