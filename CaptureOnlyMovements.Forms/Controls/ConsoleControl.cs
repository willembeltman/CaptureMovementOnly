using System.Windows.Forms;

namespace CaptureOnlyMovements.Forms.Controls
{
    public partial class ConsoleControl : UserControl
    {
        public ConsoleControl()
        {
            InitializeComponent();
        }

        internal void WriteLine(string line)
        {
            return;
            if (InvokeRequired)
            {
                Invoke(() => { WriteLine(line); });
                return;
            }
            List.Items.Add(line);
        }
    }
}
