using CaptureOnlyMovements.Forms;
using System;
using System.Text;
using System.Windows.Forms;


public class HiddenForm : Form
{
    private System.Windows.Forms.NotifyIcon NotificationIcon;
    private System.Windows.Forms.ContextMenuStrip ContextMenuStrip;
    private ToolStripMenuItem OpenSettingsButton;
    private ToolStripMenuItem StartRecordingButton;
    private ToolStripMenuItem StopRecordingButton;
    private ToolStripMenuItem exitMenuItem;
    private Recorder Recorder;
    private ToolStripMenuItem OpenDebugButton;

    public HiddenForm()
    {
        NotificationIcon = new System.Windows.Forms.NotifyIcon();

        // Zorg ervoor dat je een geldig icoonpad hebt!
        NotificationIcon.Icon = new System.Drawing.Icon("Computer.ico"); // Vervang dit pad
        NotificationIcon.Visible = true;
        NotificationIcon.Text = "Mijn Systeemvak Applicatie";

        // Initialiseer ContextMenuStrip
        ContextMenuStrip = new System.Windows.Forms.ContextMenuStrip();

        // Voeg menu-items toe aan het contextmenu

        StartRecordingButton = new ToolStripMenuItem("Start recording");
        StartRecordingButton.Click += StartRecording_Click;
        ContextMenuStrip.Items.Add(StartRecordingButton);

        StopRecordingButton = new ToolStripMenuItem("Stop recording");
        StopRecordingButton.Visible = false;
        StopRecordingButton.Click += StopRecording_Click;
        ContextMenuStrip.Items.Add(StopRecordingButton);

        OpenSettingsButton = new ToolStripMenuItem("Open settings window");
        OpenSettingsButton.Click += OpenSettings_Click;
        ContextMenuStrip.Items.Add(OpenSettingsButton);

        OpenDebugButton = new ToolStripMenuItem("Open debug window");
        OpenDebugButton.Click += OpenDebug_Click;
        ContextMenuStrip.Items.Add(OpenDebugButton);

        exitMenuItem = new ToolStripMenuItem("Close");
        exitMenuItem.Click += ExitMenuItem_Click;
        ContextMenuStrip.Items.Add(exitMenuItem);

        // Koppel het contextmenu aan de NotifyIcon
        NotificationIcon.ContextMenuStrip = ContextMenuStrip;

        // Verberg het hoofdformulier bij het opstarten
        this.Load += Form1_Load;
        this.ShowInTaskbar = false; // Verberg de applicatie van de taakbalk
        this.WindowState = FormWindowState.Minimized; // Minimaliseer het venster
        this.Hide(); // Verberg het venster

        Recorder = new Recorder(); // Initialiseer de Recorder klasse
        Recorder.StateUpdated += RecorderStateUpdated;
    }


    private void RecorderStateUpdated(bool recording)
    {
        if (InvokeRequired)
        {
            Invoke(new Action(() => RecorderStateUpdated(recording)));
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
        using var settingsForm = new SettingsForm(Recorder);
        settingsForm.ShowDialog();
    }

    private void OpenDebug_Click(object? sender, EventArgs e)
    {
        using var debugForm = new DebugForm(Recorder);
        debugForm.ShowDialog();
    }

    private void ExitMenuItem_Click(object? sender, EventArgs e)
    {
        // Opruimen van de NotifyIcon voordat de applicatie wordt afgesloten
        if (NotificationIcon != null)
        {
            NotificationIcon.Visible = false;
            NotificationIcon.Dispose();
        }
        Application.Exit();
    }
}
