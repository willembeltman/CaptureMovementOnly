namespace CaptureOnlyMovements.Forms
{
    partial class DebugForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            Console = new CaptureOnlyMovements.Forms.Controls.ConsoleControl();
            SuspendLayout();
            // 
            // Console
            // 
            Console.BackColor = System.Drawing.Color.Black;
            Console.Dock = System.Windows.Forms.DockStyle.Fill;
            Console.ForeColor = System.Drawing.Color.White;
            Console.Location = new System.Drawing.Point(0, 0);
            Console.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            Console.Name = "Console";
            Console.Size = new System.Drawing.Size(696, 348);
            Console.TabIndex = 0;
            // 
            // DebugForm
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(696, 348);
            Controls.Add(Console);
            Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            Name = "DebugForm";
            Text = "OpenDebugForm";
            ResumeLayout(false);
        }

        #endregion

        private Controls.ConsoleControl Console;
    }
}