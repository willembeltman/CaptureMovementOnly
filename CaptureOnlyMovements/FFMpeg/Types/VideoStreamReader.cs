using CaptureOnlyMovements.Interfaces;
using CaptureOnlyMovements.Types;
using System.Diagnostics;

namespace CaptureOnlyMovements.FFMpeg.Types;

public class VideoStreamReader : IDisposable
{
    public VideoStreamReader(
        IKillSwitch killSwitch,
        MediaContainer mediaContainer,
        Resolution resolution,
        IConsole? console = null,
        double startTime = 0)
    {
        Console = console;
        KillSwitch = killSwitch;
        MediaContainer = mediaContainer;
        Resolution = resolution;

        var ffMpegFullname = Path.Combine(FFMpegDirectory.FullName, "ffmpeg.exe");
        var startTimeSpan = TimeSpan.FromSeconds(startTime);
        var startTimeStamp = startTimeSpan.ToString(@"hh\:mm\:ss\.fff");
        var arguments = $"-i \"{FileInfo.FullName}\" " +
                        $"-ss {startTimeStamp} " +
                        $"-s {Resolution.Width}x{Resolution.Height} " +
                        $"-pix_fmt bgr24 -f rawvideo -";

        var processStartInfo = new ProcessStartInfo
        {
            FileName = ffMpegFullname,
            WorkingDirectory = FFMpegDirectory.FullName,
            Arguments = arguments,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };

        Process = Process.Start(processStartInfo) ?? throw new Exception("Cannot create process");
        StandardOutputReader = Process.StandardOutput.BaseStream;
        StandardErrorReader = new StreamReader(Process.StandardError.BaseStream);
        Stopwatch = Stopwatch.StartNew();

        StandardErrorReaderThread = new Thread(new ThreadStart(ReadStandardError));
        StandardErrorReaderThread.Start();
    }

    private readonly Process Process;

    public Stopwatch Stopwatch { get; }
    public Thread StandardErrorReaderThread { get; }

    private readonly Stream StandardOutputReader;

    public StreamReader StandardErrorReader { get; }
    public IConsole? Console { get; }
    public IKillSwitch KillSwitch { get; }
    public MediaContainer MediaContainer { get; }
    public Resolution Resolution { get; }

    private DirectoryInfo FFMpegDirectory => MediaContainer.FFMpegDirectory;
    private FileInfo FileInfo => MediaContainer.FileInfo;

    public bool ProcessEnded { get; private set; }
    //public string ErrorMessage { get; private set; }
    public bool ErrorReaderKillSwitch { get; private set; }

    private void ReadStandardError()
    {
        var line = StandardErrorReader.ReadLine();
        while (line != null && !KillSwitch.KillSwitch && !ErrorReaderKillSwitch)
        {
            //ErrorMessage += line + "\r\n"; // Dit kan omdat als het niet goed gaat, laat ffmpeg de error zien.
            Console?.WriteLine(line);

            line = StandardErrorReader.ReadLine();
        }
        if (line != null)
        {
            if (!Process.HasExited)
                Process.Kill();
        }
        ProcessEnded = true;
    }

    public IEnumerable<byte[]> ReadEnumerable(IKillSwitch killSwitch)
    {
        var frame = ReadFrame();
        if (!killSwitch.KillSwitch && frame != null)
            yield return frame.Buffer;
        while (!killSwitch.KillSwitch && frame != null)
        {
            frame = ReadFrame(frame.Buffer);
            if (frame == null) break;
            yield return frame.Buffer;
        }
    }

    public Frame? ReadFrame(byte[]? buffer = null)
    {
        var byteLength = Resolution.PixelCount * 3;
        buffer ??= new byte[byteLength];

        var endOfVideo = false;
        var read = 0;
        while (read < byteLength && !endOfVideo)
        {
            var partialread = StandardOutputReader.Read(buffer, read, byteLength - read);
            read += partialread;
            endOfVideo = partialread <= 0;
        }

        if (endOfVideo)
        {
            return null;
        }

        return new Frame(buffer, Resolution);
    }

    public void Dispose()
    {
        Stopwatch?.Stop();

        ErrorReaderKillSwitch = true;
        if (StandardErrorReaderThread != null && Thread.CurrentThread != StandardErrorReaderThread && !ProcessEnded)
        {
            StandardErrorReaderThread.Join();
        }

        if (!Process.HasExited)
            Process.Kill();
        Process.Dispose();

        StandardOutputReader?.Dispose();
        StandardErrorReader?.Dispose();
        GC.SuppressFinalize(this);
    }
}