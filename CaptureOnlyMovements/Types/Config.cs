using CaptureOnlyMovements.Delegates;
using CaptureOnlyMovements.Enums;
using CaptureOnlyMovements.Interfaces;
using System.IO;

namespace CaptureOnlyMovements.Types;

public class Config : IComparerConfig, IEncoderConfig
{
    public string OutputFileNamePrefix { get; set; } = "";
    public int MaximumPixelDifferenceValue { get; set; } = 144;
    public int MaximumDifferentPixelCount { get; set; } = 450;
    public int MinPlaybackSpeed { get; set; } = 5;
    public int OutputFps { get; set; } = 60;
    public QualityEnum OutputQuality { get; set; } = QualityEnum.Identical;
    public PresetEnum OutputPreset { get; set; } = PresetEnum.VerySlow;
    public EncoderEnum OutputEncoder { get; set; } = EncoderEnum.SOFTWARE_H264;

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
