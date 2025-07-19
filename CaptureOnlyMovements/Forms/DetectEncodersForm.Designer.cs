namespace CaptureOnlyMovements.Forms
{
    partial class DetectEncodersForm
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            label1 = new System.Windows.Forms.Label();
            progress = new System.Windows.Forms.ProgressBar();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new System.Drawing.Point(12, 9);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(273, 25);
            label1.TabIndex = 0;
            label1.Text = "Detecting encoders, please wait...";
            // 
            // progressBar1
            // 
            progress.Location = new System.Drawing.Point(12, 37);
            progress.Name = "progressBar1";
            progress.Size = new System.Drawing.Size(296, 46);
            progress.TabIndex = 1;
            // 
            // DetectEncodersForm
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(10F, 25F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(320, 100);
            Controls.Add(progress);
            Controls.Add(label1);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            Name = "DetectEncodersForm";
            StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            Text = "DetectEncodersForm";
            Load += DetectEncodersForm_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ProgressBar progress;
    }
}