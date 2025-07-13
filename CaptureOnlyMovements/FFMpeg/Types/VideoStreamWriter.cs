using CaptureOnlyMovements.Enums;
using CaptureOnlyMovements.Helpers;
using CaptureOnlyMovements.Interfaces;
using CaptureOnlyMovements.Types;
using System.Diagnostics;
using System.Runtime.InteropServices; // Voor OS-detectie

namespace CaptureOnlyMovements.FFMpeg.Types;

public class VideoStreamWriter : IDisposable
{
    public VideoStreamWriter(
        IFFMpegDebugWriter debugWriter,
        IKillSwitch killSwitch,
        MediaContainer mediaContainer,
        Resolution resolution,
        int fps,
        QualityEnum qualityEnum,
        PresetEnum presetEnum,
        bool useGpu)
    {
        DebugWriter = debugWriter;
        KillSwitch = killSwitch;
        MediaContainer = mediaContainer;

        var ffMpegFullname = Path.Combine(FFMpegDirectory.FullName, "ffmpeg.exe");

        if (!File.Exists(ffMpegFullname))
        {
            var error = new Exception(@$"FFmpeg executable not found at: {ffMpegFullname}

Please download FFmpeg and place it in the specified location. The program will now close. Restart the application after installing FFmpeg.");
            killSwitch.FatalException(error);
            throw error;
        }
        if (FileInfo.Exists)
        {
            var error = new Exception($"Cannot write to file '{FileInfo.FullName}', file already exists.");
            killSwitch.FatalException(error);
            throw error;
        }

        string codec;
        string arguments;

        if (useGpu)
        {
            var gpuType = GpuHelper.DetectGpu(); // Detecteer de GPU
            switch (gpuType)
            {
                case GpuEnum.Nvidia:
                    codec = "h264_nvenc";
                    arguments = $"-f rawvideo -pix_fmt rgb24 -s {resolution.Width}x{resolution.Height} -r {fps} -i - -c:v {codec} " +
                        $"-rc vbr -cq {GpuHelper.GetQuality(qualityEnum, gpuType)} -preset {GpuHelper.GetPreset(presetEnum, gpuType)} \"{FileInfo.FullName}\"";
                    break;
                case GpuEnum.AMD:
                    codec = "h264_amf";
                    arguments = $"-f rawvideo -pix_fmt rgb24 -s {resolution.Width}x{resolution.Height} -r {fps} -i - -c:v {codec} " +
                                $"-rc vbr -qp_i {GpuHelper.GetQuality(qualityEnum, gpuType)} -qp_p {GpuHelper.GetQuality(qualityEnum, gpuType)} -quality {GpuHelper.GetPreset(presetEnum, gpuType)} \"{FileInfo.FullName}\"";
                    break;
                case GpuEnum.Intel:
                    //arguments = $"-f rawvideo -pix_fmt rgb24 -s {resolution.Width}x{resolution.Height} -r {fps} -i - -c:v h264_qsv -global_quality {CRF} -preset {Preset} \"{FileInfo.FullName}\"",
                    codec = "h264_qsv";
                    arguments = $"-f rawvideo -pix_fmt rgb24 -s {resolution.Width}x{resolution.Height} -r {fps} -i - -c:v {codec} " +
                        $"-global_quality {GpuHelper.GetQuality(qualityEnum, gpuType)} -preset {GpuHelper.GetPreset(presetEnum, gpuType)} \"{FileInfo.FullName}\"";
                    break;
                default:
                    //arguments = $"-f rawvideo -pix_fmt rgb24 -s {resolution.Width}x{resolution.Height} -r {fps} -i - -c:v libx265 -crf {CRF} -preset {Preset} \"{FileInfo.FullName}\"",
                    codec = "libx265";
                    arguments = $"-f rawvideo -pix_fmt rgb24 -s {resolution.Width}x{resolution.Height} -r {fps} -i - -c:v {codec} " +
                        $"-crf {GpuHelper.GetQuality(qualityEnum, gpuType)} -preset {GpuHelper.GetPreset(presetEnum, gpuType)} \"{FileInfo.FullName}\"";
                    break;
            }
        }
        else
        {
            codec = "libx264";
            arguments = $"-f rawvideo -pix_fmt rgb24 -s {resolution.Width}x{resolution.Height} -r {fps} -i - -c:v {codec} " +
                $"-crf {GpuHelper.GetQuality(qualityEnum, GpuEnum.None)} -preset {GpuHelper.GetPreset(presetEnum, GpuEnum.None)} \"{FileInfo.FullName}\"";
        }

        Process = new Process()
        {
            StartInfo = new ProcessStartInfo()
            {
                FileName = ffMpegFullname,
                WorkingDirectory = FFMpegDirectory.FullName,
                Arguments = arguments,
                RedirectStandardInput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true,
            }
        };

        if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux) && codec == "h264_qsv")
        {
            Process.StartInfo.Environment.Add("LIBVA_DRIVER_NAME", "iHD"); // Voor Intel op Linux
            Process.StartInfo.Environment.Add("DISPLAY", ":0"); // Voor X11 indien nodig
        }

