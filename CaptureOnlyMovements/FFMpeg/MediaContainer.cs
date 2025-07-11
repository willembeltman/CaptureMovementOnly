using CaptureOnlyMovements.Types;

namespace CaptureOnlyMovements.FFMpeg;

public class MediaContainer
{
    public MediaContainer(DirectoryInfo ffmpegdir, string fullName)
    {
        FFMpegDirectory = ffmpegdir;
        FileInfo = new FileInfo(fullName);
    }

    public DirectoryInfo FFMpegDirectory { get; }
    public FileInfo FileInfo { get; }

    public VideoStreamWriter OpenVideoStreamWriter(
        Resolution resolution,
        double fps = 25,
        int crf = 23,
        string preset = "medium",
        bool useQuickSync = true)
        => new VideoStreamWriter(this, resolution, fps, crf, preset, useQuickSync);
}