using CaptureOnlyMovements.Types;
using System;
using System.Drawing;
using System.Windows.Forms;
using Vortice.Direct3D11;
using Vortice.DXGI;
using MapFlags = Vortice.Direct3D11.MapFlags;
using Vortice.Mathematics;
using Vortice.Direct3D;
using System.Runtime.InteropServices;
using Vortice.D3DCompiler; // Voor shadercompilatie

namespace CaptureOnlyMovements.Forms.Controls;

public class DisplayControl : UserControl
{
    public Resolution Resolution { get; private set; }
    public Resizer? Resizer { get; private set; }

    private Frame? FrameBuffer1;
    private Frame? FrameBuffer2;
    private bool FrameBufferSwitch;

    public bool Dirty { get; private set; }

    private Frame? RenderFrame => FrameBufferSwitch ? FrameBuffer1 : FrameBuffer2;
    private void SetInputFrame(Frame value)
    {
        if (!FrameBufferSwitch)
        {
            FrameBuffer1 = value;
        }
        else
        {
            FrameBuffer2 = value;
        }
        FrameBufferSwitch = !FrameBufferSwitch;
        Dirty = true;
    }

    // Direct3D fields
    private IDXGISwapChain1? _swapChain;
    private ID3D11Device? _device;
    private ID3D11DeviceContext? _context;
    private ID3D11Texture2D? _backBuffer;
    private ID3D11RenderTargetView? _renderTargetView;
    private Timer? _renderTimer;

    // Nieuwe velden voor shaders en rendering
    private ID3D11VertexShader? _vertexShader;
    private ID3D11PixelShader? _pixelShader;
    private ID3D11InputLayout? _inputLayout;
    private ID3D11Buffer? _vertexBuffer;
    private ID3D11ShaderResourceView? _shaderResourceView;
    private ID3D11SamplerState? _samplerState;

    public DisplayControl()
    {
        BackColor = System.Drawing.Color.Black;
        Resize += DisplayControl_Resize;
        Load += DisplayControl_Load;
        Disposed += DisplayControl_Disposed;
    }

    private void DisplayControl_Load(object? sender, EventArgs e)
    {
        InitializeD3D();
        InitializeShaders();
        InitializeVertexBuffer();
        InitializeSamplerState();
        DisplayControl_Resize(this, EventArgs.Empty);
        FrameBuffer1 = new Frame(Resolution);
        FrameBuffer2 = new Frame(Resolution);

        // Start render timer
        _renderTimer = new Timer { Interval = 16 }; // ~60 FPS
        _renderTimer.Tick += (s, ev) => RenderLoop();
        _renderTimer.Start();
    }

    private void InitializeD3D()
    {
        var swapChainDesc = new SwapChainDescription1
        {
            Width = Convert.ToUInt32(Math.Max(1, Width)),
            Height = Convert.ToUInt32(Math.Max(1, Height)),
            Format = Format.B8G8R8A8_UNorm,
            Stereo = false,
            SampleDescription = new SampleDescription(1, 0),
            BufferCount = 2,
            Scaling = Scaling.Stretch,
            SwapEffect = SwapEffect.FlipSequential,
            AlphaMode = AlphaMode.Ignore,
            Flags = SwapChainFlags.None
        };

        IDXGIFactory2 factory = DXGI.CreateDXGIFactory2<IDXGIFactory2>(true);
        D3D11.D3D11CreateDevice(
            null,
            DriverType.Hardware,
            DeviceCreationFlags.BgraSupport,
            null,
            out _device,
            out _context);

        _swapChain = factory.CreateSwapChainForHwnd(_device, Handle, swapChainDesc);

        CreateRenderTarget();
    }

