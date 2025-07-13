namespace CaptureOnlyMovements.Interfaces;

public interface IComparerConfig
{
    int MaximumPixelDifferenceValue { get; }
    int MaximumDifferentPixelCount { get; }
    int MinPlaybackSpeed { get; set; }
}
