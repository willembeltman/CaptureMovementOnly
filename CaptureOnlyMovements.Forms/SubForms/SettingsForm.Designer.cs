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
            components = new System.ComponentModel.Container();
            tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            label1 = new System.Windows.Forms.Label();
            label2 = new System.Windows.Forms.Label();
            MaximumPixelDifferenceValue = new System.Windows.Forms.TextBox();
            MaximumDifferentPixelCount = new System.Windows.Forms.TextBox();
            label4 = new System.Windows.Forms.Label();
            Quality = new System.Windows.Forms.ComboBox();
            label5 = new System.Windows.Forms.Label();
            Preset = new System.Windows.Forms.ComboBox();
            label7 = new System.Windows.Forms.Label();
            UseGpu = new System.Windows.Forms.CheckBox();
            Fps = new System.Windows.Forms.ComboBox();
            label3 = new System.Windows.Forms.Label();
            MinPlaybackSpeed = new System.Windows.Forms.TextBox();
            label6 = new System.Windows.Forms.Label();
            label9 = new System.Windows.Forms.Label();
            SaveButton = new System.Windows.Forms.Button();
            label10 = new System.Windows.Forms.Label();
            FpsCounterLabel = new System.Windows.Forms.Label();
            Timer = new System.Windows.Forms.Timer(components);
            tableLayoutPanel1.SuspendLayout();
            SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.ColumnCount = 2;
            tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 40F));
            tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 60F));
            tableLayoutPanel1.Controls.Add(label1, 0, 0);
            tableLayoutPanel1.Controls.Add(label2, 0, 1);
            tableLayoutPanel1.Controls.Add(MaximumPixelDifferenceValue, 1, 0);
            tableLayoutPanel1.Controls.Add(MaximumDifferentPixelCount, 1, 1);
            tableLayoutPanel1.Controls.Add(label4, 0, 2);
            tableLayoutPanel1.Controls.Add(Quality, 1, 2);
            tableLayoutPanel1.Controls.Add(label5, 0, 3);
            tableLayoutPanel1.Controls.Add(Preset, 1, 3);
            tableLayoutPanel1.Controls.Add(label7, 0, 4);
            tableLayoutPanel1.Controls.Add(UseGpu, 1, 4);
            tableLayoutPanel1.Controls.Add(Fps, 1, 5);
            tableLayoutPanel1.Controls.Add(label3, 0, 5);
            tableLayoutPanel1.Controls.Add(MinPlaybackSpeed, 1, 6);
            tableLayoutPanel1.Controls.Add(label6, 0, 6);
            tableLayoutPanel1.Controls.Add(label9, 1, 7);
            tableLayoutPanel1.Controls.Add(SaveButton, 1, 9);
            tableLayoutPanel1.Controls.Add(label10, 1, 8);
            tableLayoutPanel1.Controls.Add(FpsCounterLabel, 0, 9);
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
            tableLayoutPanel1.Size = new System.Drawing.Size(550, 333);
            tableLayoutPanel1.TabIndex = 0;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new System.Drawing.Point(3, 0);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(194, 15);
            label1.TabIndex = 0;
            label1.Text = "Minimum pixel difference (0-255*3)";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new System.Drawing.Point(3, 33);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(200, 15);
            label2.TabIndex = 1;
            label2.Text = "Maximum number of different pixels";
            // 
            // MaximumPixelDifferenceValue
            // 
            MaximumPixelDifferenceValue.Dock = System.Windows.Forms.DockStyle.Fill;
            MaximumPixelDifferenceValue.Location = new System.Drawing.Point(223, 3);
            MaximumPixelDifferenceValue.Name = "MaximumPixelDifferenceValue";
            MaximumPixelDifferenceValue.Size = new System.Drawing.Size(324, 23);
            MaximumPixelDifferenceValue.TabIndex = 6;
            // 
            // MaximumDifferentPixelCount
            // 
            MaximumDifferentPixelCount.Dock = System.Windows.Forms.DockStyle.Fill;
            MaximumDifferentPixelCount.Location = new System.Drawing.Point(223, 36);
            MaximumDifferentPixelCount.Name = "MaximumDifferentPixelCount";
            MaximumDifferentPixelCount.Size = new System.Drawing.Size(324, 23);
            MaximumDifferentPixelCount.TabIndex = 7;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new System.Drawing.Point(3, 66);
            label4.Name = "label4";
            label4.Size = new System.Drawing.Size(45, 15);
            label4.TabIndex = 3;
            label4.Text = "Quality";
            // 
            // Quality
            // 
            Quality.Dock = System.Windows.Forms.DockStyle.Fill;
            Quality.FormattingEnabled = true;
            Quality.Items.AddRange(new object[] { "identical", "high", "medium", "low", "lower", "verylow" });
            Quality.Location = new System.Drawing.Point(223, 69);
            Quality.Name = "Quality";
            Quality.Size = new System.Drawing.Size(324, 23);
            Quality.TabIndex = 19;
            Quality.Text = "identical";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new System.Drawing.Point(3, 99);
            label5.Name = "label5";
            label5.Size = new System.Drawing.Size(39, 15);
            label5.TabIndex = 4;
            label5.Text = "Preset";
            // 
            // Preset
            // 
            Preset.Dock = System.Windows.Forms.DockStyle.Fill;
            Preset.FormattingEnabled = true;
            Preset.Items.AddRange(new object[] { "veryfast", "faster", "fast", "medium", "slow", "slower", "veryslow" });
            Preset.Location = new System.Drawing.Point(223, 102);
            Preset.Name = "Preset";
            Preset.Size = new System.Drawing.Size(324, 23);
            Preset.TabIndex = 8;
            Preset.Text = "veryslow";
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Location = new System.Drawing.Point(3, 132);
            label7.Name = "label7";
            label7.Size = new System.Drawing.Size(48, 15);
            label7.TabIndex = 11;
            label7.Text = "UseGpu";
            // 
            // UseGpu
            // 
            UseGpu.AutoSize = true;
            UseGpu.Location = new System.Drawing.Point(223, 135);
            UseGpu.Name = "UseGpu";
            UseGpu.Size = new System.Drawing.Size(15, 14);
            UseGpu.TabIndex = 12;
            UseGpu.UseVisualStyleBackColor = true;
            // 
            // Fps
            // 
            Fps.Dock = System.Windows.Forms.DockStyle.Fill;
            Fps.FormattingEnabled = true;
            Fps.Items.AddRange(new object[] { "25", "30", "50", "60" });
            Fps.Location = new System.Drawing.Point(223, 168);
            Fps.Name = "Fps";
            Fps.Size = new System.Drawing.Size(324, 23);
            Fps.TabIndex = 10;
            Fps.Text = "60";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new System.Drawing.Point(3, 165);
            label3.Name = "label3";
            label3.Size = new System.Drawing.Size(76, 15);
            label3.TabIndex = 2;
            label3.Text = "Playback FPS";
            // 
            // MinPlaybackSpeed
            // 
            MinPlaybackSpeed.Dock = System.Windows.Forms.DockStyle.Fill;
            MinPlaybackSpeed.Location = new System.Drawing.Point(223, 201);
            MinPlaybackSpeed.Name = "MinPlaybackSpeed";
            MinPlaybackSpeed.Size = new System.Drawing.Size(324, 23);
            MinPlaybackSpeed.TabIndex = 13;
            MinPlaybackSpeed.Text = "4";
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new System.Drawing.Point(3, 198);
            label6.Name = "label6";
            label6.Size = new System.Drawing.Size(145, 15);
            label6.TabIndex = 5;
            label6.Text = "Minimum Playback Speed";
            // 
            // label9
            // 
            label9.AutoSize = true;
            label9.Location = new System.Drawing.Point(223, 231);
            label9.Name = "label9";
            label9.Size = new System.Drawing.Size(322, 30);
            label9.TabIndex = 20;
            label9.Text = "Note: Realtime (value 1) will be difficult because the frames are being compared on the CPU single threaded.";
            // 
            // SaveButton
            // 
            SaveButton.Dock = System.Windows.Forms.DockStyle.Fill;
            SaveButton.Location = new System.Drawing.Point(223, 300);
            SaveButton.Name = "SaveButton";
            SaveButton.Size = new System.Drawing.Size(324, 30);
            SaveButton.TabIndex = 16;
            SaveButton.Text = "Save and close";
            SaveButton.UseVisualStyleBackColor = true;
            SaveButton.Click += SaveButton_Click;
            // 
            // label10
            // 
            label10.AutoSize = true;
            label10.Location = new System.Drawing.Point(223, 264);
            label10.Name = "label10";
            label10.Size = new System.Drawing.Size(207, 15);
            label10.TabIndex = 21;
            label10.Text = "Please use OBS for realtime capturing.";
            // 
            // FpsCounterLabel
            // 
            FpsCounterLabel.AutoSize = true;
            FpsCounterLabel.Location = new System.Drawing.Point(3, 297);
            FpsCounterLabel.Name = "FpsCounterLabel";
            FpsCounterLabel.Size = new System.Drawing.Size(0, 15);
            FpsCounterLabel.TabIndex = 22;
            // 
            // Timer
            // 
            Timer.Tick += Timer_Tick;
            // 
            // ConfigForm
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(560, 343);
            Controls.Add(tableLayoutPanel1);
            Name = "ConfigForm";
            Padding = new System.Windows.Forms.Padding(5);
            Text = "Settings";
            Load += ConfigForm_Load;
            VisibleChanged += ConfigForm_VisibleChanged;
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
        private System.Windows.Forms.ComboBox Quality;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label FpsCounterLabel;
        private System.Windows.Forms.Timer Timer;
    }
}