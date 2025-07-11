using SharpDX.DXGI;
using SharpDX.Direct3D11;
using CaptureOnlyMovements.Types;

namespace CaptureOnlyMovements.DirectX;

public class ScreenshotCapturer : IDisposable
{
    private Factory1 _factory;
    private Adapter1 _adapter;
    private Output1 _output;
    private SharpDX.Direct3D11.Device _device;
    private OutputDuplication _duplicatedOutput;

    public ScreenshotCapturer()
    {
        _factory = new Factory1();
        _adapter = _factory.GetAdapter1(0);
        _device = new SharpDX.Direct3D11.Device(_adapter);
        _output = _adapter.GetOutput(0).QueryInterface<Output1>();
        _duplicatedOutput = _output.DuplicateOutput(_device);
    }

    public Frame CaptureFrame(byte[]? buffer = null)
    {
        SharpDX.DXGI.Resource? screenResource = null;

        while (screenResource == null)
        {
            _duplicatedOutput.TryAcquireNextFrame(10, out var info, out screenResource);
            Thread.Sleep(100); // Wait for the next frame
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

        using var stagingTexture = new Texture2D(_device, stagingDesc);

        var context = _device.ImmediateContext;
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
        _duplicatedOutput.ReleaseFrame();

        return new(buffer, new(width, height));
    }

    public void Dispose()
    {
        _duplicatedOutput?.Dispose();
        _output?.Dispose();
        _device?.Dispose();
        _adapter?.Dispose();
        _factory?.Dispose();
    }
}
