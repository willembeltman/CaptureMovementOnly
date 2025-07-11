namespace CaptureOnlyMovements.Forms
{
    partial class SettingsForm
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
            tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            SaveButton = new System.Windows.Forms.Button();
            label1 = new System.Windows.Forms.Label();
            label2 = new System.Windows.Forms.Label();
            label3 = new System.Windows.Forms.Label();
            label4 = new System.Windows.Forms.Label();
            label5 = new System.Windows.Forms.Label();
            label6 = new System.Windows.Forms.Label();
            MaximumPixelDifferenceValue = new System.Windows.Forms.TextBox();
            MaximumDifferentPixelCount = new System.Windows.Forms.TextBox();
            Preset = new System.Windows.Forms.ComboBox();
            Fps = new System.Windows.Forms.ComboBox();
            label7 = new System.Windows.Forms.Label();
            UseGpu = new System.Windows.Forms.CheckBox();
            MinPlaybackSpeed = new System.Windows.Forms.TextBox();
            MaxLinesInDebug = new System.Windows.Forms.TextBox();
            label8 = new System.Windows.Forms.Label();
            Quality = new System.Windows.Forms.ComboBox();
            tableLayoutPanel1.SuspendLayout();
            SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.ColumnCount = 2;
            tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            tableLayoutPanel1.Controls.Add(SaveButton, 1, 9);
            tableLayoutPanel1.Controls.Add(label1, 0, 0);
            tableLayoutPanel1.Controls.Add(label2, 0, 1);
            tableLayoutPanel1.Controls.Add(label3, 0, 2);
            tableLayoutPanel1.Controls.Add(label4, 0, 3);
            tableLayoutPanel1.Controls.Add(label5, 0, 4);
            tableLayoutPanel1.Controls.Add(label6, 0, 5);
            tableLayoutPanel1.Controls.Add(MaximumPixelDifferenceValue, 1, 0);
            tableLayoutPanel1.Controls.Add(MaximumDifferentPixelCount, 1, 1);
            tableLayoutPanel1.Controls.Add(Preset, 1, 4);
            tableLayoutPanel1.Controls.Add(Fps, 1, 2);
            tableLayoutPanel1.Controls.Add(label7, 0, 6);
            tableLayoutPanel1.Controls.Add(UseGpu, 1, 6);
            tableLayoutPanel1.Controls.Add(MinPlaybackSpeed, 1, 5);
            tableLayoutPanel1.Controls.Add(MaxLinesInDebug, 1, 7);
            tableLayoutPanel1.Controls.Add(label8, 0, 7);
            tableLayoutPanel1.Controls.Add(Quality, 1, 3);
            tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            tableLayoutPanel1.Location = new System.Drawing.Point(5, 5);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 10;
            tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10F));
            tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10F));
            tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10F));
            tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10F));
            tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10F));
            tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10F));
            tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10F));
            tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10F));
            tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10F));
            tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10F));
            tableLayoutPanel1.Size = new System.Drawing.Size(422, 281);
            tableLayoutPanel1.TabIndex = 0;
            // 
            // SaveButton
            // 
            SaveButton.Dock = System.Windows.Forms.DockStyle.Fill;
            SaveButton.Location = new System.Drawing.Point(214, 255);
            SaveButton.Name = "SaveButton";
            SaveButton.Size = new System.Drawing.Size(205, 23);
            SaveButton.TabIndex = 16;
            SaveButton.Text = "Save and close";
            SaveButton.UseVisualStyleBackColor = true;
            SaveButton.Click += SaveButton_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new System.Drawing.Point(3, 0);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(167, 15);
            label1.TabIndex = 0;
            label1.Text = "MaximumPixelDifferenceValue";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new System.Drawing.Point(3, 28);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(167, 15);
            label2.TabIndex = 1;
            label2.Text = "MaximumPixelDifferenceValue";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new System.Drawing.Point(3, 56);
            label3.Name = "label3";
            label3.Size = new System.Drawing.Size(26, 15);
            label3.TabIndex = 2;
            label3.Text = "FPS";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new System.Drawing.Point(3, 84);
            label4.Name = "label4";
            label4.Size = new System.Drawing.Size(45, 15);
            label4.TabIndex = 3;
            label4.Text = "Quality";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new System.Drawing.Point(3, 112);
            label5.Name = "label5";
            label5.Size = new System.Drawing.Size(39, 15);
            label5.TabIndex = 4;
            label5.Text = "Preset";
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new System.Drawing.Point(3, 140);
            label6.Name = "label6";
            label6.Size = new System.Drawing.Size(107, 15);
            label6.TabIndex = 5;
            label6.Text = "MinPlaybackSpeed";
            // 
            // MaximumPixelDifferenceValue
            // 
            MaximumPixelDifferenceValue.Dock = System.Windows.Forms.DockStyle.Fill;
            MaximumPixelDifferenceValue.Location = new System.Drawing.Point(214, 3);
            MaximumPixelDifferenceValue.Name = "MaximumPixelDifferenceValue";
            MaximumPixelDifferenceValue.Size = new System.Drawing.Size(205, 23);
            MaximumPixelDifferenceValue.TabIndex = 6;
            // 
            // MaximumDifferentPixelCount
            // 
            MaximumDifferentPixelCount.Dock = System.Windows.Forms.DockStyle.Fill;
            MaximumDifferentPixelCount.Location = new System.Drawing.Point(214, 31);
            MaximumDifferentPixelCount.Name = "MaximumDifferentPixelCount";
            MaximumDifferentPixelCount.Size = new System.Drawing.Size(205, 23);
            MaximumDifferentPixelCount.TabIndex = 7;
            // 
            // Preset
            // 
            Preset.Dock = System.Windows.Forms.DockStyle.Fill;
            Preset.FormattingEnabled = true;
            Preset.Items.AddRange(new object[] { "veryfast", "faster", "fast", "medium", "slow", "slower", "veryslow" });
            Preset.Location = new System.Drawing.Point(214, 115);
            Preset.Name = "Preset";
            Preset.Size = new System.Drawing.Size(205, 23);
            Preset.TabIndex = 8;
            Preset.Text = "veryslow";
            // 
            // Fps
            // 
            Fps.Dock = System.Windows.Forms.DockStyle.Fill;
            Fps.FormattingEnabled = true;
            Fps.Items.AddRange(new object[] { "25", "30", "50", "60" });
            Fps.Location = new System.Drawing.Point(214, 59);
            Fps.Name = "Fps";
            Fps.Size = new System.Drawing.Size(205, 23);
            Fps.TabIndex = 10;
            Fps.Text = "60";
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Location = new System.Drawing.Point(3, 168);
            label7.Name = "label7";
            label7.Size = new System.Drawing.Size(48, 15);
            label7.TabIndex = 11;
            label7.Text = "UseGpu";
            // 
            // UseGpu
            // 
            UseGpu.AutoSize = true;
            UseGpu.Location = new System.Drawing.Point(214, 171);
            UseGpu.Name = "UseGpu";
            UseGpu.Size = new System.Drawing.Size(15, 14);
            UseGpu.TabIndex = 12;
            UseGpu.UseVisualStyleBackColor = true;
            // 
            // MinPlaybackSpeed
            // 
            MinPlaybackSpeed.Dock = System.Windows.Forms.DockStyle.Fill;
            MinPlaybackSpeed.Location = new System.Drawing.Point(214, 143);
            MinPlaybackSpeed.Name = "MinPlaybackSpeed";
            MinPlaybackSpeed.Size = new System.Drawing.Size(205, 23);
            MinPlaybackSpeed.TabIndex = 13;
            MinPlaybackSpeed.Text = "4";
            // 
            // MaxLinesInDebug
            // 
            MaxLinesInDebug.Dock = System.Windows.Forms.DockStyle.Fill;
            MaxLinesInDebug.Location = new System.Drawing.Point(214, 199);
            MaxLinesInDebug.Name = "MaxLinesInDebug";
            MaxLinesInDebug.Size = new System.Drawing.Size(205, 23);
            MaxLinesInDebug.TabIndex = 17;
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Location = new System.Drawing.Point(3, 196);
            label8.Name = "label8";
            label8.Size = new System.Drawing.Size(101, 15);
            label8.TabIndex = 18;
            label8.Text = "MaxLinesInDebug";
            // 
            // Quality
            // 
            Quality.Dock = System.Windows.Forms.DockStyle.Fill;
            Quality.FormattingEnabled = true;
            Quality.Items.AddRange(new object[] { "identical", "high", "medium", "low", "lower", "verylow" });
            Quality.Location = new System.Drawing.Point(214, 87);
            Quality.Name = "Quality";
            Quality.Size = new System.Drawing.Size(205, 23);
            Quality.TabIndex = 19;
            Quality.Text = "identical";
            // 
            // SettingsForm
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(432, 291);
            Controls.Add(tableLayoutPanel1);
            Name = "SettingsForm";
            Padding = new System.Windows.Forms.Padding(5);
            Text = "Settings";
            Load += Settings_Load;
            tableLayoutPanel1.ResumeLayout(false);
            tableLayoutPanel1.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox MaximumPixelDifferenceValue;
        private System.Windows.Forms.TextBox MaximumDifferentPixelCount;
        private System.Windows.Forms.ComboBox Preset;
        private System.Windows.Forms.ComboBox Fps;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.CheckBox UseGpu;
        private System.Windows.Forms.TextBox MinPlaybackSpeed;
        private System.Windows.Forms.Button SaveButton;
        private System.Windows.Forms.TextBox MaxLinesInDebug;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.ComboBox Quality;
    }
}