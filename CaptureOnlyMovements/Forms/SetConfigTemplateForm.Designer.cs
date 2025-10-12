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
            PresetList = new System.Windows.Forms.ListBox();
            label1 = new System.Windows.Forms.Label();
            PrefixTextBox = new System.Windows.Forms.TextBox();
            ProcessExeTextBox = new System.Windows.Forms.TextBox();
            label2 = new System.Windows.Forms.Label();
            ProcessActiveCheckbox = new System.Windows.Forms.CheckBox();
            StartButton = new System.Windows.Forms.Button();
            SelectProcessExeButton = new System.Windows.Forms.Button();
            SuspendLayout();
            // 
            // PresetList
            // 
            PresetList.FormattingEnabled = true;
            PresetList.Location = new System.Drawing.Point(12, 12);
            PresetList.Name = "PresetList";
            PresetList.Size = new System.Drawing.Size(328, 94);
            PresetList.TabIndex = 0;
            PresetList.SelectedIndexChanged += PresetList_SelectedIndexChanged;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new System.Drawing.Point(12, 115);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(36, 15);
            label1.TabIndex = 1;
            label1.Text = "Prefix";
            // 
            // PrefixTextBox
            // 
            PrefixTextBox.Location = new System.Drawing.Point(124, 112);
            PrefixTextBox.Name = "PrefixTextBox";
            PrefixTextBox.Size = new System.Drawing.Size(216, 23);
            PrefixTextBox.TabIndex = 2;
            // 
            // ProcessExeTextBox
            // 
            ProcessExeTextBox.Location = new System.Drawing.Point(124, 141);
            ProcessExeTextBox.Name = "ProcessExeTextBox";
            ProcessExeTextBox.Size = new System.Drawing.Size(216, 23);
            ProcessExeTextBox.TabIndex = 3;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new System.Drawing.Point(12, 141);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(106, 15);
            label2.TabIndex = 4;
            label2.Text = "Process executeble";
            // 
            // ProcessActiveCheckbox
            // 
            ProcessActiveCheckbox.AutoSize = true;
            ProcessActiveCheckbox.Location = new System.Drawing.Point(12, 173);
            ProcessActiveCheckbox.Name = "ProcessActiveCheckbox";
            ProcessActiveCheckbox.Size = new System.Drawing.Size(216, 19);
            ProcessActiveCheckbox.TabIndex = 5;
            ProcessActiveCheckbox.Text = "Only record when process has focus";
            ProcessActiveCheckbox.UseVisualStyleBackColor = true;
            // 
            // StartButton
            // 
            StartButton.Location = new System.Drawing.Point(12, 199);
            StartButton.Name = "StartButton";
            StartButton.Size = new System.Drawing.Size(328, 54);
            StartButton.TabIndex = 6;
            StartButton.Text = "Start";
            StartButton.UseVisualStyleBackColor = true;
            StartButton.Click += StartButton_Click;
            // 
            // SelectProcessExeButton
            // 
            SelectProcessExeButton.Location = new System.Drawing.Point(265, 170);
            SelectProcessExeButton.Name = "SelectProcessExeButton";
            SelectProcessExeButton.Size = new System.Drawing.Size(75, 23);
            SelectProcessExeButton.TabIndex = 7;
            SelectProcessExeButton.Text = "Select file";
            SelectProcessExeButton.UseVisualStyleBackColor = true;
            SelectProcessExeButton.Click += SelectProcessExeButton_Click;
            // 
            // SetConfigTemplateForm
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(352, 265);
            Controls.Add(SelectProcessExeButton);
            Controls.Add(StartButton);
            Controls.Add(ProcessActiveCheckbox);
            Controls.Add(label2);
            Controls.Add(ProcessExeTextBox);
            Controls.Add(PrefixTextBox);
            Controls.Add(label1);
            Controls.Add(PresetList);
            Name = "SetConfigTemplateForm";
            Text = "SetConfigTemplateForm";
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