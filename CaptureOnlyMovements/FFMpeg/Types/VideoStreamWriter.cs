using CaptureOnlyMovements.Enums;
using CaptureOnlyMovements.Interfaces;
using CaptureOnlyMovements.Pipeline.Interfaces;
using CaptureOnlyMovements.Types;
using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;

namespace CaptureOnlyMovements.FFMpeg.Types;

public class VideoStreamWriter : IDisposable, IFrameWriter
{
    public VideoStreamWriter(
        IKillSwitch killSwitch,
        MediaInfo mediaContainer,
        Resolution resolution,
        int fps,
        QualityEnum qualityEnum,
        PresetEnum presetEnum,
        EncoderEnum encoderEnum,
        IConsole? console = null)
    {
        Console = console;
        KillSwitch = killSwitch;
        Resolution = resolution;
        MediaContainer = mediaContainer;

        var ffMpegFullname = Path.Combine(MediaContainer.FFMpegDirectory.FullName, "ffmpeg.exe");
        var fullName = MediaContainer.FileInfo.FullName;

        if (!File.Exists(ffMpegFullname))
        {
            var error = new Exception(@$"FFmpeg executable not found at: {ffMpegFullname}

Please download FFmpeg and place it in the specified location. The program will now close. Restart the application after installing FFmpeg.");
            killSwitch.FatalException(error);
            throw error;
        }
        if (MediaContainer.FileInfo.Exists)
        {
            var error = new Exception($"Cannot write to file '{fullName}', file already exists.");
            killSwitch.FatalException(error);
            throw error;
        }

        var pixFmt = "bgr24";
        var format = "rawvideo";
        var overwrite = "-y";
        var size = $"-s {Resolution.Width}x{Resolution.Height}";
        var frameRate = $"-r {fps.ToString("F2", CultureInfo.InvariantCulture)}";
        var codec = GetCodec(encoderEnum);
        var preset = GetPreset(encoderEnum, presetEnum);
        var quality = GetQuality(encoderEnum, qualityEnum);
        var arguments = GetArguments(encoderEnum, pixFmt, format, overwrite, size, frameRate, codec, preset, quality, fullName);

        Console?.WriteLine("== FFmpeg Encoder Setup ==");
        Console?.WriteLine($"Encoder: {encoderEnum}");
        Console?.WriteLine($"Codec: {codec}");
        Console?.WriteLine($"Quality: {quality}");
        Console?.WriteLine($"Preset: {preset}");
        Console?.WriteLine($"Command: ffmpeg {arguments}");
        Console?.WriteLine("");

        Process = new Process()
        {
            StartInfo = new ProcessStartInfo()
            {
                FileName = ffMpegFullname,
                WorkingDirectory = MediaContainer.FFMpegDirectory.FullName,
                Arguments = arguments,
                RedirectStandardInput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true,
            }
        };

        if (!Process.Start()) throw new Exception("Error launching FFMPEG");

        Writer = new BinaryWriter(Process.StandardInput.BaseStream);
        Process.ErrorDataReceived += (_, e) =>
        {
            if (e.Data == null) return;
            Console?.WriteLine(e.Data);
        };
        Process.BeginErrorReadLine();
    }

    private readonly Process Process;
    private readonly IConsole? Console;
    private readonly BinaryWriter Writer;
    private readonly Resolution Resolution;
    private readonly IKillSwitch KillSwitch;
    private readonly MediaInfo MediaContainer;

    public void WriteFrame(Frame frame)
    {
        var frameData = frame.Buffer;
        if (frameData == null || frameData.Length == 0)
        {
            throw new ArgumentException("Frame data cannot be null or empty.", nameof(frameData));
        }
        if (frameData.Length != Resolution.PixelCount * 3)
        {
            throw new ArgumentException($"Frame data length {frameData.Length} does not match expected size for resolution {Resolution.Width}x{Resolution.Height}. Expected: {Resolution.Width * Resolution.Height * 3} bytes.");
        }
        if (KillSwitch.KillSwitch)
        {
            return;
        }

        try
        {
            Writer.Write(frameData);
        }
        catch (IOException ex)
        {
            if (Process.HasExited)
            {
                throw new Exception("FFMpeg process has already exited.");
            }

            KillSwitch.FatalException(ex);
        }
    }

    private static string GetCodec(EncoderEnum encoder)
       => encoder switch
       {
            EncoderEnum.NVIDIA_HEVC => "hevc_nvenc",
            EncoderEnum.NVIDIA_H264 => "h264_nvenc",

            EncoderEnum.AMD_HEVC => "hevc_amf",
            EncoderEnum.AMD_H264 => "h264_amf",

            EncoderEnum.INTEL_HEVC => "hevc_qsv",
            EncoderEnum.INTEL_H264 => "h264_qsv",

            EncoderEnum.SOFTWARE_HEVC => "libx265",
            _ => "libx264"
       };

    private static string GetQuality(EncoderEnum encoder, QualityEnum quality)
        => encoder switch
        {
            EncoderEnum.NVIDIA_H264 or EncoderEnum.NVIDIA_HEVC => quality switch
            {
                QualityEnum.Identical => "20",
                QualityEnum.High => "23",
                QualityEnum.Low => "30",
                QualityEnum.Lower => "35",
                QualityEnum.VeryLow => "40",
                _ => "26", // Medium
            },

            EncoderEnum.AMD_H264 or EncoderEnum.AMD_HEVC => quality switch
            {
                QualityEnum.Identical => "10",
                QualityEnum.High => "18",
                QualityEnum.Low => "26",
                QualityEnum.Lower => "30",
                QualityEnum.VeryLow => "34",
                _ => "22", // Medium
            },

            EncoderEnum.INTEL_H264 or EncoderEnum.INTEL_HEVC => quality switch
            {
                QualityEnum.Identical => "23",
                QualityEnum.High => "25",
                QualityEnum.Low => "30",
                QualityEnum.Lower => "33",
                QualityEnum.VeryLow => "36",
                _ => "27", // Medium
            },

            // Software
            _ => quality switch
            {
                QualityEnum.Identical => "23",
                QualityEnum.High => "25",
                QualityEnum.Low => "30",
                QualityEnum.Lower => "33",
                QualityEnum.VeryLow => "36",
                _ => "27", // Medium
            },
        };

    private static string GetPreset(EncoderEnum encoder, PresetEnum preset)
        => encoder switch
        {
            EncoderEnum.NVIDIA_H264 or EncoderEnum.NVIDIA_HEVC => preset switch
            {
                PresetEnum.VerySlow => "p7",
                PresetEnum.Slower => "p6",
                PresetEnum.Slow => "p5",
                PresetEnum.Fast => "p2",
                PresetEnum.Faster => "p1",
                PresetEnum.VeryFast => "p1",
                _ => "p4", // Medium
            },
            
            EncoderEnum.AMD_H264 or EncoderEnum.AMD_HEVC => preset switch
            {
                PresetEnum.VerySlow => "quality",
                PresetEnum.Slower => "quality",
                PresetEnum.Slow => "balanced",
                PresetEnum.Fast => "speed",
                PresetEnum.Faster => "speed",
                PresetEnum.VeryFast => "speed",
                _ => "balanced", // Medium
            },

            // Software or Intel
            _ => preset switch
            {
                PresetEnum.VerySlow => "veryslow",
                PresetEnum.Slower => "slower",
                PresetEnum.Slow => "slow",
                PresetEnum.Fast => "fast",
                PresetEnum.Faster => "faster",
                PresetEnum.VeryFast => "veryfast",
                _ => "medium", // Medium
            },
        };

    private static string GetArguments(EncoderEnum encoder, string pixFmt,
        string format, string overwrite, string size, string frameRate,
        string codec, string preset, string quality, string fullName) 
        => encoder switch
        {
            EncoderEnum.NVIDIA_HEVC or EncoderEnum.NVIDIA_H264 =>
                $"-f {format} " +
                $"-pix_fmt {pixFmt} " +
                $"{size} {frameRate} -i - " +
                $"-c:v {codec} " +
                $"-rc vbr " +
                $"-cq {quality} " +
                $"-preset {preset} " +
                $"-tune hq " +
                $"-movflags +faststart {overwrite} " +
                $"\"{fullName}\"",

            EncoderEnum.AMD_HEVC or EncoderEnum.AMD_H264 =>
                $"-f {format} " +
                $"-pix_fmt {pixFmt} " +
                $"{size} {frameRate} -i - " +
                $"-vf format=nv12 " + // converteer intern naar nv12
                $"-c:v {codec} " +
                $"-rc vbr " +
                $"-qvbr {quality} " +
                $"-quality {preset} " +
                $"-movflags +faststart {overwrite} " +
                $"\"{fullName}\"",

            EncoderEnum.INTEL_HEVC or EncoderEnum.INTEL_H264 =>
                $"-f {format} " +
                $"-pix_fmt {pixFmt} " +
                $"{size} {frameRate} -i - " +
                $"-vf format=nv12 " + // converteer intern naar nv12
                $"-c:v {codec} " +
                $"-rc icq " +
                $"-global_quality {quality} " +
                $"-preset {preset} " +
                $"-movflags +faststart {overwrite} " +
                $"\"{fullName}\"",

            // Software
            _ =>
                $"-f {format} " +
                $"-pix_fmt {pixFmt} " +
                $"{size} {frameRate} -i - " +
                $"-c:v {codec} " +
                $"-crf {quality} " +
                $"-preset {preset} " +
                $"-movflags +faststart {overwrite} " +
                $"\"{fullName}\"",
        };

    public void Dispose()
    {
        Writer.Dispose();

        if (!Process.WaitForExit(10_000))
            Process.Kill();

        Process.Dispose();

        GC.SuppressFinalize(this);
    }
}