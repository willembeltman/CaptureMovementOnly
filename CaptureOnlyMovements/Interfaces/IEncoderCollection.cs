using CaptureOnlyMovements.Enums;

namespace CaptureOnlyMovements.Interfaces;

public interface IEncoderCollection
{
    EncoderEnum[] List { get; }
}