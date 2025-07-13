using CaptureOnlyMovements.Helpers;
using CaptureOnlyMovements.Interfaces;
using CaptureOnlyMovements.Types;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;

namespace CaptureOnlyMovements.Forms.SubForms
{
    public partial class ConverterForm : Form, IApplication, IPreview
    {
        public ConverterForm(IApplication application)
        {
            InitializeComponent();

            Files = [];
            Application = application;
            Converter = new Converter(this, this, null, Console1, Console2, Files);

            Load += ConverterForm_Load;
            VisibleChanged += ConverterForm_VisibleChange;
            Application.Config.StateChanged += StateChanged;
        }

        public BindingList<FileConfig> Files { get; }
        public IApplication Application { get; }
        public Converter Converter { get; }

        private void StateChanged()
        {
            if (InvokeRequired)
                Invoke(StateChanged);

            if (Converter.Converting)
            {
                DeleteSelectedFilesButton.Enabled = false;
                MoveSelectedFileDownButton.Enabled = false;
                DeleteSelectedFilesButton.Enabled = false;
                StartButton.Enabled = false;
                AddFilesButton.Enabled = false;
                StopButton.Enabled = true;
            }
            else
            {
                FileGrid_SelectionChanged(this, new EventArgs());
            }
        }
        private void FileGrid_SelectionChanged(object sender, System.EventArgs e)
        {
            if (Files.Count > 1 && FileGrid.SelectedRows.Count > 0)
            {
                MoveSelectedFileUpButton.Enabled = true;
                MoveSelectedFileDownButton.Enabled = true;
                DeleteSelectedFilesButton.Enabled = true;
            }
            else if (FileGrid.SelectedRows.Count > 0)
            {
                MoveSelectedFileUpButton.Enabled = false;
                MoveSelectedFileDownButton.Enabled = false;
                DeleteSelectedFilesButton.Enabled = true;
            }
            else
            {
                MoveSelectedFileUpButton.Enabled = false;
                MoveSelectedFileDownButton.Enabled = false;
                DeleteSelectedFilesButton.Enabled = false;
            }
            StartButton.Enabled = Files.Count > 0;

            AddFilesButton.Enabled = true;
            StopButton.Enabled = false;
        }

        private void ConverterForm_Load(object? sender, System.EventArgs e) => FileGrid.DataSource = Files;
        private void ConverterForm_VisibleChange(object? sender, EventArgs e) => Timer.Enabled = Visible;

        private void AddFilesButton_Click(object sender, System.EventArgs e)
        {
            var dialog = new OpenFileDialog()
            {
                Multiselect = true,
            };
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                foreach (var a in dialog.FileNames)
                {
                    Files.Add(new FileConfig(a));
                }
            }
        }
        private void DeleteSelectedFileButton_Click(object sender, System.EventArgs e)
        {
            if (FileGrid.SelectedRows.Count > 0)
            {
                var indexes = new int[FileGrid.SelectedRows.Count];
                for (var i = 0; i < FileGrid.SelectedRows.Count; i++)
                {
                    var row = FileGrid.SelectedRows[i];
                    if (row == null) continue;
                    var index = row.Index;
                    indexes[i] = index;
                }

                foreach (var index in indexes.OrderByDescending(a => a))
                {
                    Files.RemoveAt(index);
                }
            }
        }
        private void MoveSelectedFileUpButton_Click(object sender, System.EventArgs e)
        {
            if (Files.Count > 1 && FileGrid.SelectedRows.Count > 0)
            {
                var indexes = new List<int>();
                for (var i = 0; i < FileGrid.SelectedRows.Count; i++)
                {
                    var row = FileGrid.SelectedRows[i];
                    if (row == null) continue;

                    var index = row.Index;
                    indexes.Add(index);
                }
                indexes = [.. indexes.OrderBy(a => a)];
                if (indexes.Min() == 0) return;

                foreach (var index in indexes)
                {
                    var item = Files[index];
                    Files.RemoveAt(index);
                    Files.Insert(index - 1, item);
                }

                FileGrid.ClearSelection();
                foreach (var index in indexes)
                {
                    FileGrid.Rows[index - 1].Selected = true;
                }
            }
        }
        private void MoveSelectedFileDownButton_Click(object sender, System.EventArgs e)
        {
            if (Files.Count > 1 && FileGrid.SelectedRows.Count > 0)
            {
                var indexes = new List<int>();
                for (var i = 0; i < FileGrid.SelectedRows.Count; i++)
                {
                    var row = FileGrid.SelectedRows[i];
                    if (row == null) continue;

                    var index = row.Index;
                    indexes.Add(index);
                }
                indexes = [.. indexes.OrderBy(a => a)];
                if (indexes.Max() == Files.Count - 1) return;

                foreach (var index in indexes)
                {
                    var item = Files[index];
                    Files.RemoveAt(index);
                    Files.Insert(index + 1, item);
                }

                FileGrid.ClearSelection();
                foreach (var index in indexes)
                {
                    FileGrid.Rows[index + 1].Selected = true;
                }
            }

        }

        private void StartButton_Click(object sender, EventArgs e) => Converter.Start();
        private void StopButton_Click(object sender, EventArgs e) => Converter.Stop();
        private void Timer_Tick(object sender, EventArgs e)
        {
            InputFpsLabel.Text = InputFps.CalculateFps().ToString("F2") + " fps";
            OutputFpsLabel.Text = OutputFps.CalculateFps().ToString("F2") + " fps";
        }

        #region IApplication implementation 

        public FpsCounter InputFps { get; } = new FpsCounter();
        public FpsCounter OutputFps { get; } = new FpsCounter();

        public Config Config => Application.Config;
        public bool IsBusy => Converter.Converting;

        public void FatalException(string message, string title) => MessageBox.Show(message, title);
        public void FatalException(Exception exception) => FatalException(exception.Message, "Fatal exception");

        #endregion

        #region IPreview implementation

        public bool ShowMask => ShowDiffernceCheckbox.Checked;
        public bool ShowPreview => ShowPreviewCheckbox.Checked;

        public void SetMask(BwFrame frame) => displayControl1.SetFrame(frame);
        public void SetPreview(Frame frame) => displayControl1.SetFrame(frame);

        #endregion

        // Belangrijk: Overrides om te zorgen dat de applicatie niet sluit als het formulier gesloten wordt
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            // Voorkom dat het formulier direct sluit als de gebruiker op X klikt
            e.Cancel = true;
            this.Hide();
            base.OnFormClosing(e);
        }
    }
}