        if (!Process.Start()) throw new Exception("Error launching FFMPEG");
        Stopwatch = Stopwatch.StartNew();
        StandardInputWriter = new BinaryWriter(Process.StandardInput.BaseStream);
        StandardErrorReader = new StreamReader(Process.StandardError.BaseStream);

        StandardErrorReaderThread = new Thread(new ThreadStart(ReadStandardError));
        StandardErrorReaderThread.Start();
    }

    public MediaContainer MediaContainer { get; }

    private readonly IFFMpegDebugWriter? DebugWriter;
    private readonly IKillSwitch KillSwitch;
    private bool ErrorReaderKillSwitch;
    private readonly Process Process;
    private readonly Stopwatch Stopwatch;
    private readonly BinaryWriter StandardInputWriter;
    private readonly StreamReader StandardErrorReader;
    private readonly Thread StandardErrorReaderThread;
    private string ErrorMessage = "";
    private bool ProcessEnded;

    private DirectoryInfo FFMpegDirectory => MediaContainer.FFMpegDirectory;
    private FileInfo FileInfo => MediaContainer.FileInfo;

    private void ReadStandardError()
    {
        var line = StandardErrorReader.ReadLine();
        while (line != null && !KillSwitch.KillSwitch && !ErrorReaderKillSwitch)
        {
            ErrorMessage += line + "\r\n"; // Dit kan omdat als het niet goed gaat, laat ffmpeg de error zien.
            //DebugWriter?.FFMpegDebugWriteLine(line);

            line = StandardErrorReader.ReadLine();
        }
        if (line != null)
        {
            if (!Process.HasExited)
                Process.Kill();
        }
        ProcessEnded = true;
    }

    public void WriteFrame(byte[] frameData)
    {
        if (ProcessEnded)
        {
            if (Stopwatch.Elapsed.TotalSeconds > 10)
                throw new Exception($"FFMpeg shut down, maybe file already exist, or disk is full, or something else.\r\n\r\n{ErrorMessage}");
            else
                throw new Exception($"FFMpeg shut down, error creating file\r\n\r\n{ErrorMessage}");
        }
        if (KillSwitch.KillSwitch)
        {
            return;
        }
        StandardInputWriter.Write(frameData);
    }

    public void Dispose()
    {
        Stopwatch?.Stop();

        ErrorReaderKillSwitch = true;
        if (StandardErrorReaderThread != null && Thread.CurrentThread != StandardErrorReaderThread && !ProcessEnded)
        {
            StandardErrorReaderThread.Join();
        }

        if (!Process.HasExited)
            Process.Kill();
        Process.Dispose();

        StandardInputWriter?.Dispose();
        StandardErrorReader?.Dispose();
        GC.SuppressFinalize(this);
    }
}
