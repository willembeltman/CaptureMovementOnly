using CaptureOnlyMovements.Interfaces;

namespace CaptureOnlyMovements.Pipeline.Interfaces;

public interface IPipeline
{
    int Start(IKillSwitch? cancellationToken, int count);
}