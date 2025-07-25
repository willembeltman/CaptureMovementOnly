﻿using CaptureOnlyMovements.Interfaces;
using CaptureOnlyMovements.Types;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;

namespace CaptureOnlyMovements.FFMpeg.Types;

public class VideoStreamReader : IDisposable, IFrameReader
{
    public VideoStreamReader(
        IKillSwitch killSwitch,
        MediaInfo mediaContainer,
        Resolution resolution,
        IConsole? console = null,
        double startTime = 0)
    {
        Console = console;
        KillSwitch = killSwitch;
        MediaContainer = mediaContainer;
        Resolution = resolution;

        var ffMpegFullname = Path.Combine(MediaContainer.FFMpegDirectory.FullName, "ffmpeg.exe");
        var startTimeSpan = TimeSpan.FromSeconds(startTime);
        var startTimeStamp = startTimeSpan.ToString(@"hh\:mm\:ss\.fff");
        var arguments = $"-i \"{MediaContainer.FileInfo.FullName}\" " +
                        $"-ss {startTimeStamp} " +
                        $"-s {Resolution.Width}x{Resolution.Height} " +
                        $"-pix_fmt bgr24 -f rawvideo -";

        var processStartInfo = new ProcessStartInfo
        {
            FileName = ffMpegFullname,
            WorkingDirectory = MediaContainer.FFMpegDirectory.FullName,
            Arguments = arguments,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };

        Process = Process.Start(processStartInfo) ?? throw new Exception("Cannot create process");
        StandardOutputReader = Process.StandardOutput.BaseStream;
        StandardErrorReader = new StreamReader(Process.StandardError.BaseStream);

        StandardErrorReaderThread = new Thread(new ThreadStart(ReadStandardError));
        StandardErrorReaderThread.Start();
    }

    private readonly Process Process;
    private readonly Stream StandardOutputReader;

    public StreamReader StandardErrorReader { get; }
    public IConsole? Console { get; }
    public IKillSwitch KillSwitch { get; }
    public MediaInfo MediaContainer { get; }
    public Resolution Resolution { get; }
    public Thread StandardErrorReaderThread { get; }

    public bool ProcessEnded { get; private set; }
    public bool ErrorReaderKillSwitch { get; private set; }

    private void ReadStandardError()
    {
        var line = StandardErrorReader.ReadLine();
        while (line != null && !KillSwitch.KillSwitch && !ErrorReaderKillSwitch)
        {
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
        if (frame == null)
            yield break;
        if (!killSwitch.KillSwitch && frame != null)
            yield return frame.Buffer;
        while (!killSwitch.KillSwitch && frame != null)
        {
            frame = ReadFrame(frame);
            if (frame == null) break;
            yield return frame.Buffer;
        }
    }

    public Frame? ReadFrame(Frame? frame = null)
    {
        var byteLength = Resolution.PixelCount * 3;
        var buffer = frame?.Buffer;
        if (buffer?.Length != byteLength)
            buffer = new byte[byteLength];

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