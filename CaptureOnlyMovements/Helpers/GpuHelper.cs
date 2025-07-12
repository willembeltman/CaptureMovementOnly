using CaptureOnlyMovements.Enums;
using System.Diagnostics;
using System.Management; // Voor GPU-detectie op Windows
using System.Runtime.InteropServices; // Voor OS-detectie

namespace CaptureOnlyMovements.Helpers;

public static class GpuHelper
{
    public static GpuEnum DetectGpu()
    {
        try
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                using var searcher = new ManagementObjectSearcher("SELECT * FROM Win32_VideoController");
                foreach (var obj in searcher.Get())
                {
                    string name = obj["Name"]!.ToString()!.ToLower() ?? "";
                    if (name.Contains("nvidia")) return GpuEnum.Nvidia;
                    if (name.Contains("amd") || name.Contains("radeon")) return GpuEnum.AMD;
                    if (name.Contains("intel")) return GpuEnum.Intel;
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