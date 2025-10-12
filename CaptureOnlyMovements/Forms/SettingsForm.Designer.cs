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
            PrefixPresetListLabel = new System.Windows.Forms.Label();
            PrefixPresetList = new System.Windows.Forms.DataGridView();
            MinPlaybackSpeedLabel = new System.Windows.Forms.Label();
            MinPlaybackSpeed = new System.Windows.Forms.TextBox();
            MaximumPixelDifferenceValueLabel = new System.Windows.Forms.Label();
            MaximumPixelDifferenceValue = new System.Windows.Forms.TextBox();
            MaximumDifferentPixelCountLabel = new System.Windows.Forms.Label();
            MaximumDifferentPixelCount = new System.Windows.Forms.TextBox();
            EncoderComboLabel = new System.Windows.Forms.Label();
            EncoderCombo = new System.Windows.Forms.ComboBox();
            PresetComboLabel = new System.Windows.Forms.Label();
            PresetCombo = new System.Windows.Forms.ComboBox();
            QualityComboLabel = new System.Windows.Forms.Label();
            QualityCombo = new System.Windows.Forms.ComboBox();
            FpsComboLabel = new System.Windows.Forms.Label();
            FpsCombo = new System.Windows.Forms.ComboBox();
            Note1 = new System.Windows.Forms.Label();
            Note2 = new System.Windows.Forms.Label();
            Note3 = new System.Windows.Forms.Label();
            SaveButton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)PrefixPresetList).BeginInit();
            SuspendLayout();
            // 
            // PrefixPresetListLabel
            // 
            PrefixPresetListLabel.AutoSize = true;
            PrefixPresetListLabel.Location = new System.Drawing.Point(13, 13);
            PrefixPresetListLabel.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            PrefixPresetListLabel.Name = "PrefixPresetListLabel";
            PrefixPresetListLabel.Size = new System.Drawing.Size(68, 25);
            PrefixPresetListLabel.TabIndex = 44;
            PrefixPresetListLabel.Text = "Presets";
            // 
            // PrefixPresetList
            // 
            PrefixPresetList.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            PrefixPresetList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            PrefixPresetList.Location = new System.Drawing.Point(316, 13);
            PrefixPresetList.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            PrefixPresetList.Name = "PrefixPresetList";
            PrefixPresetList.RowHeadersWidth = 62;
            PrefixPresetList.Size = new System.Drawing.Size(991, 405);
            PrefixPresetList.TabIndex = 45;
            // 
            // MinPlaybackSpeedLabel
            // 
            MinPlaybackSpeedLabel.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            MinPlaybackSpeedLabel.AutoSize = true;
            MinPlaybackSpeedLabel.Location = new System.Drawing.Point(13, 723);
            MinPlaybackSpeedLabel.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            MinPlaybackSpeedLabel.Name = "MinPlaybackSpeedLabel";
            MinPlaybackSpeedLabel.Size = new System.Drawing.Size(228, 25);
            MinPlaybackSpeedLabel.TabIndex = 32;
            MinPlaybackSpeedLabel.Text = "Minimum Recording Speed";
            // 
            // MinPlaybackSpeed
            // 
            MinPlaybackSpeed.AccessibleDescription = "Note: Realtime (value 1) will be difficult because the frames are being compared single threaded with no bufferpipeline.";
            MinPlaybackSpeed.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            MinPlaybackSpeed.Location = new System.Drawing.Point(316, 718);
            MinPlaybackSpeed.Margin = new System.Windows.Forms.Padding(6, 5, 6, 5);
            MinPlaybackSpeed.Name = "MinPlaybackSpeed";
            MinPlaybackSpeed.Size = new System.Drawing.Size(987, 31);
            MinPlaybackSpeed.TabIndex = 38;
            // 
            // MaximumPixelDifferenceValueLabel
            // 
            MaximumPixelDifferenceValueLabel.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            MaximumPixelDifferenceValueLabel.AutoSize = true;
            MaximumPixelDifferenceValueLabel.Location = new System.Drawing.Point(13, 433);
            MaximumPixelDifferenceValueLabel.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            MaximumPixelDifferenceValueLabel.Name = "MaximumPixelDifferenceValueLabel";
            MaximumPixelDifferenceValueLabel.Size = new System.Drawing.Size(292, 25);
            MaximumPixelDifferenceValueLabel.TabIndex = 27;
            MaximumPixelDifferenceValueLabel.Text = "Minimum pixel difference (0-255*3)";
            // 
            // MaximumPixelDifferenceValue
            // 
            MaximumPixelDifferenceValue.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            MaximumPixelDifferenceValue.Location = new System.Drawing.Point(316, 428);
            MaximumPixelDifferenceValue.Margin = new System.Windows.Forms.Padding(6, 5, 6, 5);
            MaximumPixelDifferenceValue.Name = "MaximumPixelDifferenceValue";
            MaximumPixelDifferenceValue.Size = new System.Drawing.Size(987, 31);
            MaximumPixelDifferenceValue.TabIndex = 33;
            // 
            // MaximumDifferentPixelCountLabel
            // 
            MaximumDifferentPixelCountLabel.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            MaximumDifferentPixelCountLabel.AutoSize = true;
            MaximumDifferentPixelCountLabel.Location = new System.Drawing.Point(13, 482);
            MaximumDifferentPixelCountLabel.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            MaximumDifferentPixelCountLabel.Name = "MaximumDifferentPixelCountLabel";
            MaximumDifferentPixelCountLabel.Size = new System.Drawing.Size(301, 25);
            MaximumDifferentPixelCountLabel.TabIndex = 28;
            MaximumDifferentPixelCountLabel.Text = "Maximum number of different pixels";
            // 
            // MaximumDifferentPixelCount
            // 
            MaximumDifferentPixelCount.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            MaximumDifferentPixelCount.Location = new System.Drawing.Point(316, 477);
            MaximumDifferentPixelCount.Margin = new System.Windows.Forms.Padding(6, 5, 6, 5);
            MaximumDifferentPixelCount.Name = "MaximumDifferentPixelCount";
            MaximumDifferentPixelCount.Size = new System.Drawing.Size(987, 31);
            MaximumDifferentPixelCount.TabIndex = 34;
            // 
            // EncoderComboLabel
            // 
            EncoderComboLabel.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            EncoderComboLabel.AutoSize = true;
            EncoderComboLabel.Location = new System.Drawing.Point(13, 580);
            EncoderComboLabel.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            EncoderComboLabel.Name = "EncoderComboLabel";
            EncoderComboLabel.Size = new System.Drawing.Size(185, 25);
            EncoderComboLabel.TabIndex = 37;
            EncoderComboLabel.Text = "Use gpu for encoding";
            // 
            // EncoderCombo
            // 
            EncoderCombo.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            EncoderCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            EncoderCombo.FormattingEnabled = true;
            EncoderCombo.Items.AddRange(new object[] { "software", "nvidia", "amd", "intel" });
            EncoderCombo.Location = new System.Drawing.Point(316, 575);
            EncoderCombo.Margin = new System.Windows.Forms.Padding(6, 5, 6, 5);
            EncoderCombo.Name = "EncoderCombo";
            EncoderCombo.Size = new System.Drawing.Size(987, 33);
            EncoderCombo.TabIndex = 43;
            // 
            // PresetComboLabel
            // 
            PresetComboLabel.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            PresetComboLabel.AutoSize = true;
            PresetComboLabel.Location = new System.Drawing.Point(13, 675);
            PresetComboLabel.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            PresetComboLabel.Name = "PresetComboLabel";
            PresetComboLabel.Size = new System.Drawing.Size(111, 25);
            PresetComboLabel.TabIndex = 31;
            PresetComboLabel.Text = "Video Preset";
            // 
            // PresetCombo
            // 
            PresetCombo.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            PresetCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            PresetCombo.FormattingEnabled = true;
            PresetCombo.Items.AddRange(new object[] { "veryfast", "faster", "fast", "medium", "slow", "slower", "veryslow" });
            PresetCombo.Location = new System.Drawing.Point(316, 670);
            PresetCombo.Margin = new System.Windows.Forms.Padding(6, 5, 6, 5);
            PresetCombo.Name = "PresetCombo";
            PresetCombo.Size = new System.Drawing.Size(987, 33);
            PresetCombo.TabIndex = 35;
            // 
            // QualityComboLabel
            // 
            QualityComboLabel.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            QualityComboLabel.AutoSize = true;
            QualityComboLabel.Location = new System.Drawing.Point(13, 627);
            QualityComboLabel.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            QualityComboLabel.Name = "QualityComboLabel";
            QualityComboLabel.Size = new System.Drawing.Size(119, 25);
            QualityComboLabel.TabIndex = 30;
            QualityComboLabel.Text = "Video Quality";
            // 
            // QualityCombo
            // 
            QualityCombo.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            QualityCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            QualityCombo.FormattingEnabled = true;
            QualityCombo.Items.AddRange(new object[] { "identical", "high", "medium", "low", "lower", "verylow" });
            QualityCombo.Location = new System.Drawing.Point(316, 622);
            QualityCombo.Margin = new System.Windows.Forms.Padding(6, 5, 6, 5);
            QualityCombo.Name = "QualityCombo";
            QualityCombo.Size = new System.Drawing.Size(987, 33);
            QualityCombo.TabIndex = 40;
            // 
            // FpsComboLabel
            // 
            FpsComboLabel.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            FpsComboLabel.AutoSize = true;
            FpsComboLabel.Location = new System.Drawing.Point(13, 530);
            FpsComboLabel.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            FpsComboLabel.Name = "FpsComboLabel";
            FpsComboLabel.Size = new System.Drawing.Size(166, 25);
            FpsComboLabel.TabIndex = 29;
            FpsComboLabel.Text = "Video Playback FPS";
            // 
            // FpsCombo
            // 
            FpsCombo.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            FpsCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            FpsCombo.FormattingEnabled = true;
            FpsCombo.Items.AddRange(new object[] { "25", "30", "50", "60" });
            FpsCombo.Location = new System.Drawing.Point(316, 525);
            FpsCombo.Margin = new System.Windows.Forms.Padding(6, 5, 6, 5);
            FpsCombo.Name = "FpsCombo";
            FpsCombo.Size = new System.Drawing.Size(987, 33);
            FpsCombo.TabIndex = 36;
            // 
            // Note1
            // 
            Note1.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            Note1.AutoSize = true;
            Note1.Location = new System.Drawing.Point(330, 768);
            Note1.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            Note1.Name = "Note1";
            Note1.Size = new System.Drawing.Size(421, 25);
            Note1.TabIndex = 41;
            Note1.Text = "Note: Realtime (value 1) will be difficult because the ";
            // 
            // Note2
            // 
            Note2.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            Note2.AutoSize = true;
            Note2.Location = new System.Drawing.Point(330, 793);
            Note2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            Note2.Name = "Note2";
            Note2.Size = new System.Drawing.Size(541, 25);
            Note2.TabIndex = 46;
            Note2.Text = "frames are being compared single threaded with no bufferpipeline.";
            // 
            // Note3
            // 
            Note3.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            Note3.AutoSize = true;
            Note3.Location = new System.Drawing.Point(330, 818);
            Note3.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            Note3.Name = "Note3";
            Note3.Size = new System.Drawing.Size(312, 25);
            Note3.TabIndex = 42;
            Note3.Text = "Please use OBS for realtime capturing.";
            // 
            // SaveButton
            // 
            SaveButton.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            SaveButton.Location = new System.Drawing.Point(316, 853);
            SaveButton.Margin = new System.Windows.Forms.Padding(6, 5, 6, 5);
            SaveButton.Name = "SaveButton";
            SaveButton.Size = new System.Drawing.Size(989, 65);
            SaveButton.TabIndex = 39;
            SaveButton.Text = "Apply and close";
            SaveButton.UseVisualStyleBackColor = true;
            // 
            // ConfigForm
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(10F, 25F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(1317, 932);
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
            Margin = new System.Windows.Forms.Padding(6, 5, 6, 5);
            Name = "ConfigForm";
            Padding = new System.Windows.Forms.Padding(7, 8, 7, 8);
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