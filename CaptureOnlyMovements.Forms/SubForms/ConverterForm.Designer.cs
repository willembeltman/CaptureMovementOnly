namespace CaptureOnlyMovements.Forms.SubForms
{
    partial class ConverterForm
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
            AddFilesButton = new System.Windows.Forms.Button();
            MoveSelectedFileUpButton = new System.Windows.Forms.Button();
            MoveSelectedFileDownButton = new System.Windows.Forms.Button();
            DeleteSelectedFilesButton = new System.Windows.Forms.Button();
            FileGrid = new System.Windows.Forms.DataGridView();
            Console = new CaptureOnlyMovements.Forms.Controls.ConsoleControl();
            tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            groupBox3 = new System.Windows.Forms.GroupBox();
            OutputFpsLabel = new System.Windows.Forms.Label();
            InputFpsLabel = new System.Windows.Forms.Label();
            ShowDiffernceCheckbox = new System.Windows.Forms.CheckBox();
            StartButton = new System.Windows.Forms.Button();
            StopButton = new System.Windows.Forms.Button();
            groupBox2 = new System.Windows.Forms.GroupBox();
            groupBox1 = new System.Windows.Forms.GroupBox();
            Rectengles = new System.Windows.Forms.DataGridView();
            AddRectangeButton = new System.Windows.Forms.Button();
            DeleteSelectedRectangleButton = new System.Windows.Forms.Button();
            ConsoleFFMpeg = new CaptureOnlyMovements.Forms.Controls.ConsoleControl();
            displayControl1 = new CaptureOnlyMovements.Forms.Controls.DisplayControl();
            Timer = new System.Windows.Forms.Timer(components);
            ((System.ComponentModel.ISupportInitialize)FileGrid).BeginInit();
            tableLayoutPanel1.SuspendLayout();
            groupBox3.SuspendLayout();
            groupBox2.SuspendLayout();
            groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)Rectengles).BeginInit();
            SuspendLayout();
            // 
            // AddFilesButton
            // 
            AddFilesButton.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            AddFilesButton.Location = new System.Drawing.Point(6, 157);
            AddFilesButton.Name = "AddFilesButton";
            AddFilesButton.Size = new System.Drawing.Size(314, 23);
            AddFilesButton.TabIndex = 0;
            AddFilesButton.Text = "Add file(s)";
            AddFilesButton.UseVisualStyleBackColor = true;
            AddFilesButton.Click += AddFilesButton_Click;
            // 
            // MoveSelectedFileUpButton
            // 
            MoveSelectedFileUpButton.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
            MoveSelectedFileUpButton.Enabled = false;
            MoveSelectedFileUpButton.Location = new System.Drawing.Point(390, 157);
            MoveSelectedFileUpButton.Name = "MoveSelectedFileUpButton";
            MoveSelectedFileUpButton.Size = new System.Drawing.Size(51, 23);
            MoveSelectedFileUpButton.TabIndex = 4;
            MoveSelectedFileUpButton.Text = "Up";
            MoveSelectedFileUpButton.UseVisualStyleBackColor = true;
            MoveSelectedFileUpButton.Click += MoveSelectedFileUpButton_Click;
            // 
            // MoveSelectedFileDownButton
            // 
            MoveSelectedFileDownButton.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
            MoveSelectedFileDownButton.Enabled = false;
            MoveSelectedFileDownButton.Location = new System.Drawing.Point(447, 157);
            MoveSelectedFileDownButton.Name = "MoveSelectedFileDownButton";
            MoveSelectedFileDownButton.Size = new System.Drawing.Size(51, 23);
            MoveSelectedFileDownButton.TabIndex = 5;
            MoveSelectedFileDownButton.Text = "Down";
            MoveSelectedFileDownButton.UseVisualStyleBackColor = true;
            MoveSelectedFileDownButton.Click += MoveSelectedFileDownButton_Click;
            // 
            // DeleteSelectedFilesButton
            // 
            DeleteSelectedFilesButton.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
            DeleteSelectedFilesButton.Enabled = false;
            DeleteSelectedFilesButton.Location = new System.Drawing.Point(326, 157);
            DeleteSelectedFilesButton.Name = "DeleteSelectedFilesButton";
            DeleteSelectedFilesButton.Size = new System.Drawing.Size(58, 23);
            DeleteSelectedFilesButton.TabIndex = 6;
            DeleteSelectedFilesButton.Text = "Delete";
            DeleteSelectedFilesButton.UseVisualStyleBackColor = true;
            DeleteSelectedFilesButton.Click += DeleteSelectedFileButton_Click;
            // 
            // FileGrid
            // 
            FileGrid.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            FileGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            FileGrid.Location = new System.Drawing.Point(6, 22);
            FileGrid.Name = "FileGrid";
            FileGrid.Size = new System.Drawing.Size(492, 129);
            FileGrid.TabIndex = 7;
            FileGrid.SelectionChanged += FileGrid_SelectionChanged;
            // 
            // Console
            // 
            Console.BackColor = System.Drawing.Color.Black;
            Console.Dock = System.Windows.Forms.DockStyle.Fill;
            Console.ForeColor = System.Drawing.Color.White;
            Console.Location = new System.Drawing.Point(513, 195);
            Console.Name = "Console";
            Console.Size = new System.Drawing.Size(505, 186);
            Console.TabIndex = 12;
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            tableLayoutPanel1.ColumnCount = 2;
            tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            tableLayoutPanel1.Controls.Add(groupBox3, 0, 2);
            tableLayoutPanel1.Controls.Add(groupBox2, 0, 0);
            tableLayoutPanel1.Controls.Add(groupBox1, 0, 1);
            tableLayoutPanel1.Controls.Add(ConsoleFFMpeg, 1, 2);
            tableLayoutPanel1.Controls.Add(Console, 1, 1);
            tableLayoutPanel1.Controls.Add(displayControl1, 1, 0);
            tableLayoutPanel1.Location = new System.Drawing.Point(12, 12);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 3;
            tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.3333321F));
            tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.3333321F));
            tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.3333321F));
            tableLayoutPanel1.Size = new System.Drawing.Size(1021, 576);
            tableLayoutPanel1.TabIndex = 15;
            // 
            // groupBox3
            // 
            groupBox3.Controls.Add(OutputFpsLabel);
            groupBox3.Controls.Add(InputFpsLabel);
            groupBox3.Controls.Add(ShowDiffernceCheckbox);
            groupBox3.Controls.Add(StartButton);
            groupBox3.Controls.Add(StopButton);
            groupBox3.Dock = System.Windows.Forms.DockStyle.Fill;
            groupBox3.Location = new System.Drawing.Point(3, 387);
            groupBox3.Name = "groupBox3";
            groupBox3.Size = new System.Drawing.Size(504, 186);
            groupBox3.TabIndex = 17;
            groupBox3.TabStop = false;
            groupBox3.Text = "Start / Stop";
            // 
            // OutputFpsLabel
            // 
            OutputFpsLabel.AutoSize = true;
            OutputFpsLabel.Location = new System.Drawing.Point(424, 143);
            OutputFpsLabel.Name = "OutputFpsLabel";
            OutputFpsLabel.Size = new System.Drawing.Size(32, 15);
            OutputFpsLabel.TabIndex = 17;
            OutputFpsLabel.Text = "0 fps";
            // 
            // InputFpsLabel
            // 
            InputFpsLabel.AutoSize = true;
            InputFpsLabel.Location = new System.Drawing.Point(366, 143);
            InputFpsLabel.Name = "InputFpsLabel";
            InputFpsLabel.Size = new System.Drawing.Size(32, 15);
            InputFpsLabel.TabIndex = 16;
            InputFpsLabel.Text = "0 fps";
            // 
            // ShowDiffernceCheckbox
            // 
            ShowDiffernceCheckbox.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
            ShowDiffernceCheckbox.AutoSize = true;
            ShowDiffernceCheckbox.Location = new System.Drawing.Point(366, 161);
            ShowDiffernceCheckbox.Name = "ShowDiffernceCheckbox";
            ShowDiffernceCheckbox.Size = new System.Drawing.Size(111, 19);
            ShowDiffernceCheckbox.TabIndex = 15;
            ShowDiffernceCheckbox.Text = "Show difference";
            ShowDiffernceCheckbox.UseVisualStyleBackColor = true;
            // 
            // StartButton
            // 
            StartButton.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            StartButton.Enabled = false;
            StartButton.Font = new System.Drawing.Font("Segoe UI", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            StartButton.Location = new System.Drawing.Point(6, 22);
            StartButton.Name = "StartButton";
            StartButton.Size = new System.Drawing.Size(354, 158);
            StartButton.TabIndex = 13;
            StartButton.Text = "Start";
            StartButton.UseVisualStyleBackColor = true;
            StartButton.Click += StartButton_Click;
            // 
            // StopButton
            // 
            StopButton.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
            StopButton.Enabled = false;
            StopButton.Font = new System.Drawing.Font("Segoe UI", 15.75F, System.Drawing.FontStyle.Bold);
            StopButton.Location = new System.Drawing.Point(366, 22);
            StopButton.Name = "StopButton";
            StopButton.Size = new System.Drawing.Size(132, 118);
            StopButton.TabIndex = 14;
            StopButton.Text = "Stop";
            StopButton.UseVisualStyleBackColor = true;
            StopButton.Click += StopButton_Click;
            // 
            // groupBox2
            // 
            groupBox2.Controls.Add(FileGrid);
            groupBox2.Controls.Add(AddFilesButton);
            groupBox2.Controls.Add(MoveSelectedFileUpButton);
            groupBox2.Controls.Add(MoveSelectedFileDownButton);
            groupBox2.Controls.Add(DeleteSelectedFilesButton);
            groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            groupBox2.Location = new System.Drawing.Point(3, 3);
            groupBox2.Name = "groupBox2";
            groupBox2.Size = new System.Drawing.Size(504, 186);
            groupBox2.TabIndex = 16;
            groupBox2.TabStop = false;
            groupBox2.Text = "Select your files";
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(Rectengles);
            groupBox1.Controls.Add(AddRectangeButton);
            groupBox1.Controls.Add(DeleteSelectedRectangleButton);
            groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            groupBox1.Location = new System.Drawing.Point(3, 195);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new System.Drawing.Size(504, 186);
            groupBox1.TabIndex = 16;
            groupBox1.TabStop = false;
            groupBox1.Text = "Ignored rectangles";
            // 
            // Rectengles
            // 
            Rectengles.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            Rectengles.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            Rectengles.Location = new System.Drawing.Point(6, 22);
            Rectengles.Name = "Rectengles";
            Rectengles.Size = new System.Drawing.Size(492, 129);
            Rectengles.TabIndex = 9;
            // 
            // AddRectangeButton
            // 
            AddRectangeButton.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            AddRectangeButton.Enabled = false;
            AddRectangeButton.Location = new System.Drawing.Point(6, 157);
            AddRectangeButton.Name = "AddRectangeButton";
            AddRectangeButton.Size = new System.Drawing.Size(354, 23);
            AddRectangeButton.TabIndex = 10;
            AddRectangeButton.Text = "Add rectangle";
            AddRectangeButton.UseVisualStyleBackColor = true;
            // 
            // DeleteSelectedRectangleButton
            // 
            DeleteSelectedRectangleButton.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
            DeleteSelectedRectangleButton.Enabled = false;
            DeleteSelectedRectangleButton.Location = new System.Drawing.Point(366, 157);
            DeleteSelectedRectangleButton.Name = "DeleteSelectedRectangleButton";
            DeleteSelectedRectangleButton.Size = new System.Drawing.Size(132, 23);
            DeleteSelectedRectangleButton.TabIndex = 11;
            DeleteSelectedRectangleButton.Text = "Delete";
            DeleteSelectedRectangleButton.UseVisualStyleBackColor = true;
            // 
            // ConsoleFFMpeg
            // 
            ConsoleFFMpeg.BackColor = System.Drawing.Color.Black;
            ConsoleFFMpeg.Dock = System.Windows.Forms.DockStyle.Fill;
            ConsoleFFMpeg.ForeColor = System.Drawing.Color.White;
            ConsoleFFMpeg.Location = new System.Drawing.Point(513, 387);
            ConsoleFFMpeg.Name = "ConsoleFFMpeg";
            ConsoleFFMpeg.Size = new System.Drawing.Size(505, 186);
            ConsoleFFMpeg.TabIndex = 13;
            // 
            // displayControl1
            // 
            displayControl1.BackColor = System.Drawing.Color.Black;
            displayControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            displayControl1.Location = new System.Drawing.Point(513, 3);
            displayControl1.Name = "displayControl1";
            displayControl1.Size = new System.Drawing.Size(505, 186);
            displayControl1.TabIndex = 18;
            // 
            // Timer
            // 
            Timer.Tick += Timer_Tick;
            // 
            // ConverterForm
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(1045, 600);
            Controls.Add(tableLayoutPanel1);
            Name = "ConverterForm";
            Text = "ConverterForm";
            Load += ConverterForm_Load;
            VisibleChanged += ConverterForm_VisibleChanged;
            ((System.ComponentModel.ISupportInitialize)FileGrid).EndInit();
            tableLayoutPanel1.ResumeLayout(false);
            groupBox3.ResumeLayout(false);
            groupBox3.PerformLayout();
            groupBox2.ResumeLayout(false);
            groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)Rectengles).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Button AddFilesButton;
        private System.Windows.Forms.Button MoveSelectedFileUpButton;
        private System.Windows.Forms.Button MoveSelectedFileDownButton;
        private System.Windows.Forms.Button DeleteSelectedFilesButton;
        private System.Windows.Forms.DataGridView FileGrid;
        private Controls.ConsoleControl Console;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private Controls.ConsoleControl ConsoleFFMpeg;
        private System.Windows.Forms.DataGridView Rectengles;
        private System.Windows.Forms.Button AddRectangeButton;
        private System.Windows.Forms.Button DeleteSelectedRectangleButton;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.CheckBox ShowDiffernceCheckbox;
        private System.Windows.Forms.Button StartButton;
        private System.Windows.Forms.Button StopButton;
        private Controls.DisplayControl displayControl1;
        private System.Windows.Forms.Timer Timer;
        private System.Windows.Forms.Label InputFpsLabel;
        private System.Windows.Forms.Label OutputFpsLabel;
    }
}