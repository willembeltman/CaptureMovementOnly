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
            AddFilesButton.Location = new System.Drawing.Point(7, 209);
            AddFilesButton.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            AddFilesButton.Name = "AddFilesButton";
            AddFilesButton.Size = new System.Drawing.Size(360, 31);
            AddFilesButton.TabIndex = 0;
            AddFilesButton.Text = "Add file(s)";
            AddFilesButton.UseVisualStyleBackColor = true;
            AddFilesButton.Click += AddFilesButton_Click;
            // 
            // MoveSelectedFileUpButton
            // 
            MoveSelectedFileUpButton.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
            MoveSelectedFileUpButton.Enabled = false;
            MoveSelectedFileUpButton.Location = new System.Drawing.Point(447, 209);
            MoveSelectedFileUpButton.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            MoveSelectedFileUpButton.Name = "MoveSelectedFileUpButton";
            MoveSelectedFileUpButton.Size = new System.Drawing.Size(58, 31);
            MoveSelectedFileUpButton.TabIndex = 4;
            MoveSelectedFileUpButton.Text = "Up";
            MoveSelectedFileUpButton.UseVisualStyleBackColor = true;
            MoveSelectedFileUpButton.Click += MoveSelectedFileUpButton_Click;
            // 
            // MoveSelectedFileDownButton
            // 
            MoveSelectedFileDownButton.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
            MoveSelectedFileDownButton.Enabled = false;
            MoveSelectedFileDownButton.Location = new System.Drawing.Point(512, 209);
            MoveSelectedFileDownButton.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            MoveSelectedFileDownButton.Name = "MoveSelectedFileDownButton";
            MoveSelectedFileDownButton.Size = new System.Drawing.Size(58, 31);
            MoveSelectedFileDownButton.TabIndex = 5;
            MoveSelectedFileDownButton.Text = "Down";
            MoveSelectedFileDownButton.UseVisualStyleBackColor = true;
            MoveSelectedFileDownButton.Click += MoveSelectedFileDownButton_Click;
            // 
            // DeleteSelectedFilesButton
            // 
            DeleteSelectedFilesButton.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
            DeleteSelectedFilesButton.Enabled = false;
            DeleteSelectedFilesButton.Location = new System.Drawing.Point(374, 209);
            DeleteSelectedFilesButton.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            DeleteSelectedFilesButton.Name = "DeleteSelectedFilesButton";
            DeleteSelectedFilesButton.Size = new System.Drawing.Size(66, 31);
            DeleteSelectedFilesButton.TabIndex = 6;
            DeleteSelectedFilesButton.Text = "Delete";
            DeleteSelectedFilesButton.UseVisualStyleBackColor = true;
            DeleteSelectedFilesButton.Click += DeleteSelectedFileButton_Click;
            // 
            // FileGrid
            // 
            FileGrid.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            FileGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            FileGrid.Location = new System.Drawing.Point(7, 29);
            FileGrid.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            FileGrid.Name = "FileGrid";
            FileGrid.RowHeadersWidth = 51;
            FileGrid.Size = new System.Drawing.Size(563, 172);
            FileGrid.TabIndex = 7;
            FileGrid.SelectionChanged += FileGrid_SelectionChanged;
            // 
            // Console
            // 
            Console.BackColor = System.Drawing.Color.Black;
            Console.Dock = System.Windows.Forms.DockStyle.Fill;
            Console.ForeColor = System.Drawing.Color.White;
            Console.Location = new System.Drawing.Point(586, 261);
            Console.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            Console.Name = "Console";
            Console.Size = new System.Drawing.Size(578, 246);
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
            tableLayoutPanel1.Location = new System.Drawing.Point(14, 16);
            tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 3;
            tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.3333321F));
            tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.3333321F));
            tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.3333321F));
            tableLayoutPanel1.Size = new System.Drawing.Size(1167, 768);
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
            groupBox3.Location = new System.Drawing.Point(3, 516);
            groupBox3.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            groupBox3.Name = "groupBox3";
            groupBox3.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
            groupBox3.Size = new System.Drawing.Size(577, 248);
            groupBox3.TabIndex = 17;
            groupBox3.TabStop = false;
            groupBox3.Text = "Start / Stop";
            // 
            // OutputFpsLabel
            // 
            OutputFpsLabel.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
            OutputFpsLabel.AutoSize = true;
            OutputFpsLabel.Location = new System.Drawing.Point(485, 191);
            OutputFpsLabel.Name = "OutputFpsLabel";
            OutputFpsLabel.Size = new System.Drawing.Size(41, 20);
            OutputFpsLabel.TabIndex = 17;
            OutputFpsLabel.Text = "0 fps";
            // 
            // InputFpsLabel
            // 
            InputFpsLabel.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
            InputFpsLabel.AutoSize = true;
            InputFpsLabel.Location = new System.Drawing.Point(418, 191);
            InputFpsLabel.Name = "InputFpsLabel";
            InputFpsLabel.Size = new System.Drawing.Size(41, 20);
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
            StartButton.Location = new System.Drawing.Point(7, 29);
            StartButton.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            StartButton.Name = "StartButton";
            StartButton.Size = new System.Drawing.Size(406, 211);
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
            StopButton.Location = new System.Drawing.Point(419, 29);
            StopButton.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            StopButton.Name = "StopButton";
            StopButton.Size = new System.Drawing.Size(151, 157);
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
            groupBox2.Location = new System.Drawing.Point(3, 4);
            groupBox2.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            groupBox2.Name = "groupBox2";
            groupBox2.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
            groupBox2.Size = new System.Drawing.Size(577, 248);
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
            groupBox1.Location = new System.Drawing.Point(3, 260);
            groupBox1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            groupBox1.Name = "groupBox1";
            groupBox1.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
            groupBox1.Size = new System.Drawing.Size(577, 248);
            groupBox1.TabIndex = 16;
            groupBox1.TabStop = false;
            groupBox1.Text = "Ignored rectangles";
            // 
            // Rectengles
            // 
            Rectengles.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            Rectengles.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            Rectengles.Location = new System.Drawing.Point(7, 29);
            Rectengles.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            Rectengles.Name = "Rectengles";
            Rectengles.RowHeadersWidth = 51;
            Rectengles.Size = new System.Drawing.Size(563, 172);
            Rectengles.TabIndex = 9;
            // 
            // AddRectangeButton
            // 
            AddRectangeButton.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            AddRectangeButton.Enabled = false;
            AddRectangeButton.Location = new System.Drawing.Point(7, 209);
            AddRectangeButton.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            AddRectangeButton.Name = "AddRectangeButton";
            AddRectangeButton.Size = new System.Drawing.Size(406, 31);
            AddRectangeButton.TabIndex = 10;
            AddRectangeButton.Text = "Add rectangle";
            AddRectangeButton.UseVisualStyleBackColor = true;
            // 
            // DeleteSelectedRectangleButton
            // 
            DeleteSelectedRectangleButton.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
            DeleteSelectedRectangleButton.Enabled = false;
            DeleteSelectedRectangleButton.Location = new System.Drawing.Point(419, 209);
            DeleteSelectedRectangleButton.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            DeleteSelectedRectangleButton.Name = "DeleteSelectedRectangleButton";
            DeleteSelectedRectangleButton.Size = new System.Drawing.Size(151, 31);
            DeleteSelectedRectangleButton.TabIndex = 11;
            DeleteSelectedRectangleButton.Text = "Delete";
            DeleteSelectedRectangleButton.UseVisualStyleBackColor = true;
            // 
            // ConsoleFFMpeg
            // 
            ConsoleFFMpeg.BackColor = System.Drawing.Color.Black;
            ConsoleFFMpeg.Dock = System.Windows.Forms.DockStyle.Fill;
            ConsoleFFMpeg.ForeColor = System.Drawing.Color.White;
            ConsoleFFMpeg.Location = new System.Drawing.Point(586, 517);
            ConsoleFFMpeg.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            ConsoleFFMpeg.Name = "ConsoleFFMpeg";
            ConsoleFFMpeg.Size = new System.Drawing.Size(578, 246);
            ConsoleFFMpeg.TabIndex = 13;
            // 
            // displayControl1
            // 
            displayControl1.BackColor = System.Drawing.Color.Black;
            displayControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            displayControl1.Location = new System.Drawing.Point(586, 4);
            displayControl1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            displayControl1.Name = "displayControl1";
            displayControl1.Size = new System.Drawing.Size(578, 248);
            displayControl1.TabIndex = 18;
            // 
            // Timer
            // 
            Timer.Tick += Timer_Tick;
            // 
            // ConverterForm
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(1194, 800);
            Controls.Add(tableLayoutPanel1);
            Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
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