﻿using CaptureOnlyMovements.Interfaces;
using System.Windows.Forms;

namespace CaptureOnlyMovements.Forms;

public partial class FFMpegDebugForm : Form, IConsole
{
    public FFMpegDebugForm()
    {
        InitializeComponent();
    }

    public void WriteLine(string line)
    {
        Console.WriteLine(line);
    }

    protected override void OnFormClosing(FormClosingEventArgs e)
    {
        // Voorkom dat het formulier direct sluit als de gebruiker op X klikt
        e.Cancel = true;
        this.Hide();
        base.OnFormClosing(e);
    }
}
