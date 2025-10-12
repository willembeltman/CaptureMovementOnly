using CaptureOnlyMovements.Interfaces;
using CaptureOnlyMovements.Types;
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;

namespace CaptureOnlyMovements.Pipeline.Tasks
{

    public class WaitTillVisualStudioHasForcus
    {
        public WaitTillVisualStudioHasForcus(Config config, IKillSwitch killSwitch)
        {
            Config = config ?? throw new ArgumentNullException(nameof(config));
            KillSwitch = killSwitch ?? throw new ArgumentNullException(nameof(killSwitch));
        }

        public Config Config { get; }
        public IKillSwitch KillSwitch { get; }

        [DllImport("user32.dll")]
        private static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll")]
        private static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint processId);

        /// <summary>
        /// Blocks the executing thread till Visual Studio has focus.
        /// </summary>
        internal void Wait()
        {
            if (!Config.WaitForProcess) 
                return;

            try
            {
                var targetExe = Config.ProcessExecutebleFullName?.ToLowerInvariant();
                if (string.IsNullOrWhiteSpace(targetExe))
                    throw new InvalidOperationException("Config.ProcessExecutebleFullName is not set.");

                while (!KillSwitch.KillSwitch)
                {
                    IntPtr foregroundWindow = GetForegroundWindow();
                    if (foregroundWindow != IntPtr.Zero)
                    {
                        GetWindowThreadProcessId(foregroundWindow, out uint processId);

                        try
                        {
                            using (var process = Process.GetProcessById((int)processId))
                            {
                                string exePath = process.MainModule?.FileName?.ToLowerInvariant();
                                if (exePath == targetExe)
                                    return; // Visual Studio heeft focus
                            }
                        }
                        catch
                        {
                            // Proces kan net afgesloten zijn; negeren.
                        }
                    }

                    Thread.Sleep(250); // CPU vriendelijk wachten
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[WaitTillVisualStudioHasForcus] Exception: {ex}");
            }
        }
    }

}