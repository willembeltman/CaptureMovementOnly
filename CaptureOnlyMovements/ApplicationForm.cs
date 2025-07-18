using CaptureOnlyMovements.Forms;
using CaptureOnlyMovements.Forms.SubForms;
using CaptureOnlyMovements.Interfaces;
using CaptureOnlyMovements.Types;
using System;
using System.Windows.Forms;

namespace CaptureOnlyMovements;

public class ApplicationForm : Form, IApplication
{
    public ApplicationForm()
    {
        Config = Config.Load();
        Config.StateChanged += StateChanged;
        InputFps = new FpsCounter();
        OutputFps = new FpsCounter();

        Timer = new Timer
        {
            Interval = 100 // 1 seconde
        };
        Timer.Tick += Timer_Tick;
        NotificationIcon = new()
        {
            // Zorg ervoor dat je een geldig icoonpad hebt!
            Icon = new System.Drawing.Icon("Computer.ico"), // Vervang dit pad
            Visible = true,
            Text = "Capture Motion Only"
        };

        NewContextMenuStrip = new();

        StartRecordingButton = new ToolStripMenuItem("Start recording");
        StartRecordingButton.Click += StartRecording_Click;
        NewContextMenuStrip.Items.Add(StartRecordingButton);

        StopRecordingButton = new ToolStripMenuItem("Stop recording") { Visible = false };
        StopRecordingButton.Click += StopRecording_Click;
        NewContextMenuStrip.Items.Add(StopRecordingButton);


        NewContextMenuStrip.Items.Add(new ToolStripSeparator());


        var OpenConverterButton = new ToolStripMenuItem("Convert video(s)");
        OpenConverterButton.Click += OpenConverter_Click;
        NewContextMenuStrip.Items.Add(OpenConverterButton);


        NewContextMenuStrip.Items.Add(new ToolStripSeparator());


        var OpenSettingsButton = new ToolStripMenuItem("Open settings window");
        OpenSettingsButton.Click += OpenSettings_Click;
        NewContextMenuStrip.Items.Add(OpenSettingsButton);


        NewContextMenuStrip.Items.Add(new ToolStripSeparator());


        var OpenDebugButton = new ToolStripMenuItem("Open main debug window");
        OpenDebugButton.Click += OpenDebug_Click;
        NewContextMenuStrip.Items.Add(OpenDebugButton);

        var OpenFFMpegDebugButton = new ToolStripMenuItem("Open ffmpeg debug window");
        OpenFFMpegDebugButton.Click += OpenFFMpegDebug_Click;
        NewContextMenuStrip.Items.Add(OpenFFMpegDebugButton);

        var OpenMaskButton = new ToolStripMenuItem("Open mask window");
        OpenMaskButton.Click += OpenMaskButton_Click;
        NewContextMenuStrip.Items.Add(OpenMaskButton);


        NewContextMenuStrip.Items.Add(new ToolStripSeparator());


        var ExitMenuItem = new ToolStripMenuItem("Close");
        ExitMenuItem.Click += ExitMenuItem_Click;
        NewContextMenuStrip.Items.Add(ExitMenuItem);


        NotificationIcon.ContextMenuStrip = NewContextMenuStrip;

        Load += ApplicationForm_Load;
        ShowInTaskbar = false; // Verberg de applicatie van de taakbalk
        WindowState = FormWindowState.Minimized; // Minimaliseer het venster
        Hide(); // Verberg het venster

        ConverterForm = new ConverterForm(this);
        ConfigForm = new ConfigForm(this);
        DebugForm = new DebugForm();
        FFMpegDebugForm = new FFMpegDebugForm();
        MaskForm = new MaskForm(this);
        Recorder = new Recorder(this, DebugForm, FFMpegDebugForm, MaskForm);
    }

    private readonly Timer Timer;
    private readonly NotifyIcon NotificationIcon;
    private readonly ContextMenuStrip NewContextMenuStrip;
    private readonly ToolStripMenuItem StartRecordingButton;
    private readonly ToolStripMenuItem StopRecordingButton;

    private readonly Recorder Recorder;
    private readonly DebugForm DebugForm;
    private readonly ConfigForm ConfigForm;
    private readonly ConverterForm ConverterForm;
    private readonly FFMpegDebugForm FFMpegDebugForm;
    private readonly MaskForm MaskForm;

    public Config Config { get; }
    public FpsCounter InputFps { get; }
    public FpsCounter OutputFps { get; }

    public bool IsBusy => Recorder.Recording || ConverterForm.IsBusy;

    private void StateChanged()
    {
        if (InvokeRequired)
        {
            Invoke(new Action(() => StateChanged()));
            return;
        }

        StartRecordingButton.Visible = !Recorder.Recording; // Verberg de startknop
        StopRecordingButton.Visible = Recorder.Recording; // Toon de stopknop

        if (Recorder.Recording)
        {
            NotificationIcon.Icon = new System.Drawing.Icon("ComputerRecording.ico");
        }
        else
        {
            NotificationIcon.Icon = new System.Drawing.Icon("Computer.ico");
        }
    }
    private void Timer_Tick(object? sender, EventArgs e)
    {
        if (Recorder.Recording)
        {
            Timer.Interval = 100;
            NotificationIcon.Text = $"Capture Motion Only (Recording - Reading: {InputFps.CalculateFps().ToString("F2")} fps / Writing: {OutputFps.CalculateFps().ToString("F2")} fps)";
        }
        else
        {
            Timer.Interval = 500;
            NotificationIcon.Text = "Capture Motion Only (Not Recording)";
        }
    }
    private void ApplicationForm_Load(object? sender, EventArgs e)
    {
        Hide(); // Zorgt ervoor dat het formulier verborgen is bij het laden
        Timer.Start(); // Start de timer om de notificatie-tekst bij te werken
    }
    private void StartRecording_Click(object? sender, EventArgs e) => Recorder.Start();
    private void StopRecording_Click(object? sender, EventArgs e) => Recorder.Stop();
    private void OpenConverter_Click(object? sender, EventArgs e) => ConverterForm.Show();
    private void OpenSettings_Click(object? sender, EventArgs e) => ConfigForm.Show();
    private void OpenDebug_Click(object? sender, EventArgs e) => DebugForm.Show();
    private void OpenFFMpegDebug_Click(object? sender, EventArgs e) => FFMpegDebugForm.Show();

    private void OpenMaskButton_Click(object? sender, EventArgs e) => MaskForm.Show();
    private void ExitMenuItem_Click(object? sender, EventArgs e) => Exit();

    public void FatalException(Exception exception) => FatalException(exception.Message, "Fatal exception");
    public void FatalException(string message, string title)
    {
        MessageBox.Show(message, title, MessageBoxButtons.OK, MessageBoxIcon.Error);
        Exit();
    }

    private void Exit()
    {
        Recorder.Dispose();
        MaskForm.Dispose();
        DebugForm.Dispose();
        ConfigForm.Dispose();
        ConverterForm.Dispose();
        FFMpegDebugForm.Dispose();

        if (NotificationIcon != null)
        {
            NotificationIcon.Visible = false;
            NotificationIcon.Dispose();
        }
        Application.Exit();
    }
    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            Recorder.Dispose();
            MaskForm.Dispose();
            DebugForm.Dispose();
            ConfigForm.Dispose();
            ConverterForm.Dispose();
            FFMpegDebugForm.Dispose();
            NotificationIcon.Dispose();
        }
        base.Dispose(disposing);
    }
}