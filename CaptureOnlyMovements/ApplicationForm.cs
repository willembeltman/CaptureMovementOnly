﻿using CaptureOnlyMovements.Forms;
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

        InputFps = new FpsCounter();
        OutputFps = new FpsCounter();

        DebugForm = new DebugForm();
        FFMpegDebugForm = new FFMpegDebugForm();

        MaskForm = new MaskForm(this);
        ConfigForm = new ConfigForm(this);
        ConverterForm = new ConverterForm(this);
        DetectEncodersForm = new DetectEncodersForm(this);
        Recorder = new Recorder(this, DebugForm, FFMpegDebugForm, MaskForm);

        Timer = new Timer
        {
            Interval = 100
        };

        NotificationIcon = new NotifyIcon()
        {
            Icon = new System.Drawing.Icon("Computer.ico"),
            Visible = true,
            Text = "Capture Motion Only"
        };

        NewContextMenuStrip = new ContextMenuStrip();

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
        OpenMaskButton.Click += OpenMask_Click;
        NewContextMenuStrip.Items.Add(OpenMaskButton);

        NewContextMenuStrip.Items.Add(new ToolStripSeparator());

        var ExitMenuButton = new ToolStripMenuItem("Close");
        ExitMenuButton.Click += ExitMenuItem_Click;
        NewContextMenuStrip.Items.Add(ExitMenuButton);

        NotificationIcon.ContextMenuStrip = NewContextMenuStrip;


        Load += ApplicationForm_Load;
        Config.StateChanged += StateChanged;
        Timer.Tick += Timer_Tick;
        ShowInTaskbar = false;
        WindowState = FormWindowState.Minimized;
        Hide();
    }

    private readonly Timer Timer;
    private readonly Recorder Recorder;
    private readonly MaskForm MaskForm;
    private readonly DebugForm DebugForm;
    private readonly ConfigForm ConfigForm;
    private readonly NotifyIcon NotificationIcon;
    private readonly ConverterForm ConverterForm;
    private readonly FFMpegDebugForm FFMpegDebugForm;
    private readonly DetectEncodersForm DetectEncodersForm;

    private readonly ContextMenuStrip NewContextMenuStrip;
    private readonly ToolStripMenuItem StartRecordingButton;
    private readonly ToolStripMenuItem StopRecordingButton;

    public Config Config { get; }
    public FpsCounter InputFps { get; }
    public FpsCounter OutputFps { get; }
    public IWorkingEncoders WorkingEncoders => DetectEncodersForm;
    public bool IsBusy => Recorder.Recording || ConverterForm.IsBusy;

    private void StateChanged()
    {
        if (InvokeRequired)
        {
            Invoke(new Action(() => StateChanged()));
            return;
        }

        StartRecordingButton.Visible = !Recorder.Recording;
        StopRecordingButton.Visible = Recorder.Recording; 

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
        Hide();
        DetectEncodersForm.Show();
        Timer.Start(); 
    }
    private void StartRecording_Click(object? sender, EventArgs e) => Recorder.Start();
    private void StopRecording_Click(object? sender, EventArgs e) => Recorder.Stop();
    private void OpenConverter_Click(object? sender, EventArgs e) => ConverterForm.Show();
    private void OpenSettings_Click(object? sender, EventArgs e) => ConfigForm.Show();
    private void OpenDebug_Click(object? sender, EventArgs e) => DebugForm.Show();
    private void OpenFFMpegDebug_Click(object? sender, EventArgs e) => FFMpegDebugForm.Show();
    private void OpenMask_Click(object? sender, EventArgs e) => MaskForm.Show();
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
        DetectEncodersForm.Dispose();

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
            DetectEncodersForm.Dispose();
        }
        base.Dispose(disposing);
    }
}