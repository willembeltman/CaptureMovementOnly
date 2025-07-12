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
            DebugList = new System.Windows.Forms.ListBox();
            SuspendLayout();
            // 
            // DebugList
            // 
            DebugList.BackColor = System.Drawing.Color.Black;
            DebugList.Dock = System.Windows.Forms.DockStyle.Fill;
            DebugList.ForeColor = System.Drawing.Color.White;
            DebugList.FormattingEnabled = true;
            DebugList.Location = new System.Drawing.Point(0, 0);
            DebugList.Name = "DebugList";
            DebugList.Size = new System.Drawing.Size(609, 459);
            DebugList.TabIndex = 0;
            // 
            // DebugForm
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(609, 459);
            Controls.Add(DebugList);
            Name = "DebugForm";
            Text = "OpenDebugForm";
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.ListBox DebugList;
    }
}