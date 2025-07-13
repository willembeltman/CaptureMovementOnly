using CaptureOnlyMovements.Interfaces;
using System.Collections.Concurrent;
using System.Windows.Forms;

namespace CaptureOnlyMovements.Forms.Controls;

public partial class ConsoleControl : UserControl, IConsole
{
    ConcurrentQueue<string> Queue = new ConcurrentQueue<string>();
    private int MaxVisibleLines => List.ClientSize.Height / List.ItemHeight;

    public ConsoleControl()
    {
        InitializeComponent();
    }

    public void WriteLine(string line)
    {
        Queue.Enqueue(line);
    }

    private void Timer_Tick(object sender, System.EventArgs e)
    {
        while (Queue.TryDequeue(out var line))
        {
            if (List.Items.Count > MaxVisibleLines - 1) // Determine how many lines to keep
            {
                List.Items.RemoveAt(0);
            }
            List.Items.Add(line);
        }

    }
}
