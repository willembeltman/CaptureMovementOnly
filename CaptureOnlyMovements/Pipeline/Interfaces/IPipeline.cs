using CaptureOnlyMovements.Interfaces;

namespace CaptureOnlyMovements.Pipeline.Interfaces;

public interface IPipeline : IBasePipeline
{
    int Start(IKillSwitch? cancellationToken, int count);
}