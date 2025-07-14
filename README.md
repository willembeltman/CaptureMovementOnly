# CaptureMovementOnly

**A timelapse screen recorder that captures only visual changes.**  
CaptureMovementOnly helps you create compact, meaningful timelapses of your development process (or any screen activity) by saving only frames where the screen actually changes.

---

## What is it?

CaptureMovementOnly is a lightweight Windows desktop application that records your screen — but only when something visually changes. This results in a clean timelapse that focuses on your activity, skipping over periods of inactivity.

Typical use case:  
- Record yourself coding or designing  
- Only changed frames are saved  
- Get a concise timelapse that shows your workflow without idle time  
- Generates **tiny files** with **low CPU usage**, ideal for long workdays

---

## Features

- 🖥️ **Live desktop recording** with change-detection
- 🎞️ **Existing video to timelapse**: process any video into a shorter version based on visual differences
- ⚡ **Highly optimized** using `unsafe` C# code and low-level memory operations
- 🎮 **DirectX 11 (via Vortice)** for fast GPU-based screen capture
- 🛠️ **FFmpeg integration** for encoding/decoding without external GUI tools

---

## Example: Developer Timelapse

Just leave the app running while you code.  
At the end of the day, you’ll have a sharp, shortened video that shows what you built — skipping over any moment where nothing changed on the screen.

---

## How it works

The application captures frames using DirectX and compares them pixel-by-pixel with the previous frame.  
If there’s a visual difference, the frame is saved. If not, it’s skipped.

This drastically reduces the number of frames stored and results in much smaller videos — perfect for creating effective programming timelapses without wasting resources.

The same approach is used to process existing video files into shorter, visually-relevant versions.

---

## Technical Details

- **Capture backend**: DirectX 11 using [Vortice.Windows](https://github.com/amerkoleci/Vortice.Windows)
- **Video encoding/decoding**: via FFmpeg and FFprobe
- **Frame comparison**: custom algorithm using per-pixel RGB difference
- **Performance**: low CPU usage

---

## Requirements

- Windows 10 or later
- [.NET 9 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/9.0)
- **FFmpeg & FFprobe** — see below

---

## Installing FFmpeg and FFprobe

CaptureMovementOnly requires both `ffmpeg.exe` and `ffprobe.exe` to be available **in the root folder of the program** (e.g. next to the `.exe` file of your project).

### Step 1: Download FFmpeg

Download the official static build from:  
👉 https://www.gyan.dev/ffmpeg/builds/

Choose the latest **release full build (Windows, static)** and extract the contents.

Copy `ffmpeg.exe` and `ffprobe.exe` into the root of your project (for example: `~\CaptureMovementOnly\`).

### Step 2: Set `Copy to Output Directory`

Make sure `ffmpeg.exe` and `ffprobe.exe` are added to your project and set to:  
> **Copy to Output Directory** → `Copy if newer`

---

## License

MIT License — free to use, modify, and distribute.

---

## Author

Made by [Willem Beltman](https://github.com/willembeltman)  
GitHub: https://github.com/willembeltman/CaptureMovementOnly

---

## Contributing

Pull requests and ideas are welcome!  
If you have suggestions or improvements, feel free to open an issue or PR.
