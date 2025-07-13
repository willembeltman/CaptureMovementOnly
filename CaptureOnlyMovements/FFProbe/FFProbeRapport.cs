using CaptureOnlyMovements.FFProbe.Types;
using System.Diagnostics;
using System.Text.Json;

namespace CaptureOnlyMovements.FFProbe;

#pragma warning disable IDE1006 // Naming Styles

public class FFProbeRapport
{
    public List<FFProbeStream>? streams { get; set; }
    public FFProbeFormat? format { get; set; }

    public static FFProbeRapport GetRapport(string fullName, DirectoryInfo ffprobeDir)
    {
        var ffprobeInfo = new FileInfo(Path.Combine(ffprobeDir.FullName, "ffprobe.exe"));
        if (!ffprobeInfo.Exists)
            throw new Exception($"Cannot find ffprobe executeble on {ffprobeInfo.FullName}");

        var arguments = $" -v error -show_format -show_streams -print_format json \"{fullName}\"";

        var process = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = ffprobeInfo.FullName,
                WorkingDirectory = ffprobeDir?.FullName,
                Arguments = arguments,
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true
            }
        };

        process.Start();
        string json = process.StandardOutput.ReadToEnd();
        process.WaitForExit();

        var rapport = JsonSerializer.Deserialize<FFProbeRapport>(json);
        return rapport ?? throw new Exception($"Error getting rapport");
    }
}

#pragma warning restore IDE1006 // Naming Styles