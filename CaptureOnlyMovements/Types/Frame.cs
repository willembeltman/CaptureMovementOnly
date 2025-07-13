namespace CaptureOnlyMovements.Types;

public class Frame
{
    public Frame(byte[] buffer, Resolution resolution)
    {
        Buffer = buffer;
        Resolution = resolution;
    }
    public Frame(Resolution resolution)
    {
        Resolution = resolution;
        Buffer = new byte[resolution.ByteLength];
    }

    public byte[] Buffer { get; }
    public Resolution Resolution { get; }
}
