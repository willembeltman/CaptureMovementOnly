


namespace CaptureOnlyMovements;

public class Config
{
    public int MaximumPixelDifferenceValue { get; set; } = 144;
    public int MaximumDifferentPixelCount { get; set; } = 450;
    public int MinPlaybackSpeed { get; set; } = 8;
    public int MaxLinesInDebug { get; set; } = 100;
    public int OutputFPS { get; set; } = 60;
    public int OutputCRF { get; set; } = 23;
    public string Preset { get; set; } = "veryslow";
    public bool UseQuickSync { get; set; } = true;

    public static Config Read()
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
        var json = System.Text.Json.JsonSerializer.Serialize(this, new System.Text.Json.JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText("CaptureOnlyMovements.json", json);
    }
}
