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
            Console.Dock = System.Windows.Forms.DockStyle.Fill;
            Console.Location = new System.Drawing.Point(0, 0);
            Console.Name = "Console";
            Console.Size = new System.Drawing.Size(609, 459);
            Console.TabIndex = 0;
            // 
            // DebugForm
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(609, 459);
            Controls.Add(Console);
            Name = "DebugForm";
            Text = "OpenDebugForm";
            ResumeLayout(false);
        }

        #endregion

        private Controls.ConsoleControl Console;
    }
}