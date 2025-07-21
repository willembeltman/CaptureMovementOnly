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
            TitleLabel = new System.Windows.Forms.Label();
            ProgressBar = new System.Windows.Forms.ProgressBar();
            StatusLabel = new System.Windows.Forms.Label();
            SuspendLayout();
            // 
            // TitleLabel
            // 
            TitleLabel.AutoSize = true;
            TitleLabel.Location = new System.Drawing.Point(20, 9);
            TitleLabel.Name = "TitleLabel";
            TitleLabel.Size = new System.Drawing.Size(273, 25);
            TitleLabel.TabIndex = 0;
            TitleLabel.Text = "Detecting encoders, please wait...";
            // 
            // progress
            // 
            ProgressBar.Location = new System.Drawing.Point(12, 37);
            ProgressBar.Name = "progress";
            ProgressBar.Size = new System.Drawing.Size(296, 46);
            ProgressBar.TabIndex = 1;
            // 
            // StatusLabel
            // 
            StatusLabel.AutoSize = true;
            StatusLabel.Font = new System.Drawing.Font("Segoe UI", 7F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            StatusLabel.Location = new System.Drawing.Point(12, 86);
            StatusLabel.Name = "StatusLabel";
            StatusLabel.Size = new System.Drawing.Size(276, 19);
            StatusLabel.TabIndex = 2;
            StatusLabel.Text = "Quering GPUs from system information.";
            // 
            // DetectEncodersForm
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(10F, 25F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(320, 121);
            Controls.Add(StatusLabel);
            Controls.Add(ProgressBar);
            Controls.Add(TitleLabel);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            Name = "DetectEncodersForm";
            StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            Text = "DetectEncodersForm";
            Load += DetectEncodersForm_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Label TitleLabel;
        private System.Windows.Forms.ProgressBar ProgressBar;
        private System.Windows.Forms.Label StatusLabel;
    }
}