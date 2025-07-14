using System;
using System.Windows.Forms;

namespace CaptureOnlyMovements.Forms;

internal static class Program
{
    [STAThread]
    static void Main()
    {
        ApplicationConfiguration.Initialize();
        Application.Run(new ApplicationForm());
    }
}