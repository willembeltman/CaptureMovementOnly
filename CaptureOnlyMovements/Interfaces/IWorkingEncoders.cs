using CaptureOnlyMovements.Enums;

namespace CaptureOnlyMovements.Interfaces;

public interface IWorkingEncoders
{
    EncoderEnum[] List { get; }
}