using CaptureOnlyMovements.FFMpeg;

namespace CaptureOnlyMovements.Types;

public class FileItemMediaContainer
{
    public FileItemMediaContainer(FileConfig fileConfig)
    {
        FileConfig = fileConfig;
        MediaContainer = new MediaContainer(fileConfig.FullName!);
    }

    public FileConfig FileConfig { get; set; }
    public MediaContainer MediaContainer { get; set; }
}