namespace CaptureOnlyMovements.Forms
{
    partial class MaskForm
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
            displayControl1 = new CaptureOnlyMovements.Forms.Controls.DisplayControl();
            SuspendLayout();
            // 
            // displayControl1
            // 
            displayControl1.BackColor = System.Drawing.Color.Black;
            displayControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            displayControl1.Location = new System.Drawing.Point(0, 0);
            displayControl1.Name = "displayControl1";
            displayControl1.Size = new System.Drawing.Size(800, 450);
            displayControl1.TabIndex = 0;
            // 
            // MaskForm
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(800, 450);
            Controls.Add(displayControl1);
            Name = "MaskForm";
            Text = "MaskForm";
            ResumeLayout(false);
        }

        #endregion

        private Controls.DisplayControl displayControl1;
    }
}