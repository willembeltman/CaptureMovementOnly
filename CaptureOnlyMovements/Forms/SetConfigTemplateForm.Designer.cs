using System.Drawing;
using System.Windows.Forms;

namespace CaptureOnlyMovements.Forms
{
    partial class SetConfigTemplateForm
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
            PresetList = new ListBox();
            label1 = new Label();
            PrefixTextBox = new TextBox();
            ProcessExeTextBox = new TextBox();
            label2 = new Label();
            ProcessActiveCheckbox = new CheckBox();
            StartButton = new Button();
            SelectProcessExeButton = new Button();
            SuspendLayout();
            // 
            // PresetList
            // 
            PresetList.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            PresetList.BackColor = Color.FromArgb(64, 64, 64);
            PresetList.BorderStyle = BorderStyle.None;
            PresetList.ForeColor = Color.WhiteSmoke;
            PresetList.Location = new Point(12, 12);
            PresetList.Name = "PresetList";
            PresetList.Size = new Size(336, 90);
            PresetList.TabIndex = 0;
            PresetList.SelectedIndexChanged += PresetList_SelectedIndexChanged;
            // 
            // label1
            // 
            label1.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            label1.AutoSize = true;
            label1.Location = new Point(12, 112);
            label1.Name = "label1";
            label1.Size = new Size(36, 15);
            label1.TabIndex = 9;
            label1.Text = "Prefix";
            // 
            // PrefixTextBox
            // 
            PrefixTextBox.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            PrefixTextBox.BackColor = Color.FromArgb(64, 64, 64);
            PrefixTextBox.BorderStyle = BorderStyle.FixedSingle;
            PrefixTextBox.ForeColor = Color.WhiteSmoke;
            PrefixTextBox.Location = new Point(124, 109);
            PrefixTextBox.Name = "PrefixTextBox";
            PrefixTextBox.Size = new Size(224, 23);
            PrefixTextBox.TabIndex = 2;
            // 
            // ProcessExeTextBox
            // 
            ProcessExeTextBox.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            ProcessExeTextBox.BackColor = Color.FromArgb(64, 64, 64);
            ProcessExeTextBox.BorderStyle = BorderStyle.FixedSingle;
            ProcessExeTextBox.ForeColor = Color.WhiteSmoke;
            ProcessExeTextBox.Location = new Point(124, 140);
            ProcessExeTextBox.Name = "ProcessExeTextBox";
            ProcessExeTextBox.Size = new Size(224, 23);
            ProcessExeTextBox.TabIndex = 3;
            // 
            // label2
            // 
            label2.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            label2.AutoSize = true;
            label2.Location = new Point(12, 140);
            label2.Name = "label2";
            label2.Size = new Size(106, 15);
            label2.TabIndex = 8;
            label2.Text = "Process executable";
            // 
            // ProcessActiveCheckbox
            // 
            ProcessActiveCheckbox.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            ProcessActiveCheckbox.AutoSize = true;
            ProcessActiveCheckbox.ForeColor = Color.WhiteSmoke;
            ProcessActiveCheckbox.Location = new Point(14, 172);
            ProcessActiveCheckbox.Name = "ProcessActiveCheckbox";
            ProcessActiveCheckbox.Size = new Size(216, 19);
            ProcessActiveCheckbox.TabIndex = 5;
            ProcessActiveCheckbox.Text = "Only record when process has focus";
            ProcessActiveCheckbox.UseVisualStyleBackColor = false;
            // 
            // StartButton
            // 
            StartButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            StartButton.BackColor = Color.FromArgb(80, 80, 80);
            StartButton.FlatAppearance.BorderSize = 0;
            StartButton.FlatStyle = FlatStyle.Flat;
            StartButton.Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold);
            StartButton.ForeColor = Color.White;
            StartButton.Location = new Point(12, 200);
            StartButton.Name = "StartButton";
            StartButton.Size = new Size(336, 50);
            StartButton.TabIndex = 6;
            StartButton.Text = "Start";
            StartButton.UseVisualStyleBackColor = false;
            StartButton.Click += StartButton_Click;
            // 
            // SelectProcessExeButton
            // 
            SelectProcessExeButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            SelectProcessExeButton.BackColor = Color.FromArgb(50, 50, 50);
            SelectProcessExeButton.FlatAppearance.BorderSize = 0;
            SelectProcessExeButton.FlatStyle = FlatStyle.Flat;
            SelectProcessExeButton.ForeColor = Color.WhiteSmoke;
            SelectProcessExeButton.Location = new Point(273, 170);
            SelectProcessExeButton.Name = "SelectProcessExeButton";
            SelectProcessExeButton.Size = new Size(75, 23);
            SelectProcessExeButton.TabIndex = 7;
            SelectProcessExeButton.Text = "Select file";
            SelectProcessExeButton.UseVisualStyleBackColor = false;
            SelectProcessExeButton.Click += SelectProcessExeButton_Click;
            // 
            // SetConfigTemplateForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(24, 24, 24);
            ClientSize = new Size(360, 265);
            Controls.Add(SelectProcessExeButton);
            Controls.Add(StartButton);
            Controls.Add(ProcessActiveCheckbox);
            Controls.Add(label2);
            Controls.Add(ProcessExeTextBox);
            Controls.Add(PrefixTextBox);
            Controls.Add(label1);
            Controls.Add(PresetList);
            Font = new Font("Segoe UI", 9F);
            ForeColor = Color.WhiteSmoke;
            FormBorderStyle = FormBorderStyle.SizableToolWindow;
            MinimumSize = new Size(360, 275);
            Name = "SetConfigTemplateForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Config Preset Manager";
            Load += SetConfigTemplateForm_Load;
            ResumeLayout(false);
            PerformLayout();
        }


        #endregion

        private System.Windows.Forms.ListBox PresetList;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox PrefixTextBox;
        private System.Windows.Forms.TextBox ProcessExeTextBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckBox ProcessActiveCheckbox;
        private System.Windows.Forms.Button StartButton;
        private System.Windows.Forms.Button SelectProcessExeButton;
    }
}