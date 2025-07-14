using CaptureOnlyMovements.Enums;
using CaptureOnlyMovements.FFMpeg.Types;
using CaptureOnlyMovements.FFProbe;
using CaptureOnlyMovements.Interfaces;
using CaptureOnlyMovements.Types;

namespace CaptureOnlyMovements.FFMpeg;

public class MediaContainerInfo(string fullName, DirectoryInfo? ffmpegdir = null, DirectoryInfo? ffprobedir = null)
{
    public DirectoryInfo FFMpegDirectory { get; } = ffmpegdir ?? new DirectoryInfo(Environment.CurrentDirectory);
    public DirectoryInfo FFProbeDirectory { get; } = ffprobedir ?? new DirectoryInfo(Environment.CurrentDirectory);
    public FileInfo FileInfo { get; } = new FileInfo(fullName);

    private FFProbeRapport? _FFProbeRapport;
    public FFProbeRapport FFProbeRapport => _FFProbeRapport ??= FFProbeRapport.GetRapport(FileInfo.FullName, FFProbeDirectory);

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
        var qualityEnum = Enum.Parse<QualityEnum>(config.OutputQuality);
        var presetEnum = Enum.Parse<PresetEnum>(config.OutputPreset);
        var fps = config.OutputFps;
        var useGpu = config.UseGpu;
        return new VideoStreamWriter(killSwitch, this, resolution, fps, qualityEnum, presetEnum, useGpu, console);
    }
}