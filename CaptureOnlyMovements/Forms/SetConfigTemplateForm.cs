using CaptureOnlyMovements.Interfaces;
using CaptureOnlyMovements.Types;
using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace CaptureOnlyMovements.Forms
{
    public partial class SetConfigTemplateForm : Form
    {
        private readonly IApplication Application;
        private ConfigPrefixPreset NewItem = new ConfigPrefixPreset() { IsNew = true };
        private ConfigPrefixPreset[] List = [];
        private Config Config => Application.Config;

        public SetConfigTemplateForm(IApplication application)
        {
            Application = application;
            InitializeComponent();
            DialogResult = DialogResult.Cancel;
            this.FormBorderStyle = FormBorderStyle.None;

        }

        private void SetConfigTemplateForm_Load(object sender, EventArgs e)
        {
            this.BackColor = Color.FromArgb(30, 30, 30);
            this.DoubleBuffered = true;
            this.Font = new Font("Segoe UI", 9F);
            this.FormBorderStyle = FormBorderStyle.SizableToolWindow; // platte border


            NewItem = new ConfigPrefixPreset() { IsNew = true };

            if (Config.LastPrefixPreset != null)
            {
                var last = Config.LastPrefixPreset;
                var notLastOrNew = Config.PrefixPresets.Where(a => a.Equals(last) == false && a.Equals(NewItem) == false).ToArray();
                List = [Config.LastPrefixPreset, .. notLastOrNew, NewItem];
            }
            else
            {
                var notNew = Config.PrefixPresets.Where(a => a.Equals(NewItem) == false).ToArray();
                List = [NewItem, .. notNew];
            }

            PresetList.DataSource = List
                .Select(a => $"{a.OutputFileNamePrefix}")
                .ToArray();
            PresetList.SelectedIndex = 0;
        }

        private void GetFromForm(ConfigPrefixPreset value)
        {
            value.OutputFileNamePrefix = PrefixTextBox.Text;
            value.ProcessExecutebleFullName = ProcessExeTextBox.Text;
            value.WaitForProcessActive = ProcessActiveCheckbox.Checked;
        }
        private void SetToForm(ConfigPrefixPreset value)
        {
            PrefixTextBox.Text = value.OutputFileNamePrefix;
            ProcessExeTextBox.Text = value.ProcessExecutebleFullName;
            ProcessActiveCheckbox.Checked = value.WaitForProcessActive;
        }


        private void PresetList_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetToForm(List[PresetList.SelectedIndex]);
        }

        private void SelectProcessExeButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog form = new OpenFileDialog();
            form.DefaultExt = "*.exe|Executebles";
            if (form.ShowDialog() == DialogResult.OK)
            {
                ProcessExeTextBox.Text = form.FileName;
            }
        }

        private void StartButton_Click(object sender, EventArgs e)
        {
            var current = List[PresetList.SelectedIndex];
            var notCurrent = Config.PrefixPresets.Where(a => a.Equals(current) == false);
            GetFromForm(current);
            if (current.IsNew)
            {
                Config.PrefixPresets = [current, .. Config.PrefixPresets];
                current.IsNew = false;
            }
            Config.LastPrefixPreset = current;
            Config.Save();
            DialogResult = DialogResult.OK;
        }

    }
}
