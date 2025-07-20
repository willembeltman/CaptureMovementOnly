using CaptureOnlyMovements.Enums;
using CaptureOnlyMovements.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
            progress.Maximum = 7;
        }

        private async void DetectEncodersForm_Load(object sender, EventArgs e)
        {
            var fFMpegDirectory = new DirectoryInfo(Environment.CurrentDirectory);

            var workingEncoders = new List<EncoderEnum>
            {
                EncoderEnum.SOFTWARE_H264,
                EncoderEnum.SOFTWARE_HEVC
            };
            
            progress.Value = 1;
            Invalidate();

            if (await TestEncoder("h264_nvenc", fFMpegDirectory)) 
                workingEncoders.Add(EncoderEnum.NVIDIA_H264);
            progress.Value = 2;
            Invalidate();

            if (await TestEncoder("hevc_nvenc", fFMpegDirectory))
                workingEncoders.Add(EncoderEnum.NVIDIA_HEVC);
            progress.Value = 3;
            Invalidate();

            if (await TestEncoder("h264_amf", fFMpegDirectory)) 
                workingEncoders.Add(EncoderEnum.AMD_H264);
            progress.Value = 4;
            Invalidate();

            if (await TestEncoder("hevc_amf", fFMpegDirectory))
                workingEncoders.Add(EncoderEnum.AMD_HEVC);
            progress.Value = 5;
            Invalidate();

            if (await TestEncoder("h264_qsv", fFMpegDirectory)) 
                workingEncoders.Add(EncoderEnum.INTEL_H264);
            progress.Value = 6;
            Invalidate();

            if (await TestEncoder("hevc_qsv", fFMpegDirectory))
                workingEncoders.Add(EncoderEnum.INTEL_HEVC);
            progress.Value = 7;
            Invalidate();

            List = workingEncoders.ToArray();

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
                var args =
                    $"-hide_banner " +
                    $"-loglevel error " +
                    $"-f lavfi " +
                    $"-i testsrc=duration=1:size=1280x720:rate=30 " +
                    $"-c:v {encoderName} " +
                    $"-f null -";

                if (encoderName.Contains("qsv") || encoderName.Contains("amf"))
                {
                    args =
                        $"-hide_banner " +
                        $"-loglevel error " +
                        $"-f lavfi " +
                        $"-i testsrc=duration=1:size=1280x720:rate=30 " +
                        $"-pix_fmt nv12 " +
                        $"-vf format=nv12 " +
                        $"-c:v {encoderName} " +
                        $"-f null -";
                }

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
