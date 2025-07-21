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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DetectEncodersForm));
            TitleLabel = new System.Windows.Forms.Label();
            ProgressBar = new System.Windows.Forms.ProgressBar();
            StatusLabel = new System.Windows.Forms.Label();
            Picture = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)Picture).BeginInit();
            SuspendLayout();
            // 
            // TitleLabel
            // 
            TitleLabel.AutoSize = true;
            TitleLabel.BackColor = System.Drawing.Color.Transparent;
            TitleLabel.ForeColor = System.Drawing.Color.White;
            TitleLabel.Location = new System.Drawing.Point(55, 97);
            TitleLabel.Name = "TitleLabel";
            TitleLabel.Size = new System.Drawing.Size(273, 25);
            TitleLabel.TabIndex = 0;
            TitleLabel.Text = "Detecting encoders, please wait...";
            // 
            // ProgressBar
            // 
            ProgressBar.BackColor = System.Drawing.Color.Black;
            ProgressBar.Location = new System.Drawing.Point(53, 125);
            ProgressBar.Name = "ProgressBar";
            ProgressBar.Size = new System.Drawing.Size(276, 31);
            ProgressBar.TabIndex = 1;
            // 
            // StatusLabel
            // 
            StatusLabel.AutoSize = true;
            StatusLabel.BackColor = System.Drawing.Color.Transparent;
            StatusLabel.Font = new System.Drawing.Font("Segoe UI", 7F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            StatusLabel.ForeColor = System.Drawing.Color.White;
            StatusLabel.Location = new System.Drawing.Point(53, 159);
            StatusLabel.Name = "StatusLabel";
            StatusLabel.Size = new System.Drawing.Size(276, 19);
            StatusLabel.TabIndex = 2;
            StatusLabel.Text = "Quering GPUs from system information.";
            // 
            // Picture
            // 
            Picture.BackColor = System.Drawing.Color.Black;
            Picture.Image = (System.Drawing.Image)resources.GetObject("Picture.Image");
            Picture.Location = new System.Drawing.Point(123, 39);
            Picture.Name = "Picture";
            Picture.Size = new System.Drawing.Size(136, 55);
            Picture.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            Picture.TabIndex = 3;
            Picture.TabStop = false;
            // 
            // DetectEncodersForm
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(10F, 25F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            BackColor = System.Drawing.Color.Black;
            ClientSize = new System.Drawing.Size(386, 230);
            Controls.Add(Picture);
            Controls.Add(StatusLabel);
            Controls.Add(ProgressBar);
            Controls.Add(TitleLabel);
            ForeColor = System.Drawing.Color.White;
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "DetectEncodersForm";
            ShowInTaskbar = false;
            StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            Text = "DetectEncodersForm";
            Load += DetectEncodersForm_Load;
            ((System.ComponentModel.ISupportInitialize)Picture).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Label TitleLabel;
        private System.Windows.Forms.ProgressBar ProgressBar;
        private System.Windows.Forms.Label StatusLabel;
        private System.Windows.Forms.PictureBox Picture;
    }
}