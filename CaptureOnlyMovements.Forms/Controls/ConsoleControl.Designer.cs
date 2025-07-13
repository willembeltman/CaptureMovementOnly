namespace CaptureOnlyMovements.Forms.Controls
{
    partial class ConsoleControl
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            List = new System.Windows.Forms.ListBox();
            SuspendLayout();
            // 
            // List
            // 
            List.BackColor = System.Drawing.Color.Black;
            List.BorderStyle = System.Windows.Forms.BorderStyle.None;
            List.Dock = System.Windows.Forms.DockStyle.Fill;
            List.ForeColor = System.Drawing.Color.White;
            List.FormattingEnabled = true;
            List.Location = new System.Drawing.Point(0, 0);
            List.Name = "List";
            List.Size = new System.Drawing.Size(418, 262);
            List.TabIndex = 1;
            // 
            // ConsoleControl
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            BackColor = System.Drawing.Color.Black;
            Controls.Add(List);
            ForeColor = System.Drawing.Color.White;
            Name = "ConsoleControl";
            Size = new System.Drawing.Size(418, 262);
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.ListBox List;
    }
}
