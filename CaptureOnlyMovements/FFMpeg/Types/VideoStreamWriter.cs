using CaptureOnlyMovements.Enums;
using CaptureOnlyMovements.Helpers;
using CaptureOnlyMovements.Interfaces;
using CaptureOnlyMovements.Types;
using System.Diagnostics;
using System.Globalization;

namespace CaptureOnlyMovements.FFMpeg.Types;

public class VideoStreamWriter : IDisposable
{
    public VideoStreamWriter(
        IKillSwitch killSwitch,
        MediaContainer mediaContainer,
        Resolution resolution,
        int fps,
        QualityEnum qualityEnum,
        PresetEnum presetEnum,
        bool useGpu,
        IConsole? console = null)
    {
        Console = console;
        KillSwitch = killSwitch;
        MediaContainer = mediaContainer;
        Resolution = resolution;

        var ffMpegFullname = Path.Combine(MediaContainer.FFMpegDirectory.FullName, "ffmpeg.exe");

        if (!File.Exists(ffMpegFullname))
        {
            var error = new Exception(@$"FFmpeg executable not found at: {ffMpegFullname}

Please download FFmpeg and place it in the specified location. The program will now close. Restart the application after installing FFmpeg.");
            killSwitch.FatalException(error);
            throw error;
        }
        if (MediaContainer.FileInfo.Exists)
        {
            var error = new Exception($"Cannot write to file '{MediaContainer.FileInfo.FullName}', file already exists.");
            killSwitch.FatalException(error);
            throw error;
        }

        string codec;
        string arguments; 
        string pixFmt = "bgr24";
        string format = "rawvideo";
        string overwrite = "-y";
        string size = $"-s {resolution.Width}x{resolution.Height}";
        string frameRate = $"-r {fps.ToString("F2", CultureInfo.InvariantCulture)}";

        if (useGpu)
        {
            var gpuType = GpuDetector.DetectGpu();

            var preset = GetPreset(presetEnum, gpuType);
            var quality = GetQuality(qualityEnum, gpuType);

            Console?.WriteLine($"Using GPU encoding: {gpuType} with preset '{preset}' and quality '{quality}'");
            Console?.WriteLine($"");

            switch (gpuType)
            {
                case GpuEnum.Nvidia:
                    codec = "hevc_nvenc";
                    arguments = $"-f {format} -pix_fmt {pixFmt} {size} {frameRate} -i - " +
                                $"-c:v {codec} -rc vbr -cq {quality} " +
                                $"-preset {preset} -tune hq {overwrite} -movflags +faststart \"{MediaContainer.FileInfo.FullName}\"";
                    break;
                case GpuEnum.AMD:
                    codec = "hevc_amf";
                    string qp_i = GetQualityAmd_I(qualityEnum);
                    string qp_p = GetQualityAmd_P(qualityEnum);
                    string qp_b = GetQualityAmd_B(qualityEnum);
                    // AMD aanroep
                    arguments = $"-f {format} -pix_fmt {pixFmt} {size} {frameRate} -i - " +
                                $"-c:v {codec} -rc constqp -qp_i {qp_i} -qp_p {qp_p} -qp_b {qp_b} " +
                                $"-quality {preset} {overwrite} -movflags +faststart \"{MediaContainer.FileInfo.FullName}\"";
                    break;
                case GpuEnum.Intel:
                    codec = "hevc_qsv";
                    arguments =
                        $"-f {format} " +
                        $"-pix_fmt {pixFmt} " +                                // bgr24 input
                        $"{size} {frameRate} -i - " +
                        $"-vf format=nv12 " +                                  // converteer intern naar nv12
                        $"-c:v {codec} " +
                        $"-global_quality {quality} " +
                        $"-preset {preset} " +
                        $"-movflags +faststart {overwrite} " +
                        $"\"{MediaContainer.FileInfo.FullName}\"";
                    break;

                default:
                    codec = "libx265";
                    arguments = $"-f {format} -pix_fmt {pixFmt} {size} {frameRate} -i - " +
                                $"-c:v {codec} -crf {quality} " +
                                $"-preset {preset} {overwrite} -movflags +faststart \"{MediaContainer.FileInfo.FullName}\"";
                    break;
            }
        }
        else
        {
            codec = "libx265";
            arguments = $"-f rawvideo -pix_fmt {pixFmt} -s {resolution.Width}x{resolution.Height} -r {fps} -i - " +
                        $"-c:v {codec} -crf {GetQuality(qualityEnum, GpuEnum.None)} " +
                        $"-preset {GetPreset(presetEnum, GpuEnum.None)} -movflags +faststart {overwrite} \"{MediaContainer.FileInfo.FullName}\"";
        }

        Console?.WriteLine($"FFMpeg command: {ffMpegFullname} {arguments}");
        Console?.WriteLine($"");

        Process = new Process()
        {
            StartInfo = new ProcessStartInfo()
            {
                FileName = ffMpegFullname,
                WorkingDirectory = MediaContainer.FFMpegDirectory.FullName,
                Arguments = arguments,
                RedirectStandardInput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true,
            }
        };

        if (!Process.Start()) throw new Exception("Error launching FFMPEG");

        Writer = new BinaryWriter(Process.StandardInput.BaseStream);
        Process.ErrorDataReceived += (_, e) =>
        {
            if (e.Data == null) return;
            Console?.WriteLine(e.Data);
        };
        Process.BeginErrorReadLine();
    }

    private readonly IKillSwitch KillSwitch;
    private readonly MediaContainer MediaContainer;
    private readonly Resolution Resolution;
    private readonly IConsole? Console;
    private readonly Process Process;
    private readonly BinaryWriter Writer;

    public void WriteFrame(byte[] frameData)
    {
        if (frameData == null || frameData.Length == 0)
        {
            throw new ArgumentException("Frame data cannot be null or empty.", nameof(frameData));
        }
        if (Process.HasExited)
        {
            throw new Exception("FFMpeg process has already exited.");
        }
        if (frameData.Length != Resolution.PixelCount * 3)
        {
            throw new ArgumentException($"Frame data length {frameData.Length} does not match expected size for resolution {Resolution.Width}x{Resolution.Height}. Expected: {Resolution.Width * Resolution.Height * 3} bytes.");
        }
        if (KillSwitch.KillSwitch)
        {
            return;
        }

        try
        {
            Writer.Write(frameData);
        }
        catch (IOException ex)
        {
            KillSwitch.FatalException(ex);
        }
    }

    public static string? GetPreset(PresetEnum presetEnum, GpuEnum gpuType)
        => gpuType switch
        {
            GpuEnum.Nvidia => presetEnum switch
            {
                PresetEnum.veryslow => "p7",
                PresetEnum.slower => "p6",
                PresetEnum.slow => "p5",
                PresetEnum.fast => "p2",
                PresetEnum.faster => "p1",
                PresetEnum.veryfast => "p1",
                _ => "p4",
            },
            GpuEnum.AMD => presetEnum switch
            {
                PresetEnum.veryslow => "quality",
                PresetEnum.slower => "quality",
                PresetEnum.slow => "balanced",
                PresetEnum.fast => "speed",
                PresetEnum.faster => "speed",
                PresetEnum.veryfast => "speed",
                _ => "balanced",
            },
            GpuEnum.Intel => presetEnum switch
            {
                PresetEnum.veryslow => "veryslow",
                PresetEnum.slower => "slower",
                PresetEnum.slow => "slow",
                PresetEnum.fast => "fast",
                PresetEnum.faster => "faster",
                PresetEnum.veryfast => "veryfast",
                _ => "medium", 
            },
            _ => Enum.GetName(presetEnum),
        };
    public static string GetQuality(QualityEnum qualityEnum, GpuEnum gpuType)
        => gpuType switch
        {
            GpuEnum.Nvidia => qualityEnum switch
            {
                QualityEnum.identical => "20",
                QualityEnum.high => "23",
                QualityEnum.low => "30",
                QualityEnum.lower => "35",
                QualityEnum.verylow => "40",
                _ => "26", // Medium
            },
            GpuEnum.Intel => qualityEnum switch
            {
                QualityEnum.identical => "23",
                QualityEnum.high => "25",
                QualityEnum.low => "30",
                QualityEnum.lower => "33",
                QualityEnum.verylow => "36",
                _ => "27", // Medium
            },
            GpuEnum.None => qualityEnum switch
            {
                QualityEnum.identical => "20", // hogere visuele kwaliteit bij software
                QualityEnum.high => "24",
                QualityEnum.low => "30",
                QualityEnum.lower => "35",
                QualityEnum.verylow => "40",
                _ => "28",
            },
            _ => "28",
        };
    public static string GetQualityAmd_I(QualityEnum qualityEnum)
        => qualityEnum switch
        {
            QualityEnum.identical => "10",
            QualityEnum.high => "18",
            QualityEnum.low => "26",
            QualityEnum.lower => "30",
            QualityEnum.verylow => "34",
            _ => "22", // Medium
        };
    public static string GetQualityAmd_P(QualityEnum qualityEnum)
        => qualityEnum switch
        {
            QualityEnum.identical => "10",
            QualityEnum.high => "18",
            QualityEnum.low => "26",
            QualityEnum.lower => "30",
            QualityEnum.verylow => "34",
            _ => "22", // Medium
        };
    public static string GetQualityAmd_B(QualityEnum qualityEnum)
        => qualityEnum switch
        {
            QualityEnum.identical => "10",
            QualityEnum.high => "18",
            QualityEnum.low => "26",
            QualityEnum.lower => "30",
            QualityEnum.verylow => "34",
            _ => "22", // Medium
        };

    public void Dispose()
    {
        Writer.Flush();
        Writer.Close();

        if (!Process.WaitForExit(10_000))
            Process.Kill();

        Process.Dispose();

        Writer?.Dispose();

        GC.SuppressFinalize(this);
    }
}
