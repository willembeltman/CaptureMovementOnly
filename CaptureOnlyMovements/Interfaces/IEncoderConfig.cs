using CaptureOnlyMovements.Enums;

namespace CaptureOnlyMovements.Interfaces;

public interface IEncoderConfig
{
    int OutputFps { get; set; }
    QualityEnum OutputQuality { get; set; }
    PresetEnum OutputPreset { get; set; }
    EncoderEnum OutputEncoder { get; set; }
}