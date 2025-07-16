using System;

namespace CaptureOnlyMovements.Interfaces
{
    public interface IKillSwitch
    {
        bool KillSwitch { get; }
        void FatalException(Exception exception);
    }
}