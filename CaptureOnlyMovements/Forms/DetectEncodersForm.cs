using CaptureOnlyMovements.Enums;
using CaptureOnlyMovements.Helpers;
using CaptureOnlyMovements.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CaptureOnlyMovements.Forms
{
    public partial class DetectEncodersForm : Form, IEncoderCollection
    {
        public EncoderEnum[] List { get; private set; } = Array.Empty<EncoderEnum>();
        public IApplication Application { get; }

        public DetectEncodersForm(IApplication application)
        {
            Application = application;
            InitializeComponent();
            ProgressBar.Maximum = 7;
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            base.OnPaintBackground(e);

            int i = 0;
            int c = 255;
            while (i < 25)
            {
                var pen = new Pen(Color.FromArgb(255, c, c, c));

                e.Graphics.DrawRectangle(pen, new Rectangle(
                    i, i, ClientRectangle.Width - i * 2, ClientRectangle.Height - i * 2));

                c = c - 42;
                if (c < 0) c = 0;
                i++;
            }

            //e.Graphics.DrawRectangle(Pens.Gray, new Rectangle(2, 2, ClientRectangle.Width - 4, ClientRectangle.Height - 4));
            //e.Graphics.DrawRectangle(Pens.White, new Rectangle(1, 1, ClientRectangle.Width - 2, ClientRectangle.Height - 2));
        }

        private async void DetectEncodersForm_Load(object sender, EventArgs e)
        {
            Picture.Left = (ClientRectangle.Width - Picture.Width) / 2;
            StatusLabel.Left = (ClientRectangle.Width - StatusLabel.Width) / 2;
            ProgressBar.Left = (ClientRectangle.Width - ProgressBar.Width) / 2;
            TitleLabel.Left = (ClientRectangle.Width - TitleLabel.Width) / 2;

            var fFMpegDirectory = new DirectoryInfo(Environment.CurrentDirectory);

            var workingEncoders = new List<EncoderEnum>
            {
                EncoderEnum.SOFTWARE_H264,
                EncoderEnum.SOFTWARE_HEVC
            };

            ProgressBar.Value = 1;

            var gpus = GpuDetector.ListGpus();

            ProgressBar.Maximum = 2 +
                (gpus.Contains(GpuEnum.AMD) ? 2 : 0) +
                (gpus.Contains(GpuEnum.NVIDIA) ? 2 : 0) +
                (gpus.Contains(GpuEnum.INTEL) ? 2 : 0);
            ProgressBar.Value = 2;

            if (gpus.Contains(GpuEnum.AMD))
            {
                StatusLabel.Text = "Testing AMD encoders.";
                StatusLabel.Left = (ClientRectangle.Width - StatusLabel.Width) / 2;

                if (await TestEncoder("h264_amf", fFMpegDirectory))
                    workingEncoders.Add(EncoderEnum.AMD_H264);
                ProgressBar.Value++;

                if (await TestEncoder("hevc_amf", fFMpegDirectory))
                    workingEncoders.Add(EncoderEnum.AMD_HEVC);
                ProgressBar.Value++;
            }

            if (gpus.Contains(GpuEnum.NVIDIA))
            {
                StatusLabel.Text = "Testing NVIDIA encoders.";
                StatusLabel.Left = (ClientRectangle.Width - StatusLabel.Width) / 2;

                if (await TestEncoder("h264_nvenc", fFMpegDirectory))
                    workingEncoders.Add(EncoderEnum.NVIDIA_H264);
                ProgressBar.Value++;

                if (await TestEncoder("hevc_nvenc", fFMpegDirectory))
                    workingEncoders.Add(EncoderEnum.NVIDIA_HEVC);
                ProgressBar.Value++;
            }

            if (gpus.Contains(GpuEnum.INTEL))
            {
                StatusLabel.Text = "Testing INTEL encoders.";
                StatusLabel.Left = (ClientRectangle.Width - StatusLabel.Width) / 2;

                if (await TestEncoder("h264_qsv", fFMpegDirectory))
                    workingEncoders.Add(EncoderEnum.INTEL_H264);
                ProgressBar.Value++;

                if (await TestEncoder("hevc_qsv", fFMpegDirectory))
                    workingEncoders.Add(EncoderEnum.INTEL_HEVC);
                ProgressBar.Value++;
            }

            StatusLabel.Text = "All done, have a nice day!";
            StatusLabel.Left = (ClientRectangle.Width - StatusLabel.Width) / 2;

            List = workingEncoders.ToArray();

            await Task.Delay(2000);

            var oldencoder = Application.Config.OutputEncoder;
            if (!List.Contains(oldencoder))
            {
                Application.Config.OutputEncoder = List.Last();
                Application.Config.Save();

                MessageBox.Show(
                    $"The selected encoder '{oldencoder}' is no longer available on this computer. It may have been removed or the hardware configuration has changed.\n\n" +
                    $"The encoder has been temporarily set to '{Application.Config.OutputEncoder}'.\n\n" +
                    "Please open the settings window to review and select an available encoder.",
                    "Warning: Encoder Unavailable",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );
            }

            this.Hide();
        }

        private static async Task<bool> TestEncoder(string encoderName, DirectoryInfo fFMpegDirectory)
        {
            try
            {
                //var args =
                //    $"-hide_banner " +
                //    $"-loglevel error " +
                //    $"-f lavfi " +
                //    $"-i testsrc=duration=1:size=1280x720:rate=30 " +
                //    $"-c:v {encoderName} " +
                //    $"-f null -";

                //if (encoderName.Contains("qsv") || encoderName.Contains("amf"))
                //{
                var args =
                    $"-hide_banner " +
                    $"-loglevel error " +
                    $"-f lavfi " +
                    $"-i testsrc=duration=1:size=1280x720:rate=30 " +
                    $"-pix_fmt nv12 " +
                    $"-vf format=nv12 " +
                    $"-c:v {encoderName} " +
                    $"-f null -";
                //}

                var process = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = "ffmpeg",
                        Arguments = args,
                        RedirectStandardError = true,
                        UseShellExecute = false,
                        CreateNoWindow = true
                    }
                };

                process.Start();

                string stderr = await process.StandardError.ReadToEndAsync();
                await process.WaitForExitAsync();
                return process.ExitCode == 0;
            }
            catch
            {
                return false;
            }
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            // Voorkom dat het formulier direct sluit als de gebruiker op X klikt
            e.Cancel = true;
            this.Hide();
            base.OnFormClosing(e);
        }
    }
}