    private void InitializeShaders()
    {
        if (_device == null) return;

        // Binnen InitializeShaders
        string vertexShaderSource = @"
    struct VS_INPUT {
        float4 Position : POSITION;
        float2 TexCoord : TEXCOORD0;
    };
    struct PS_INPUT {
        float4 Position : SV_POSITION;
        float2 TexCoord : TEXCOORD0;
    };
    PS_INPUT VS(VS_INPUT input) {
        PS_INPUT output;
        output.Position = input.Position;
        output.TexCoord = input.TexCoord;
        return output;
    }";

        var vsByteCode = Compiler.Compile(vertexShaderSource, "VS", "vertex.hlsl", "vs_5_0");
        _vertexShader = _device.CreateVertexShader(vsByteCode.Span); 

        // Input layout
        var inputElements = new[]
        {
    new InputElementDescription("POSITION", 0, Format.R32G32B32A32_Float, 0, 0),
    new InputElementDescription("TEXCOORD", 0, Format.R32G32_Float, 16, 0)
};
        _inputLayout = _device.CreateInputLayout(inputElements, vsByteCode.Span);

        // Compileer pixelshader
        string pixelShaderSource = @"
    Texture2D Texture : register(t0);
    SamplerState Sampler : register(s0);
    float4 PS(float4 pos : SV_POSITION, float2 texCoord : TEXCOORD0) : SV_TARGET {
        return Texture.Sample(Sampler, texCoord);
    }";

        var psByteCode = Compiler.Compile(pixelShaderSource, "PS", "pixel.hlsl", "ps_5_0");
        _pixelShader = _device.CreatePixelShader(psByteCode.Span);
    }

    private void InitializeVertexBuffer()
    {
        if (_device == null) return;

        // Define a screen-filling quad (two triangles)
        var vertices = new[]
        {
            // Position (x, y, z, w), TexCoord (u, v)
            -1.0f,  1.0f, 0.0f, 1.0f, 0.0f, 0.0f, // Top-left
             1.0f,  1.0f, 0.0f, 1.0f, 1.0f, 0.0f, // Top-right
            -1.0f, -1.0f, 0.0f, 1.0f, 0.0f, 1.0f, // Bottom-left
             1.0f, -1.0f, 0.0f, 1.0f, 1.0f, 1.0f  // Bottom-right
        };

        var bufferDesc = new BufferDescription
        {
            ByteWidth = (uint)(vertices.Length * sizeof(float)),
            Usage = ResourceUsage.Default,
            BindFlags = BindFlags.VertexBuffer,
            CPUAccessFlags = CpuAccessFlags.None
        };

        // Fix: Create SubresourceData for the vertex buffer
        var vertexBufferData = new SubresourceData
        {
            DataPointer = Marshal.UnsafeAddrOfPinnedArrayElement(vertices, 0),
            RowPitch = 0,
            SlicePitch = 0
        };

        _vertexBuffer = _device.CreateBuffer(bufferDesc, vertexBufferData);
    }

    private ushort[] GetIndexBuffer()
    {
        return new ushort[]
        {
            0, 1, 2, // Eerste driehoek
            2, 1, 3  // Tweede driehoek
        };
    }

    private void InitializeSamplerState()
    {
        if (_device == null) return;

        var samplerDesc = new SamplerDescription
        {
            Filter = Filter.MinMagMipLinear,
            AddressU = TextureAddressMode.Clamp,
            AddressV = TextureAddressMode.Clamp,
            AddressW = TextureAddressMode.Clamp,
            ComparisonFunc = ComparisonFunction.Never,
            MinLOD = 0,
            MaxLOD = float.MaxValue
        };
        _samplerState = _device.CreateSamplerState(samplerDesc);
    }

    private void DisplayControl_Resize(object? sender, EventArgs e)
    {
        Resolution = new Resolution(Width, Height);
        Resizer = new Resizer(Resolution);
        ResizeD3D();
    }

    public void SetFrame(Frame frame)
    {
        if (Resizer == null) return;
        if (frame.Resolution != Resolution)
        {
            frame = Resizer.Resize(frame);
        }
        SetInputFrame(frame);
    }

