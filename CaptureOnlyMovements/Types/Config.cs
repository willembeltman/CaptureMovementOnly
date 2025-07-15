using CaptureOnlyMovements.Delegates;
using CaptureOnlyMovements.Interfaces;

namespace CaptureOnlyMovements.Types;

public class Config : IComparerConfig, IEncoderConfig
{
    public int MaximumPixelDifferenceValue { get; set; } = 144;
    public int MaximumDifferentPixelCount { get; set; } = 450;
    public int MinPlaybackSpeed { get; set; } = 8;
    public int OutputFps { get; set; } = 60;
    public string OutputQuality { get; set; } = "identical";
    public string OutputPreset { get; set; } = "veryslow";
    public bool UseGpu { get; set; } = true;

    public event StateChangedDelegate? StateChanged;
    public void OnChangedState() => StateChanged?.Invoke();

    public static Config Load()
    {
        if (File.Exists("CaptureOnlyMovements.json"))
        {
            var json = File.ReadAllText("CaptureOnlyMovements.json");
            return System.Text.Json.JsonSerializer.Deserialize<Config>(json) ?? new Config();
        }
        else
        {
            var config = new Config();
            config.Save();
            return config;
        }
    }

    public void Save()
    {
        var json = System.Text.Json.JsonSerializer.Serialize(this);
        File.WriteAllText("CaptureOnlyMovements.json", json);
    }
}
