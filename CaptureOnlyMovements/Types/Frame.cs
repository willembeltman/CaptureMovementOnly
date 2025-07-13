namespace CaptureOnlyMovements.Types;

public class Frame
{
    public Frame(byte[] buffer, Resolution resolution)
    {
        Buffer = buffer;
        Resolution = resolution;
    }
    public Frame(Resolution resolution, int bytesPerPixel = 3)
    {
        Resolution = resolution;
        Buffer = new byte[resolution.PixelCount * bytesPerPixel];
    }

    public byte[] Buffer { get; }
    public Resolution Resolution { get; }
}
