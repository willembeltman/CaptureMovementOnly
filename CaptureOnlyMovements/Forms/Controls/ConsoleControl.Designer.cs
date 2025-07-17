using System.Windows.Forms;

namespace CaptureOnlyMovements.Forms.Controls
{
    partial class ConsoleControl
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
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
            List.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            List.Name = "List";
            List.Size = new System.Drawing.Size(478, 349);
            List.TabIndex = 1;
            List.SelectionMode = SelectionMode.MultiExtended;
            // 
            // ConsoleControl
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            BackColor = System.Drawing.Color.Black;
            Controls.Add(List);
            ForeColor = System.Drawing.Color.White;
            Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            Name = "ConsoleControl";
            Size = new System.Drawing.Size(478, 349);
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.ListBox List;
    }
}
