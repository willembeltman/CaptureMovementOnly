using CaptureOnlyMovements.Interfaces;
using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Windows.Forms;

namespace CaptureOnlyMovements.Forms.Controls;

public partial class ConsoleControl : UserControl, IConsole
{
    ConcurrentQueue<string> Queue = new ConcurrentQueue<string>();
    private int MaxVisibleLines => List.ClientSize.Height / List.ItemHeight;

    public ConsoleControl()
    {
        InitializeComponent();
        List.KeyDown += List_KeyDown;
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
    private void List_KeyDown(object? sender, KeyEventArgs e)
    {
        // CTRL + C?
        if (e.Control && e.KeyCode == Keys.C)
        {
            // verzamel alle geselecteerde items
            var lines = List.SelectedItems
                            .Cast<object>()
                            .Select(o => o?.ToString() ?? string.Empty);

            // zet ze als één string (met regeleinden) op het klembord
            Clipboard.SetText(string.Join(Environment.NewLine, lines));

            // geef aan dat je de toetscombinatie hebt afgehandeld
            e.Handled = true;
        }
    }
}
