using System.Drawing;
using System.Windows.Forms;

namespace CaptureOnlyMovements.Forms
{
    partial class ConfigForm
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
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            PrefixPresetListLabel = new Label();
            PrefixPresetList = new DataGridView();
            MinPlaybackSpeedLabel = new Label();
            MinPlaybackSpeed = new TextBox();
            MaximumPixelDifferenceValueLabel = new Label();
            MaximumPixelDifferenceValue = new TextBox();
            MaximumDifferentPixelCountLabel = new Label();
            MaximumDifferentPixelCount = new TextBox();
            EncoderComboLabel = new Label();
            EncoderCombo = new ComboBox();
            PresetComboLabel = new Label();
            PresetCombo = new ComboBox();
            QualityComboLabel = new Label();
            QualityCombo = new ComboBox();
            FpsComboLabel = new Label();
            FpsCombo = new ComboBox();
            Note1 = new Label();
            Note2 = new Label();
            Note3 = new Label();
            SaveButton = new Button();
            ((System.ComponentModel.ISupportInitialize)PrefixPresetList).BeginInit();
            SuspendLayout();
            // 
            // PrefixPresetListLabel
            // 
            PrefixPresetListLabel.AutoSize = true;
            PrefixPresetListLabel.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            PrefixPresetListLabel.ForeColor = Color.WhiteSmoke;
            PrefixPresetListLabel.Location = new Point(13, 13);
            PrefixPresetListLabel.Name = "PrefixPresetListLabel";
            PrefixPresetListLabel.Size = new Size(80, 28);
            PrefixPresetListLabel.TabIndex = 0;
            PrefixPresetListLabel.Text = "Presets";
            // 
            // PrefixPresetList
            // 
            PrefixPresetList.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            PrefixPresetList.BackgroundColor = Color.FromArgb(45, 45, 45);
            PrefixPresetList.BorderStyle = BorderStyle.None;
            dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = Color.FromArgb(45, 45, 45);
            dataGridViewCellStyle1.Font = new Font("Segoe UI", 9F);
            dataGridViewCellStyle1.ForeColor = Color.WhiteSmoke;
            dataGridViewCellStyle1.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = DataGridViewTriState.True;
            PrefixPresetList.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            PrefixPresetList.ColumnHeadersHeight = 34;
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = Color.FromArgb(45, 45, 45);
            dataGridViewCellStyle2.Font = new Font("Segoe UI", 9F);
            dataGridViewCellStyle2.ForeColor = Color.WhiteSmoke;
            dataGridViewCellStyle2.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = DataGridViewTriState.False;
            PrefixPresetList.DefaultCellStyle = dataGridViewCellStyle2;
            PrefixPresetList.EnableHeadersVisualStyles = false;
            PrefixPresetList.GridColor = Color.Gray;
            PrefixPresetList.Location = new Point(316, 13);
            PrefixPresetList.Name = "PrefixPresetList";
            PrefixPresetList.RowHeadersWidth = 62;
            PrefixPresetList.Size = new Size(1024, 212);
            PrefixPresetList.TabIndex = 1;
            // 
            // MinPlaybackSpeedLabel
            // 
            MinPlaybackSpeedLabel.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            MinPlaybackSpeedLabel.Font = new Font("Segoe UI", 9F);
            MinPlaybackSpeedLabel.ForeColor = Color.WhiteSmoke;
            MinPlaybackSpeedLabel.Location = new Point(13, 530);
            MinPlaybackSpeedLabel.Name = "MinPlaybackSpeedLabel";
            MinPlaybackSpeedLabel.Size = new Size(287, 23);
            MinPlaybackSpeedLabel.TabIndex = 6;
            MinPlaybackSpeedLabel.Text = "Minimum Recording Speed";
            // 
            // MinPlaybackSpeed
            // 
            MinPlaybackSpeed.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            MinPlaybackSpeed.BackColor = Color.FromArgb(45, 45, 45);
            MinPlaybackSpeed.BorderStyle = BorderStyle.FixedSingle;
            MinPlaybackSpeed.Font = new Font("Segoe UI", 9F);
            MinPlaybackSpeed.ForeColor = Color.WhiteSmoke;
            MinPlaybackSpeed.Location = new Point(316, 525);
            MinPlaybackSpeed.Name = "MinPlaybackSpeed";
            MinPlaybackSpeed.Size = new Size(1020, 31);
            MinPlaybackSpeed.TabIndex = 7;
            // 
            // MaximumPixelDifferenceValueLabel
            // 
            MaximumPixelDifferenceValueLabel.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            MaximumPixelDifferenceValueLabel.Font = new Font("Segoe UI", 9F);
            MaximumPixelDifferenceValueLabel.ForeColor = Color.WhiteSmoke;
            MaximumPixelDifferenceValueLabel.Location = new Point(13, 240);
            MaximumPixelDifferenceValueLabel.Name = "MaximumPixelDifferenceValueLabel";
            MaximumPixelDifferenceValueLabel.Size = new Size(287, 23);
            MaximumPixelDifferenceValueLabel.TabIndex = 2;
            MaximumPixelDifferenceValueLabel.Text = "Minimum pixel difference (0-255*3)";
            // 
            // MaximumPixelDifferenceValue
            // 
            MaximumPixelDifferenceValue.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            MaximumPixelDifferenceValue.BackColor = Color.FromArgb(45, 45, 45);
            MaximumPixelDifferenceValue.BorderStyle = BorderStyle.FixedSingle;
            MaximumPixelDifferenceValue.Font = new Font("Segoe UI", 9F);
            MaximumPixelDifferenceValue.ForeColor = Color.WhiteSmoke;
            MaximumPixelDifferenceValue.Location = new Point(316, 235);
            MaximumPixelDifferenceValue.Name = "MaximumPixelDifferenceValue";
            MaximumPixelDifferenceValue.Size = new Size(1020, 31);
            MaximumPixelDifferenceValue.TabIndex = 3;
            // 
            // MaximumDifferentPixelCountLabel
            // 
            MaximumDifferentPixelCountLabel.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            MaximumDifferentPixelCountLabel.Font = new Font("Segoe UI", 9F);
            MaximumDifferentPixelCountLabel.ForeColor = Color.WhiteSmoke;
            MaximumDifferentPixelCountLabel.Location = new Point(13, 289);
            MaximumDifferentPixelCountLabel.Name = "MaximumDifferentPixelCountLabel";
            MaximumDifferentPixelCountLabel.Size = new Size(287, 23);
            MaximumDifferentPixelCountLabel.TabIndex = 4;
            MaximumDifferentPixelCountLabel.Text = "Maximum number of different pixels";
            // 
            // MaximumDifferentPixelCount
            // 
            MaximumDifferentPixelCount.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            MaximumDifferentPixelCount.BackColor = Color.FromArgb(45, 45, 45);
            MaximumDifferentPixelCount.BorderStyle = BorderStyle.FixedSingle;
            MaximumDifferentPixelCount.Font = new Font("Segoe UI", 9F);
            MaximumDifferentPixelCount.ForeColor = Color.WhiteSmoke;
            MaximumDifferentPixelCount.Location = new Point(316, 284);
            MaximumDifferentPixelCount.Name = "MaximumDifferentPixelCount";
            MaximumDifferentPixelCount.Size = new Size(1020, 31);
            MaximumDifferentPixelCount.TabIndex = 5;
            // 
            // EncoderComboLabel
            // 
            EncoderComboLabel.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            EncoderComboLabel.Font = new Font("Segoe UI", 9F);
            EncoderComboLabel.ForeColor = Color.WhiteSmoke;
            EncoderComboLabel.Location = new Point(13, 387);
            EncoderComboLabel.Name = "EncoderComboLabel";
            EncoderComboLabel.Size = new Size(287, 23);
            EncoderComboLabel.TabIndex = 8;
            EncoderComboLabel.Text = "Use gpu for encoding";
            // 
            // EncoderCombo
            // 
            EncoderCombo.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            EncoderCombo.BackColor = Color.FromArgb(45, 45, 45);
            EncoderCombo.DropDownStyle = ComboBoxStyle.DropDownList;
            EncoderCombo.FlatStyle = FlatStyle.Flat;
            EncoderCombo.ForeColor = Color.WhiteSmoke;
            EncoderCombo.Items.AddRange(new object[] { "software", "nvidia", "amd", "intel" });
            EncoderCombo.Location = new Point(316, 382);
            EncoderCombo.Name = "EncoderCombo";
            EncoderCombo.Size = new Size(1020, 33);
            EncoderCombo.TabIndex = 9;
            // 
            // PresetComboLabel
            // 
            PresetComboLabel.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            PresetComboLabel.Font = new Font("Segoe UI", 9F);
            PresetComboLabel.ForeColor = Color.WhiteSmoke;
            PresetComboLabel.Location = new Point(13, 482);
            PresetComboLabel.Name = "PresetComboLabel";
            PresetComboLabel.Size = new Size(287, 23);
            PresetComboLabel.TabIndex = 10;
            PresetComboLabel.Text = "Video Preset";
            // 
            // PresetCombo
            // 
            PresetCombo.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            PresetCombo.BackColor = Color.FromArgb(45, 45, 45);
            PresetCombo.DropDownStyle = ComboBoxStyle.DropDownList;
            PresetCombo.FlatStyle = FlatStyle.Flat;
            PresetCombo.ForeColor = Color.WhiteSmoke;
            PresetCombo.Items.AddRange(new object[] { "veryfast", "faster", "fast", "medium", "slow", "slower", "veryslow" });
            PresetCombo.Location = new Point(316, 477);
            PresetCombo.Name = "PresetCombo";
            PresetCombo.Size = new Size(1020, 33);
            PresetCombo.TabIndex = 11;
            // 
            // QualityComboLabel
            // 
            QualityComboLabel.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            QualityComboLabel.Font = new Font("Segoe UI", 9F);
            QualityComboLabel.ForeColor = Color.WhiteSmoke;
            QualityComboLabel.Location = new Point(13, 434);
            QualityComboLabel.Name = "QualityComboLabel";
            QualityComboLabel.Size = new Size(287, 23);
            QualityComboLabel.TabIndex = 12;
            QualityComboLabel.Text = "Video Quality";
            // 
            // QualityCombo
            // 
            QualityCombo.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            QualityCombo.BackColor = Color.FromArgb(45, 45, 45);
            QualityCombo.DropDownStyle = ComboBoxStyle.DropDownList;
            QualityCombo.FlatStyle = FlatStyle.Flat;
            QualityCombo.ForeColor = Color.WhiteSmoke;
            QualityCombo.Items.AddRange(new object[] { "identical", "high", "medium", "low", "lower", "verylow" });
            QualityCombo.Location = new Point(316, 429);
            QualityCombo.Name = "QualityCombo";
            QualityCombo.Size = new Size(1020, 33);
            QualityCombo.TabIndex = 13;
            // 
            // FpsComboLabel
            // 
            FpsComboLabel.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            FpsComboLabel.Font = new Font("Segoe UI", 9F);
            FpsComboLabel.ForeColor = Color.WhiteSmoke;
            FpsComboLabel.Location = new Point(13, 337);
            FpsComboLabel.Name = "FpsComboLabel";
            FpsComboLabel.Size = new Size(287, 23);
            FpsComboLabel.TabIndex = 14;
            FpsComboLabel.Text = "Video Playback FPS";
            // 
            // FpsCombo
            // 
            FpsCombo.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            FpsCombo.BackColor = Color.FromArgb(45, 45, 45);
            FpsCombo.DropDownStyle = ComboBoxStyle.DropDownList;
            FpsCombo.FlatStyle = FlatStyle.Flat;
            FpsCombo.ForeColor = Color.WhiteSmoke;
            FpsCombo.Items.AddRange(new object[] { "25", "30", "50", "60" });
            FpsCombo.Location = new Point(316, 332);
            FpsCombo.Name = "FpsCombo";
            FpsCombo.Size = new Size(1020, 33);
            FpsCombo.TabIndex = 15;
            // 
            // Note1
            // 
            Note1.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            Note1.Font = new Font("Segoe UI", 9F);
            Note1.ForeColor = Color.LightGray;
            Note1.Location = new Point(330, 575);
            Note1.Name = "Note1";
            Note1.Size = new Size(1006, 23);
            Note1.TabIndex = 16;
            Note1.Text = "Note: Realtime (value 1) will be difficult because the ";
            // 
            // Note2
            // 
            Note2.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            Note2.Font = new Font("Segoe UI", 9F);
            Note2.ForeColor = Color.LightGray;
            Note2.Location = new Point(330, 600);
            Note2.Name = "Note2";
            Note2.Size = new Size(1006, 23);
            Note2.TabIndex = 17;
            Note2.Text = "frames are being compared single threaded with no bufferpipeline.";
            // 
            // Note3
            // 
            Note3.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            Note3.Font = new Font("Segoe UI", 9F);
            Note3.ForeColor = Color.LightGray;
            Note3.Location = new Point(330, 625);
            Note3.Name = "Note3";
            Note3.Size = new Size(1006, 23);
            Note3.TabIndex = 18;
            Note3.Text = "Please use OBS for realtime capturing.";
            // 
            // SaveButton
            // 
            SaveButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            SaveButton.BackColor = Color.FromArgb(80, 80, 80);
            SaveButton.FlatAppearance.BorderSize = 0;
            SaveButton.FlatStyle = FlatStyle.Flat;
            SaveButton.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            SaveButton.ForeColor = Color.WhiteSmoke;
            SaveButton.Location = new Point(316, 660);
            SaveButton.Name = "SaveButton";
            SaveButton.Size = new Size(1022, 65);
            SaveButton.TabIndex = 19;
            SaveButton.Text = "Apply and close";
            SaveButton.UseVisualStyleBackColor = false;
            // 
            // ConfigForm
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(24, 24, 24);
            ClientSize = new Size(1353, 747);
            Controls.Add(PrefixPresetListLabel);
            Controls.Add(PrefixPresetList);
            Controls.Add(MaximumPixelDifferenceValueLabel);
            Controls.Add(MaximumPixelDifferenceValue);
            Controls.Add(MaximumDifferentPixelCountLabel);
            Controls.Add(MaximumDifferentPixelCount);
            Controls.Add(MinPlaybackSpeedLabel);
            Controls.Add(MinPlaybackSpeed);
            Controls.Add(EncoderComboLabel);
            Controls.Add(EncoderCombo);
            Controls.Add(PresetComboLabel);
            Controls.Add(PresetCombo);
            Controls.Add(QualityComboLabel);
            Controls.Add(QualityCombo);
            Controls.Add(FpsComboLabel);
            Controls.Add(FpsCombo);
            Controls.Add(Note1);
            Controls.Add(Note2);
            Controls.Add(Note3);
            Controls.Add(SaveButton);
            Font = new Font("Segoe UI", 9F);
            ForeColor = Color.WhiteSmoke;
            FormBorderStyle = FormBorderStyle.SizableToolWindow;
            Name = "ConfigForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Settings";
            ((System.ComponentModel.ISupportInitialize)PrefixPresetList).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }


        #endregion

        private System.Windows.Forms.Label PrefixPresetListLabel;
        private System.Windows.Forms.DataGridView PrefixPresetList;
        private System.Windows.Forms.Label MinPlaybackSpeedLabel;
        private System.Windows.Forms.TextBox MinPlaybackSpeed;
        private System.Windows.Forms.Label MaximumPixelDifferenceValueLabel;
        private System.Windows.Forms.TextBox MaximumPixelDifferenceValue;
        private System.Windows.Forms.Label MaximumDifferentPixelCountLabel;
        private System.Windows.Forms.TextBox MaximumDifferentPixelCount;
        private System.Windows.Forms.Label EncoderComboLabel;
        private System.Windows.Forms.ComboBox EncoderCombo;
        private System.Windows.Forms.Label PresetComboLabel;
        private System.Windows.Forms.ComboBox PresetCombo;
        private System.Windows.Forms.Label QualityComboLabel;
        private System.Windows.Forms.ComboBox QualityCombo;
        private System.Windows.Forms.Label FpsComboLabel;
        private System.Windows.Forms.ComboBox FpsCombo;
        private System.Windows.Forms.Label Note1;
        private System.Windows.Forms.Label Note2;
        private System.Windows.Forms.Label Note3;
        private System.Windows.Forms.Button SaveButton;
    }
}