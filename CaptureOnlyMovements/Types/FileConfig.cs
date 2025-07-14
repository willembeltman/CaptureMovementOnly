using CaptureOnlyMovements.FFProbe;
using CaptureOnlyMovements.Interfaces;

namespace CaptureOnlyMovements.Types;

public class FileConfig : IComparerConfig
{
    public FileConfig(string fullName, DirectoryInfo? fFProbeDirectory = null)
    {
        FFProbeDirectory = fFProbeDirectory ?? new DirectoryInfo(Environment.CurrentDirectory);
        FullName = fullName;
    }

    internal readonly DirectoryInfo FFProbeDirectory;

    public string? FullName { get; }
    public int MaximumPixelDifferenceValue { get; set; } = new Config().MaximumPixelDifferenceValue;
    public int MaximumDifferentPixelCount { get; set; } = new Config().MaximumDifferentPixelCount;
    public int MinPlaybackSpeed { get; set; } = new Config().MinPlaybackSpeed;

    private string? _FFProbeRapportFullName;
    private FFProbeRapport? _FFProbeRapport;

    internal FFProbeRapport? FFProbeRapport
    {
        get
        {
            if (FullName == null) return null;
            if (_FFProbeRapport == null || _FFProbeRapportFullName != FullName)
            {
                _FFProbeRapport = FFProbeRapport.GetRapport(FullName, FFProbeDirectory);
                _FFProbeRapportFullName = FullName;
            }
            return _FFProbeRapport;
        }
    }

    public int? Width => FFProbeRapport?.streams?.FirstOrDefault(a => a.codec_type == "video")?.width;
    public int? Height => FFProbeRapport?.streams?.FirstOrDefault(a => a.codec_type == "video")?.height;
    public double? Fps
    {
        get
        {
            var fpsString = FFProbeRapport?.streams?.FirstOrDefault(a => a.codec_type == "video")?.avg_frame_rate;
            if (fpsString == null) return null;
            var split = fpsString.Split('/');
            var baseFps = Convert.ToDouble(split[0]);
            var dividerFps = Convert.ToInt64(split[1]);
            return baseFps / dividerFps;
        }
    }
}