    private void RenderLoop()
    {
        var renderFrame = RenderFrame;
        if (renderFrame == null || !Dirty || _device == null || _context == null || _swapChain == null)
            return;

        // Upload frame buffer to a D3D texture
        var data = renderFrame.Buffer;
        if (data.Length != Resolution.ByteLength) return;

        byte[] textureData;
        if (data.Length == Resolution.PixelLength * 3)
        {
            textureData = ConvertRgbToRgba(data, Resolution.PixelLength);
        }
        else
        {
            textureData = data;
        }

        // Create texture
        var texDesc = new Texture2DDescription
        {
            Width = Convert.ToUInt32(Resolution.Width),
            Height = Convert.ToUInt32(Resolution.Height),
            MipLevels = 1,
            ArraySize = 1,
            Format = Format.R8G8B8A8_UNorm,
            SampleDescription = new SampleDescription(1, 0),
            Usage = ResourceUsage.Dynamic,
            BindFlags = BindFlags.ShaderResource,
            CPUAccessFlags = CpuAccessFlags.Write
        };

        using var texture = _device.CreateTexture2D(texDesc);

        // Write data to texture
        var box = _context.Map(texture, 0, MapMode.WriteDiscard, MapFlags.None);
        Marshal.Copy(textureData, 0, box.DataPointer, textureData.Length);
        _context.Unmap(texture, 0);

        // Create ShaderResourceView
        _shaderResourceView?.Dispose(); // Release previous resource
        _shaderResourceView = _device.CreateShaderResourceView(texture);

        // Set rendering pipeline
        _context.OMSetRenderTargets(_renderTargetView!);
        _context.ClearRenderTargetView(_renderTargetView, new Color4(0, 0, 0, 1));

        // Set shaders and input layout
        _context.VSSetShader(_vertexShader);
        _context.PSSetShader(_pixelShader);
        _context.IASetInputLayout(_inputLayout);

        // Set vertex buffer
        _context.IASetVertexBuffers(0, 1, new[] { _vertexBuffer! }, new[] { Convert.ToUInt32(24) }, new[] { Convert.ToUInt32(0) });
        _context.IASetPrimitiveTopology(PrimitiveTopology.TriangleList);

        // Set index buffer
        var indices = GetIndexBuffer();
        var indexBufferDesc = new BufferDescription
        {
            ByteWidth = (uint)(indices.Length * sizeof(ushort)),
            Usage = ResourceUsage.Default,
            BindFlags = BindFlags.IndexBuffer,
            CPUAccessFlags = CpuAccessFlags.None
        };

        // Fix: Create SubresourceData for the index buffer
        var indexBufferData = new SubresourceData
        {
            DataPointer = Marshal.UnsafeAddrOfPinnedArrayElement(indices, 0),
            RowPitch = 0,
            SlicePitch = 0
        };

        using var indexBuffer = _device.CreateBuffer(indexBufferDesc, indexBufferData);
        _context.IASetIndexBuffer(indexBuffer, Format.R16_UInt, 0);

        // Set texture and sampler
        _context.PSSetShaderResources(0, 1, new[] { _shaderResourceView });
        _context.PSSetSamplers(0, 1, new[] { _samplerState });

        // Draw the quad
        _context.DrawIndexed(6, 0, 0);

        // Present the result
        _swapChain.Present(1, PresentFlags.None);

        Dirty = false;
    }

    private void ResizeD3D()
    {
        if (_swapChain != null)
        {
            _context?.OMSetRenderTargets((ID3D11RenderTargetView)null!);
            _renderTargetView?.Dispose();
            _backBuffer?.Dispose();

            _swapChain.ResizeBuffers(2, Convert.ToUInt32(Math.Max(1, Width)), Convert.ToUInt32(Math.Max(1, Height)), Format.B8G8R8A8_UNorm, SwapChainFlags.None);
            CreateRenderTarget();
        }
    }

    private void CreateRenderTarget()
    {
        if (_swapChain == null || _device == null) return;
        _backBuffer = _swapChain.GetBuffer<ID3D11Texture2D>(0);
        _renderTargetView = _device.CreateRenderTargetView(_backBuffer);
    }

    private void DisplayControl_Disposed(object? sender, EventArgs e)
    {
        _renderTimer?.Stop();
        _shaderResourceView?.Dispose();
        _samplerState?.Dispose();
        _vertexBuffer?.Dispose();
        _inputLayout?.Dispose();
        _vertexShader?.Dispose();
        _pixelShader?.Dispose();
        _renderTargetView?.Dispose();
        _backBuffer?.Dispose();
        _swapChain?.Dispose();
        _context?.Dispose();
        _device?.Dispose();
    }

    private byte[] ConvertRgbToRgba(byte[] rgb, int pixelCount)
    {
        var rgba = new byte[pixelCount * 4];
        for (int i = 0, j = 0; i < rgb.Length; i += 3, j += 4)
        {
            rgba[j] = rgb[i];       // R
            rgba[j + 1] = rgb[i + 1]; // G
            rgba[j + 2] = rgb[i + 2]; // B
            rgba[j + 3] = 255;      // A
        }
        return rgba;
    }
}