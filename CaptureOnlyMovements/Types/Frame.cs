namespace CaptureOnlyMovements.Types;

public class Frame
{
    public Frame(byte[] buffer, Resolution resolution)
    {
        Buffer = buffer;
        Resolution = resolution;
    }

    public byte[] Buffer { get; }
    public Resolution Resolution { get; }
}
