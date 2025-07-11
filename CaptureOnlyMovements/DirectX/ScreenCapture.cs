using SharpDX.DXGI;
using SharpDX.Direct3D11;
using CaptureOnlyMovements.Types;
using CaptureOnlyMovements.Interfaces;

namespace CaptureOnlyMovements.DirectX;

public class ScreenshotCapturer : IDisposable
{
    private Factory1 Factory;
    private Adapter1 Adapter;
    private Output1 Output;
    private SharpDX.Direct3D11.Device Device;
    private OutputDuplication DuplicatedOutput;

    public ScreenshotCapturer()
    {
        Factory = new Factory1();
        Adapter = Factory.GetAdapter1(0);
        Device = new SharpDX.Direct3D11.Device(Adapter);
        Output = Adapter.GetOutput(0).QueryInterface<Output1>();
        DuplicatedOutput = Output.DuplicateOutput(Device);
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
        SharpDX.DXGI.Resource? screenResource = null;
        OutputDuplicateFrameInformation info;

        DuplicatedOutput.TryAcquireNextFrame(1, out info, out screenResource);

        while (screenResource == null)
        {
            Thread.Sleep(1); // Wait for the next frame
            DuplicatedOutput.TryAcquireNextFrame(1, out info, out screenResource);
        }

        using var texture2D = screenResource.QueryInterface<Texture2D>();
        var desc = texture2D.Description;

        var width = desc.Width;
        var height = desc.Height;

        // Create staging texture CPU-readable
        var stagingDesc = new Texture2DDescription
        {
            CpuAccessFlags = CpuAccessFlags.Read,
            BindFlags = BindFlags.None,
            Format = desc.Format,
            Width = desc.Width,
            Height = desc.Height,
            MipLevels = 1,
            ArraySize = 1,
            SampleDescription = new SampleDescription(1, 0),
            Usage = ResourceUsage.Staging,
            OptionFlags = ResourceOptionFlags.None
        };

        using var stagingTexture = new Texture2D(Device, stagingDesc);

        var context = Device.ImmediateContext;
        context.CopyResource(texture2D, stagingTexture);

        // Map staging texture to memory
        var dataBox = context.MapSubresource(stagingTexture, 0, MapMode.Read, SharpDX.Direct3D11.MapFlags.None);
        int stride = dataBox.RowPitch;
        int size = stride * desc.Height;

        // Converteer BGRA → RGB
        int srcStride = dataBox.RowPitch;
        int dstStride = width * 3;
        int rgbSize = dstStride * height;

        if (buffer == null || buffer.Length != rgbSize)
            buffer = new byte[rgbSize];

        unsafe
        {
            byte* srcPtr = (byte*)dataBox.DataPointer;

            for (int y = 0; y < height; y++)
            {
                int srcRow = y * srcStride;
                int dstRow = y * dstStride;

                for (int x = 0; x < width; x++)
                {
                    int srcIndex = srcRow + x * 4; // BGRA
                    int dstIndex = dstRow + x * 3; // RGB

                    buffer[dstIndex + 0] = srcPtr[srcIndex + 2]; // R
                    buffer[dstIndex + 1] = srcPtr[srcIndex + 1]; // G
                    buffer[dstIndex + 2] = srcPtr[srcIndex + 0]; // B
                }
            }
        }

        context.UnmapSubresource(stagingTexture, 0);

        screenResource.Dispose();
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
    }
}
