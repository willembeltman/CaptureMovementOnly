using CaptureOnlyMovements.Enums;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Management;
using System.Runtime.InteropServices;

namespace CaptureOnlyMovements.Helpers;

public static class GpuDetector
{
    public static EncoderEnum DetectGpu()
    {
        var list = ListEncoders();

        if (list.Contains(EncoderEnum.INTEL_HEVC)) return EncoderEnum.INTEL_HEVC;
        if (list.Contains(EncoderEnum.NVIDIA_HEVC)) return EncoderEnum.NVIDIA_HEVC;
        if (list.Contains(EncoderEnum.AMD_HEVC)) return EncoderEnum.AMD_HEVC;
        return EncoderEnum.SOFTWARE_HEVC; // Fallback naar software-encoding
    }
    public static List<EncoderEnum> ListEncoders()
    {
        bool hasNvidia = false;
        bool hasAmd = false;
        bool hasIntel = false;

        try
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                using var searcher = new ManagementObjectSearcher("SELECT * FROM Win32_VideoController");
                foreach (var obj in searcher.Get())
                {
                    string name = obj["Name"]!.ToString()!.ToLower() ?? "";
                    if (name.Contains("nvidia")) hasNvidia = true;
                    if (name.Contains("amd") || name.Contains("radeon")) hasAmd = true;
                    if (name.Contains("intel")) hasIntel = true;
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
                if (output.Contains("nvidia")) hasNvidia = true;
                if (output.Contains("amd") || output.Contains("radeon")) hasAmd = true;
                if (output.Contains("intel")) hasIntel = true;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"GPU detection failed: {ex.Message}");
        }

        var list = new List<EncoderEnum>();
        if (hasNvidia)
        {
            list.Add(EncoderEnum.NVIDIA_H264);
            list.Add(EncoderEnum.NVIDIA_HEVC);
        }
        if (hasAmd)
        {
            list.Add(EncoderEnum.AMD_H264);
            list.Add(EncoderEnum.AMD_HEVC);
        }
        if (hasIntel)
        {
            list.Add(EncoderEnum.INTEL_H264);
            list.Add(EncoderEnum.INTEL_HEVC);
        }

        return list;
    }
}
