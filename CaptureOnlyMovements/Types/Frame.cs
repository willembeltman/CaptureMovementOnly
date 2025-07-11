namespace CaptureOnlyMovements.Types;

public struct Frame
{
    public Frame(byte[] buffer, Resolution resolution)
    {
        Buffer = buffer;
        Resolution = resolution;
        CaptureDate = DateTime.Now;
    }

    public byte[] Buffer { get; }
    public Resolution Resolution { get; }
    public DateTime CaptureDate { get; }
}
