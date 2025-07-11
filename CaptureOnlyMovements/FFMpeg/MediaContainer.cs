using CaptureOnlyMovements.Enums;
using CaptureOnlyMovements.Types;

namespace CaptureOnlyMovements.FFMpeg;

public class MediaContainer
{
    public MediaContainer(string fullName, DirectoryInfo? ffmpegdir = null)
    {
        FFMpegDirectory = ffmpegdir ?? new DirectoryInfo(Environment.CurrentDirectory);
        FileInfo = new FileInfo(fullName);
    }

    public DirectoryInfo FFMpegDirectory { get; }
    public FileInfo FileInfo { get; }

    public VideoStreamWriter OpenVideoStreamWriter(
        Resolution resolution,
        double fps = 60,
        string quality = "identical",
        string preset = "veryslow",
        bool useGpu = true)
    {
        var qualityEnum = Enum.Parse<QualityEnum>(quality);
        var presetEnum = Enum.Parse<PresetEnum>(preset);
        return new VideoStreamWriter(this, resolution, fps, qualityEnum, presetEnum, useGpu);
    }
}