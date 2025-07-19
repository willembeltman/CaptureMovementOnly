using CaptureOnlyMovements.Interfaces;
using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace CaptureOnlyMovements.Forms.Controls
{
    public partial class ConsoleControl : UserControl, IConsole
    {
        private readonly ConcurrentQueue<string> Queue = new();
        private readonly AutoResetEvent Signal = new(false);
        private readonly Thread Thread;

        private int MaxVisibleLines => List.ClientSize.Height / List.ItemHeight;

        public ConsoleControl()
        {
            InitializeComponent();
            List.KeyDown += List_KeyDown;

            // Start de achtergrondthread
            Thread = new Thread(ProcessQueue)
            {
                IsBackground = true
            };
            Thread.Start();
        }

        public void WriteLine(string line)
        {
            Queue.Enqueue(line);
            Signal.Set(); // Wek de thread op
        }

        private void ProcessQueue()
        {
            while (!IsDisposed)
            {
                Signal.WaitOne(); // Blokkeert tot er een item komt

                while (Queue.TryDequeue(out var line))
                {
                    if (IsDisposed) return;

                    // UI moet via Invoke
                    if (List.InvokeRequired)
                    {
                        List.Invoke(() => AddLineToList(line));
                    }
                    else
                    {
                        AddLineToList(line);
                    }
                }
            }
        }

        private void AddLineToList(string line)
        {
            if (List.Items.Count > MaxVisibleLines - 1)
            {
                List.Items.RemoveAt(0);
            }
            List.Items.Add(line);
        }

        private void List_KeyDown(object? sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.C)
            {
                var lines = List.SelectedItems
                                .Cast<object>()
                                .Select(o => o?.ToString() ?? string.Empty);

                Clipboard.SetText(string.Join(Environment.NewLine, lines));
                e.Handled = true;
            }
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            if (disposing)
            {
                Signal.Set(); // zorg dat de thread eruit komt
                Signal.Dispose();
            }
        }
    }
}
