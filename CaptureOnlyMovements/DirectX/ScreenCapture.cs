﻿using CaptureOnlyMovements.FrameConverters;
using CaptureOnlyMovements.Interfaces;
using CaptureOnlyMovements.Types;
using System;
using System.Collections.Generic;
using System.Threading;
using Vortice.Direct3D;
using Vortice.Direct3D11;
using Vortice.DXGI;

namespace CaptureOnlyMovements.DirectX;

public class ScreenshotCapturer : IDisposable
{
    private readonly IConsole Console;
    private readonly IDXGIFactory1 Factory;
    private readonly IDXGIAdapter1 Adapter;
    private readonly IDXGIOutput1 Output;
    private readonly ID3D11Device Device;
    private readonly IDXGIOutputDuplication DuplicatedOutput;
    private readonly BgraToBgrConverterUnsafe BgraToBgr;

    public ScreenshotCapturer(IConsole console)
    {
        Console = console;
        Factory = DXGI.CreateDXGIFactory1<IDXGIFactory1>();
        BgraToBgr = new BgraToBgrConverterUnsafe();

        var adapterResult = Factory.EnumAdapters1(0, out Adapter);
        if (!adapterResult.Success || Adapter == null)
        {
            throw new InvalidOperationException("Failed to retrieve adapter.");
        }

        FeatureLevel[] featureLevels =
        [
            FeatureLevel.Level_11_1, // Probeer eerst 11.1
            FeatureLevel.Level_11_0, // Dan 11.0
            FeatureLevel.Level_10_1, // Dan 10.1
            FeatureLevel.Level_10_0, // Dan 10.0
            FeatureLevel.Level_9_3,  // En zo verder...
            FeatureLevel.Level_9_2,
            FeatureLevel.Level_9_1
        ];

        // Gebruik DriverType.Unknown om de driver automatisch te laten bepalen (Hardware of WARP)
        var deviceResult = D3D11.D3D11CreateDevice(
            Adapter,
            DriverType.Unknown, // Laat DirectX de beste driver kiezen
            DeviceCreationFlags.BgraSupport,
            featureLevels,
            out Device!
        );

        if (!deviceResult.Success || Device == null)
        {
            // Als het nog steeds mislukt, probeer dan de WARP (software) driver expliciet
            // Dit is een fallback voor systemen zonder geschikte hardware driver
            deviceResult = D3D11.D3D11CreateDevice(
                null, // null voor adapter betekent dat het de WARP adapter zal gebruiken
                DriverType.Software, // Dwingt het gebruik van de WARP driver
                DeviceCreationFlags.BgraSupport,
                featureLevels,
                out Device!
            );

            if (!deviceResult.Success || Device == null)
            {
                throw new InvalidOperationException("Failed to create Direct3D11 device even with WARP fallback.");
            }
        }

        var outputResult = Adapter.EnumOutputs(0, out IDXGIOutput output);
        if (!outputResult.Success || output == null)
        {
            throw new InvalidOperationException("Failed to retrieve output.");
        }
        Output = output.QueryInterface<IDXGIOutput1>();
        DuplicatedOutput = Output.DuplicateOutput(Device);


        Console.WriteLine(Device.FeatureLevel.ToString());  // >= 11.0 = goed
        Console.WriteLine(Adapter.Description.Description);  // "Microsoft Basic Render Driver" = WARP!
    }

    public IEnumerable<byte[]> ReadEnumerable(IKillSwitch killSwitch)
    {
        var frame = CaptureFrame();
        if (!killSwitch.KillSwitch)
            yield return frame.Buffer;
        while (!killSwitch.KillSwitch)
            yield return CaptureFrame(frame.Buffer).Buffer;
    }

    public Frame CaptureFrame(byte[]? buffer = null)
    {
        var result = DuplicatedOutput.AcquireNextFrame(1,
            out OutduplFrameInfo _,
            out IDXGIResource? screenResource);

        while (!result.Success || screenResource == null)
        {
            Thread.Sleep(1); // Wait for the next frame
            result = DuplicatedOutput.AcquireNextFrame(1, out _, out screenResource);
        }

        // Create texture2D from IDXGIResource
        using var texture2D = screenResource.QueryInterface<ID3D11Texture2D>();
        var desc = texture2D.Description;

        var width = Convert.ToInt32(desc.Width);
        var height = Convert.ToInt32(desc.Height);

        // Create staging texture CPU-readable
        var stagingDesc = new Texture2DDescription
        {
            CPUAccessFlags = CpuAccessFlags.Read,
            BindFlags = BindFlags.None,
            Format = desc.Format,
            Width = desc.Width,
            Height = desc.Height,
            MipLevels = 1,
            ArraySize = 1,
            SampleDescription = new SampleDescription(1, 0),
            Usage = ResourceUsage.Staging
        };

        using var stagingTexture = Device.CreateTexture2D(stagingDesc);
        var context = Device.ImmediateContext;
        context.CopyResource(stagingTexture, texture2D);

        // Map staging texture to memory
        var dataBox = context.Map(stagingTexture, 0, MapMode.Read, Vortice.Direct3D11.MapFlags.None);

        // Converteer BGRA → BGR
        buffer = BgraToBgr.ConvertBgraToBgr(dataBox, width, height, buffer);

        // Clean up
        context.Unmap(stagingTexture, 0);
        DuplicatedOutput.ReleaseFrame();

        return new(buffer, new(width, height));
    }

    public void Dispose()
    {
        DuplicatedOutput?.Dispose();
        Output?.Dispose();
        Device?.Dispose();
        Adapter?.Dispose();
        Factory?.Dispose();
        BgraToBgr?.Dispose();

        GC.SuppressFinalize(this);
    }
}