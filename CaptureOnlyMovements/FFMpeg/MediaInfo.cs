using CaptureOnlyMovements.FFMpeg.Types;
using CaptureOnlyMovements.FFProbe;
using CaptureOnlyMovements.Interfaces;
using CaptureOnlyMovements.Types;
using System;
using System.IO;
using System.Linq;

namespace CaptureOnlyMovements.FFMpeg;

public class MediaInfo
{
    public MediaInfo(string fullName, DirectoryInfo? ffmpegdir = null, DirectoryInfo? ffprobedir = null)
    {
        FFMpegDirectory = ffmpegdir ?? new DirectoryInfo(Environment.CurrentDirectory);
        FFProbeDirectory = ffprobedir ?? new DirectoryInfo(Environment.CurrentDirectory);
        FileInfo = new FileInfo(fullName);
    }
    public MediaInfo(FileConfig fileConfig, DirectoryInfo? ffmpegdir = null, DirectoryInfo? ffprobedir = null)
    {
        FFMpegDirectory = ffmpegdir ?? new DirectoryInfo(Environment.CurrentDirectory);
        FFProbeDirectory = fileConfig.FFProbeDirectory;
        _FFProbeRapport = fileConfig.FFProbeRapport;
        FileInfo = new FileInfo(fileConfig.FullName ?? throw new Exception("File name not supplied"));
    }

    public DirectoryInfo FFMpegDirectory { get; }
    public DirectoryInfo FFProbeDirectory { get; }
    public FileInfo FileInfo { get; }

    private FFProbeRapport? _FFProbeRapport;
    public FFProbeRapport FFProbeRapport
        => _FFProbeRapport ??= FFProbeRapport.GetRapport(FileInfo.FullName, FFProbeDirectory);

    public VideoStreamReader OpenVideoReader(
        IKillSwitch killSwitch,
        IConsole? console = null)
    {
        var videoStream = FFProbeRapport.streams?
            .FirstOrDefault(a => a.codec_type == "video");
        if (videoStream == null || videoStream.width == null || videoStream.height == null)
            throw new Exception("Cannot find any video streams");

        var resolution = new Resolution(videoStream.width.Value, videoStream.height.Value);
        return new VideoStreamReader(killSwitch, this, resolution, console);
    }

    public VideoStreamWriter OpenVideoWriter(
        IKillSwitch killSwitch,
        Resolution resolution,
        IEncoderConfig config,
        IConsole? console = null)
    {
        return new VideoStreamWriter(killSwitch, this, resolution, config.OutputFps, config.OutputQuality, config.OutputPreset, config.OutputEncoder, console);
    }
}