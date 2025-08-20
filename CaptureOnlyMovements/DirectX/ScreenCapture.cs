using CaptureOnlyMovements.FrameConverters;
using CaptureOnlyMovements.Interfaces;
using CaptureOnlyMovements.Types;
using System;
using System.Threading;
using Vortice.Direct3D;
using Vortice.Direct3D11;
using Vortice.DXGI;

namespace CaptureOnlyMovements.DirectX;
public class ScreenshotCapturer : IDisposable
{
    private readonly IConsole Console;
    private IDXGIFactory1? Factory;
    private IDXGIAdapter1? Adapter;
    private IDXGIOutput1? Output;
    private ID3D11Device? Device;
    private IDXGIOutputDuplication? DuplicatedOutput;
    private readonly BgraToBgrConverterUnsafe? BgraToBgr;

    private readonly FeatureLevel[] FeatureLevels =
    [
        FeatureLevel.Level_11_1,
        FeatureLevel.Level_11_0,
        FeatureLevel.Level_10_1,
        FeatureLevel.Level_10_0,
        FeatureLevel.Level_9_3,
        FeatureLevel.Level_9_2,
        FeatureLevel.Level_9_1
    ];

    public ScreenshotCapturer(IConsole console)
    {
        Console = console;
        BgraToBgr = new BgraToBgrConverterUnsafe();
        InitPipeline();
    }

    private void InitPipeline()
    {
        DisposePipeline();

        Factory = DXGI.CreateDXGIFactory1<IDXGIFactory1>();

        var adapterResult = Factory.EnumAdapters1(0, out Adapter);
        if (!adapterResult.Success || Adapter == null)
            throw new InvalidOperationException("Failed to retrieve adapter.");

        var deviceResult = D3D11.D3D11CreateDevice(
            Adapter,
            DriverType.Unknown,
            DeviceCreationFlags.BgraSupport,
            FeatureLevels,
            out Device!
        );

        if (!deviceResult.Success || Device == null)
        {
            deviceResult = D3D11.D3D11CreateDevice(
                null,
                DriverType.Software,
                DeviceCreationFlags.BgraSupport,
                FeatureLevels,
                out Device!
            );
            if (!deviceResult.Success || Device == null)
                throw new InvalidOperationException("Failed to create D3D11 device.");
        }

        var outputResult = Adapter.EnumOutputs(0, out IDXGIOutput output);
        if (!outputResult.Success || output == null)
            throw new InvalidOperationException("Failed to retrieve output.");

        Output = output.QueryInterface<IDXGIOutput1>();
        DuplicatedOutput = Output.DuplicateOutput(Device);

        Console.WriteLine(Device.FeatureLevel.ToString());
        Console.WriteLine(Adapter.Description.Description);
    }

    private void DisposePipeline()
    {
        DuplicatedOutput?.Dispose();
        Output?.Dispose();
        Device?.Dispose();
        Adapter?.Dispose();
        Factory?.Dispose();
    }

    public Frame? CaptureFrame(IKillSwitch killSwitch, byte[]? buffer = null)
    {
        Frame? frame = null;

        while (frame == null && !killSwitch.KillSwitch)
        {
            try
            {
                var result = DuplicatedOutput!.AcquireNextFrame(
                    33,
                    out OutduplFrameInfo _,
                    out IDXGIResource? screenResource
                );

                if (result.Failure || screenResource == null)
                {
                    if (result.Code == Vortice.DXGI.ResultCode.AccessLost.Code)
                    {
                        Console.WriteLine("Access lost → reinitializing pipeline...");
                        InitPipeline();
                        Thread.Sleep(500); // backoff
                    }
                    else if (result.Code == Vortice.DXGI.ResultCode.WaitTimeout.Code)
                    {
                        Thread.Sleep(1); // gewoon wachten op volgende frame
                    }
                    continue;
                }

                using var texture2D = screenResource!.QueryInterface<ID3D11Texture2D>();
                var desc = texture2D.Description;
                var width = (int)desc.Width;
                var height = (int)desc.Height;

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

                using var stagingTexture = Device!.CreateTexture2D(stagingDesc);
                var context = Device.ImmediateContext;
                context.CopyResource(stagingTexture, texture2D);

                var dataBox = context.Map(stagingTexture, 0, MapMode.Read, Vortice.Direct3D11.MapFlags.None);
                buffer = BgraToBgr!.ConvertBgraToBgr(dataBox, width, height, buffer);
                context.Unmap(stagingTexture, 0);
                DuplicatedOutput.ReleaseFrame();

                frame = new(buffer, new(width, height));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception in CaptureFrame: {ex.Message}");
                InitPipeline();
                Thread.Sleep(500);
            }
        }

        return frame;
    }

    public void Dispose()
    {
        DisposePipeline();
        BgraToBgr?.Dispose();
        GC.SuppressFinalize(this);
    }
}
