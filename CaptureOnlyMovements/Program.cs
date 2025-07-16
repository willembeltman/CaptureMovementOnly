using CaptureOnlyMovements.Forms;
using System;
using System.Windows.Forms;

namespace CaptureOnlyMovements;

internal static class Program
{
    [STAThread]
    static void Main()
    {
        ApplicationConfiguration.Initialize();
        Application.Run(new ApplicationForm());
    }
}