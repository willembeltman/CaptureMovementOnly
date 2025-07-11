using CaptureOnlyMovements.Types;
using System.Diagnostics;

namespace CaptureOnlyMovements.FFMpeg;

public class VideoStreamWriter : IDisposable
{
    public VideoStreamWriter(
        MediaContainer mediaContainer,
        Resolution resolution,
        double fps = 25, 
        int crf = 23, 
        string preset = "slow",
        bool useQuickSync = true)
    {
        MediaContainer = mediaContainer;
        Resolution = resolution;
        Framerate = fps;
        CRF = crf;
        Preset = preset;

        if (FileInfo.Exists) 
            throw new Exception($"Cannot write to file '{FileInfo.FullName}', file already exists.");

        if (useQuickSync)
        {
            Process = new Process()
            {
                StartInfo = new ProcessStartInfo()
                {
                    FileName = Path.Combine(FFMpegDirectory.FullName, "ffmpeg.exe"),
                    WorkingDirectory = FFMpegDirectory.FullName,
                    Arguments = $"-f rawvideo -pix_fmt rgb24 -s {resolution.Width}x{resolution.Height} -r {(Framerate.ToString("F2")).Replace(",", ".")} -i - -c:v h264_qsv -global_quality {CRF} -preset {Preset} \"{FileInfo.FullName}\"",
                    RedirectStandardInput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true,
                    Environment =
                    {
                        { "LIBVA_DRIVER_NAME", "iHD" }, // Specify the Intel GPU driver
                        { "DISPLAY", ":0" } // Specify the display number for X11 if applicable
                    }
                }
            };
        }
        else
        {
            Process = new Process()
            {
                StartInfo = new ProcessStartInfo()
                {
                    FileName = Path.Combine(FFMpegDirectory.FullName, "ffmpeg.exe"),
                    WorkingDirectory = FFMpegDirectory.FullName,
                    Arguments = $"-f rawvideo -pix_fmt rgb24 -s {resolution.Width}x{resolution.Height} -r {Framerate} -i - -c:v libx265 -crf {CRF} -preset {Preset} \"{FileInfo.FullName}\"",
                    RedirectStandardInput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                }
            };
        }

        if (!Process.Start()) throw new Exception("Error launching FFMPEG");

        Stopwatch = Stopwatch.StartNew();
        StreamWriter = new BinaryWriter(Process.StandardInput.BaseStream);
    }

    public MediaContainer MediaContainer { get; }
    public Resolution Resolution { get; }
    public double Framerate { get; }
    public int CRF { get; }
    public string Preset { get; }
    public Process Process { get; }
    public Stopwatch Stopwatch { get; }
    public BinaryWriter StreamWriter { get; }

    public int FrameCounter { get; private set; }

    public DirectoryInfo FFMpegDirectory => MediaContainer.FFMpegDirectory;
    public FileInfo FileInfo => MediaContainer.FileInfo;

    public void WriteFrame(byte[] frameData)
    {
        if (Process.HasExited)
        {
            if (Stopwatch.Elapsed.TotalSeconds > 10)
                throw new Exception("FFMpeg shut down, maybe file already exist, or disk is full, or something else.");
            else
                throw new Exception("FFMpeg shut down, error creating file");
        }
        StreamWriter.Write(frameData);
        FrameCounter++;
    }

    public void Dispose()
    {
        StreamWriter?.Dispose();
        Process?.Dispose();
    }
}
