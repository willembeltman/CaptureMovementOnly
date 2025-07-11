using CaptureOnlyMovements.Enums;
using CaptureOnlyMovements.Interfaces;
using CaptureOnlyMovements.Types;
using System.Diagnostics;
using System.Management; // Voor GPU-detectie op Windows
using System.Runtime.InteropServices; // Voor OS-detectie

namespace CaptureOnlyMovements.FFMpeg;

public class VideoStreamWriter : IDisposable
{
    public VideoStreamWriter(
        MediaContainer mediaContainer,
        Resolution resolution,
        double fps = 25,
        QualityEnum qualityEnum = QualityEnum.medium,
        PresetEnum presetEnum = PresetEnum.medium,
        bool useGpu = true)
    {
        MediaContainer = mediaContainer;

        var ffMpegFullname = Path.Combine(FFMpegDirectory.FullName, "ffmpeg.exe");

        if (!File.Exists(ffMpegFullname))
            throw new Exception($"Cannot find file '{ffMpegFullname}', please download a new copy and put it in this location.");

        if (FileInfo.Exists)
            throw new Exception($"Cannot write to file '{FileInfo.FullName}', file already exists.");

        string codec;
        string arguments;

        if (useGpu)
        {
            var gpuType = DetectGpu(); // Detecteer de GPU
            switch (gpuType)
            {
                case GpuEnum.Nvidia:
                    codec = "h264_nvenc";
                    arguments = $"-f rawvideo -pix_fmt rgb24 -s {resolution.Width}x{resolution.Height} -r {(fps.ToString("F2")).Replace(",", ".")} -i - -c:v {codec} " +
                        $"-rc vbr -cq {GetQuality(qualityEnum, gpuType)} -preset {GetPreset(presetEnum, gpuType)} \"{FileInfo.FullName}\"";
                    break;
                case GpuEnum.AMD:
                    codec = "h264_amf";
                    arguments = $"-f rawvideo -pix_fmt rgb24 -s {resolution.Width}x{resolution.Height} -r {(fps.ToString("F2")).Replace(",", ".")} -i - -c:v {codec} " +
                                $"-rc vbr -qp_i {GetQuality(qualityEnum, gpuType)} -qp_p {GetQuality(qualityEnum, gpuType)} -quality {GetPreset(presetEnum, gpuType)} \"{FileInfo.FullName}\"";
                    break;
                case GpuEnum.Intel:
                    codec = "h264_qsv";
                    arguments = $"-f rawvideo -pix_fmt rgb24 -s {resolution.Width}x{resolution.Height} -r {(fps.ToString("F2")).Replace(",", ".")} -i - -c:v {codec} " +
                        $"-global_quality {GetQuality(qualityEnum, gpuType)} -preset {GetPreset(presetEnum, gpuType)} \"{FileInfo.FullName}\"";
                    break;
                default:
                    codec = "libx264";
                    arguments = $"-f rawvideo -pix_fmt rgb24 -s {resolution.Width}x{resolution.Height} -r {(fps.ToString("F2")).Replace(",", ".")} -i - -c:v {codec} " +
                        $"-crf {GetQuality(qualityEnum, gpuType)} -preset {GetPreset(presetEnum, gpuType)} \"{FileInfo.FullName}\"";
                    break;
            }
        }
        else
        {
            codec = "libx264";
            arguments = $"-f rawvideo -pix_fmt rgb24 -s {resolution.Width}x{resolution.Height} -r {(fps.ToString("F2")).Replace(",", ".")} -i - -c:v {codec} " +
                $"-crf {GetQuality(qualityEnum, GpuEnum.None)} -preset {GetPreset(presetEnum, GpuEnum.None)} \"{FileInfo.FullName}\"";
        }

        Process = new Process()
        {
            StartInfo = new ProcessStartInfo()
            {
                FileName = ffMpegFullname,
                WorkingDirectory = FFMpegDirectory.FullName,
                Arguments = arguments,
                RedirectStandardInput = true,
                UseShellExecute = false,
                CreateNoWindow = true,
                RedirectStandardError = true,
            }
        };

        if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux) && codec == "h264_qsv")
        {
            Process.StartInfo.Environment.Add("LIBVA_DRIVER_NAME", "iHD"); // Voor Intel op Linux
            Process.StartInfo.Environment.Add("DISPLAY", ":0"); // Voor X11 indien nodig
        }

        if (!Process.Start()) throw new Exception("Error launching FFMPEG");
        Stopwatch = Stopwatch.StartNew();
        StreamWriter = new BinaryWriter(Process.StandardInput.BaseStream);
    }


    public MediaContainer MediaContainer { get; }
    public Process Process { get; }
    public Stopwatch Stopwatch { get; }
    public BinaryWriter StreamWriter { get; }
    public DirectoryInfo FFMpegDirectory => MediaContainer.FFMpegDirectory;
    public FileInfo FileInfo => MediaContainer.FileInfo;

    public void WriteFrame(byte[] frameData, IDebugWriter debugWriter)
    {
        if (Process.HasExited)
        {
            var error = Process.StandardError.ReadToEnd();
            if (Stopwatch.Elapsed.TotalSeconds > 10)
                throw new Exception($"FFMpeg shut down, maybe file already exist, or disk is full, or something else.\r\n\r\n{error}");
            else
                throw new Exception($"FFMpeg shut down, error creating file\r\n\r\n{error}");
        }
        StreamWriter.Write(frameData);
        debugWriter.WriteLine($"Captured frame at {DateTime.Now:HH:mm:ss.fff}");
    }

    public void WriteEnumerable(IEnumerable<byte[]> enumerable, IDebugWriter debugWriter)
    {
        foreach (byte[] frame in enumerable)
        {
            WriteFrame(frame, debugWriter);
        }
    }

    private GpuEnum DetectGpu()
    {
        try
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                using (var searcher = new ManagementObjectSearcher("SELECT * FROM Win32_VideoController"))
                {
                    foreach (var obj in searcher.Get())
                    {
                        string name = obj["Name"]!.ToString()!.ToLower() ?? "";
                        if (name.Contains("nvidia")) return GpuEnum.Nvidia;
                        if (name.Contains("amd") || name.Contains("radeon")) return GpuEnum.AMD;
                        if (name.Contains("intel")) return GpuEnum.Intel;
                    }
                }
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                // Voor Linux kun je `lspci` of `lscpu` gebruiken, maar dit vereist extra parsing
                // Voor simpliciteit vertrouwen we op FFmpeg's detectie of handmatige config
                var process = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = "lspci",
                        RedirectStandardOutput = true,
                        UseShellExecute = false,
                        CreateNoWindow = true
                    }
                };
                process.Start();
                string output = process.StandardOutput.ReadToEnd();
                process.WaitForExit();
                output = output.ToLower();
                if (output.Contains("nvidia")) return GpuEnum.Nvidia;
                if (output.Contains("amd") || output.Contains("radeon")) return GpuEnum.AMD;
                if (output.Contains("intel")) return GpuEnum.Intel;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"GPU detection failed: {ex.Message}");
        }
        return GpuEnum.Unknown; // Fallback naar software-encoding
    }
    private string? GetPreset(PresetEnum presetEnum, GpuEnum gpuType)
    {
        return gpuType switch
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
            _ => Enum.GetName(presetEnum),
        };
    }
    private string GetQuality(QualityEnum qualityEnum, GpuEnum gpuType)
    {
        return gpuType switch
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
            GpuEnum.AMD => qualityEnum switch
            {
                QualityEnum.identical => "10", // Adjusted from 0 to align with CRF 23 quality
                QualityEnum.high => "18",
                QualityEnum.low => "26",
                QualityEnum.lower => "30",
                QualityEnum.verylow => "34",
                _ => "22", // Medium
            },
            _ => Convert.ToInt32(qualityEnum).ToString(),
        };
    }
    public void Dispose()
    {
        Stopwatch?.Stop();
        StreamWriter?.Dispose();
        if (Process != null)
        {
            if (!Process.HasExited)
                Process.Kill();
            Process.Dispose();
        }
    }
}