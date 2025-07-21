using CaptureOnlyMovements.Enums;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Management;
using System.Runtime.InteropServices;

namespace CaptureOnlyMovements.Helpers;

public static class GpuDetector
{
    public static GpuEnum DetectGpu()
    {
        var list = ListGpus();
        if (list.Contains(GpuEnum.INTEL)) return GpuEnum.INTEL; // Beste
        if (list.Contains(GpuEnum.NVIDIA)) return GpuEnum.NVIDIA;
        if (list.Contains(GpuEnum.AMD)) return GpuEnum.AMD;
        return GpuEnum.SOFTWARE; // Fallback naar software-encoding (aller beste btw)
    }
    public static List<GpuEnum> ListGpus()
    {
        var list = new List<GpuEnum>();

        try
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                using var searcher = new ManagementObjectSearcher("SELECT * FROM Win32_VideoController");
                foreach (var obj in searcher.Get())
                {
                    string name = obj["Name"]!.ToString()!.ToLower() ?? "";
                    if (name.Contains("nvidia") && !list.Contains(GpuEnum.NVIDIA)) list.Add(GpuEnum.NVIDIA);
                    if ((name.Contains("amd") || name.Contains("radeon")) && !list.Contains(GpuEnum.AMD)) list.Add(GpuEnum.AMD);
                    if (name.Contains("intel") && !list.Contains(GpuEnum.INTEL)) list.Add(GpuEnum.INTEL);
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
                if (output.Contains("nvidia") && !list.Contains(GpuEnum.NVIDIA)) list.Add(GpuEnum.NVIDIA);
                if ((output.Contains("amd") || output.Contains("radeon")) && !list.Contains(GpuEnum.AMD)) list.Add(GpuEnum.AMD);
                if (output.Contains("intel") && !list.Contains(GpuEnum.INTEL)) list.Add(GpuEnum.INTEL);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"GPU detection failed: {ex.Message}");
        }
        return list;
    }
}
