using CaptureOnlyMovements.Enums;
using CaptureOnlyMovements.Interfaces;
using CaptureOnlyMovements.Types;

namespace CaptureOnlyMovements.FFMpeg;

public class MediaContainer(string fullName, DirectoryInfo? ffmpegdir = null)
{
    public DirectoryInfo FFMpegDirectory { get; } = ffmpegdir ?? new DirectoryInfo(Environment.CurrentDirectory);
    public FileInfo FileInfo { get; } = new FileInfo(fullName);

    public VideoStreamWriter OpenWriter(
        IFFMpegDebugWriter debugWriter,
        IKillSwitch killSwitch,
        Resolution resolution,
        Config config)
    {
        var qualityEnum = Enum.Parse<QualityEnum>(config.OutputQuality);
        var presetEnum = Enum.Parse<PresetEnum>(config.OutputPreset);
        var fps = config.OutputFps;
        var useGpu = config.UseGpu;
        return new VideoStreamWriter(debugWriter, killSwitch, this, resolution, fps, qualityEnum, presetEnum, useGpu);
    }
}