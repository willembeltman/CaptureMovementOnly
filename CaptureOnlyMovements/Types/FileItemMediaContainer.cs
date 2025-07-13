using CaptureOnlyMovements.FFMpeg;

namespace CaptureOnlyMovements.Types;

public class FileItemMediaContainer(FileConfig fileConfig)
{
    public FileConfig FileConfig { get; set; } = fileConfig;
    public MediaContainer MediaContainer { get; set; } = new MediaContainer(fileConfig.FullName!);
}