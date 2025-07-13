using System.Collections.Concurrent;
using System.Windows.Forms;

namespace CaptureOnlyMovements.Forms.Controls
{
    public partial class ConsoleControl : UserControl
    {
        ConcurrentQueue<string> Queue = new ConcurrentQueue<string>();

        public ConsoleControl()
        {
            InitializeComponent();
        }

        internal void WriteLine(string line)
        {
            Queue.Enqueue(line);
        }

        private void Timer_Tick(object sender, System.EventArgs e)
        {
            while (Queue.TryDequeue(out var line))
            {
                List.Items.Add(line);
            }

            while (List.Items.Count > 20)
            {
                List.Items.RemoveAt(0);
            }
        }
    }
}
