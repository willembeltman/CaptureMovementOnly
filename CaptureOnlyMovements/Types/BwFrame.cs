namespace CaptureOnlyMovements.Types;

public class BwFrame
{
    public BwFrame(bool[] buffer, Resolution resolution)
    {
        Buffer = buffer;
        Resolution = resolution;
    }
    public BwFrame(Resolution resolution)
    {
        Resolution = resolution;
        Buffer = new bool[resolution.PixelCount];
    }

    public bool[] Buffer { get; }
    public Resolution Resolution { get; }
}