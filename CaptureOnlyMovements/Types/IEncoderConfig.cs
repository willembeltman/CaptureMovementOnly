
using CaptureOnlyMovements.Enums;

namespace CaptureOnlyMovements;

public interface IEncoderConfig
{
    int OutputFps { get; set; }
    string OutputQuality { get; set; }
    string OutputPreset { get; set; }
    bool UseGpu { get; set; }
}