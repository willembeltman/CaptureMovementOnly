using System;
using System.Windows.Forms;

namespace CaptureOnlyMovements.Forms;

public class HiddenForm : Form
{
    private readonly System.Windows.Forms.NotifyIcon NotificationIcon;
    private readonly System.Windows.Forms.ContextMenuStrip NewContextMenuStrip;
    private readonly ToolStripMenuItem StartRecordingButton;
    private readonly ToolStripMenuItem StopRecordingButton;
    private readonly ToolStripMenuItem OpenSettingsButton;
    private readonly ToolStripMenuItem OpenDebugButton;
    private readonly ToolStripMenuItem ExitMenuItem;
    private readonly Recorder Recorder;
    private readonly SettingsForm SettingsForm;
    private readonly DebugForm DebugForm;
    private readonly FFMpegDebugForm FFMpegDebugForm;

    public ToolStripMenuItem OpenFFMpegDebugButton { get; }

    public HiddenForm()
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

        OpenSettingsButton = new ToolStripMenuItem("Open settings window");
        OpenSettingsButton.Click += OpenSettings_Click;
        NewContextMenuStrip.Items.Add(OpenSettingsButton);

        OpenDebugButton = new ToolStripMenuItem("Open debug window");
        OpenDebugButton.Click += OpenDebug_Click;
        NewContextMenuStrip.Items.Add(OpenDebugButton);

        OpenFFMpegDebugButton = new ToolStripMenuItem("Open ffmpeg debug window");
        OpenFFMpegDebugButton.Click += OpenFFMpegDebug_Click;
        NewContextMenuStrip.Items.Add(OpenFFMpegDebugButton);

        ExitMenuItem = new ToolStripMenuItem("Close");
        ExitMenuItem.Click += ExitMenuItem_Click;
        NewContextMenuStrip.Items.Add(ExitMenuItem);

        // Koppel het contextmenu aan de NotifyIcon
        NotificationIcon.ContextMenuStrip = NewContextMenuStrip;

        // Verberg het hoofdformulier bij het opstarten
        this.Load += Form1_Load;
        this.ShowInTaskbar = false; // Verberg de applicatie van de taakbalk
        this.WindowState = FormWindowState.Minimized; // Minimaliseer het venster
        this.Hide(); // Verberg het venster

        Recorder = new Recorder(); // Initialiseer de Recorder klasse
        Recorder.StateUpdated += RecorderState_Updated;

        SettingsForm = new SettingsForm(Recorder);
        DebugForm = new DebugForm(Recorder);
        FFMpegDebugForm = new FFMpegDebugForm(Recorder);
    }

    private void RecorderState_Updated(bool recording)
    {
        if (InvokeRequired)
        {
            Invoke(new Action(() => RecorderState_Updated(recording)));
            return;
        }

        StartRecordingButton.Visible = !recording; // Verberg de startknop
        StopRecordingButton.Visible = recording; // Toon de stopknop

        if (recording)
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
        this.Hide(); // Zorg ervoor dat het formulier verborgen is bij het laden
    }

    private void StartRecording_Click(object? sender, EventArgs e)
    {
        Recorder.Start();
    }
    private void StopRecording_Click(object? sender, EventArgs e)
    {
        Recorder.Stop();
    }
    private void OpenSettings_Click(object? sender, EventArgs e)
    {
        SettingsForm.Show();
    }
    private void OpenDebug_Click(object? sender, EventArgs e)
    {
        DebugForm.Show();
    }
    private void OpenFFMpegDebug_Click(object? sender, EventArgs e)
    {
        FFMpegDebugForm.Show();
    }
    private void ExitMenuItem_Click(object? sender, EventArgs e)
    {
        Recorder.Dispose();
        DebugForm.Dispose();
        SettingsForm.Dispose();

        // Opruimen van de NotifyIcon voordat de applicatie wordt afgesloten
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
            SettingsForm.Dispose();
            DebugForm.Dispose();
            NotificationIcon.Dispose();
        }
        base.Dispose(disposing);
    }
}
