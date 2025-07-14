using CaptureOnlyMovements.FFMpeg;

namespace CaptureOnlyMovements.Types;

public class FileConfigInfo(FileConfig fileConfig)
{
    public FileConfig FileConfig { get; set; } = fileConfig;
    public MediaContainerInfo MediaContainer { get; set; } = new MediaContainerInfo(fileConfig.FullName!);
}