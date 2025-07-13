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
using Vortice.D3DCompiler;
using CaptureOnlyMovements.Filters;

namespace CaptureOnlyMovements.Forms.Controls;

public class DisplayControl : UserControl
{
    public DisplayControl()
    {
        BackColor = System.Drawing.Color.Black;
        //Resize += DisplayControl_Resize;
        Load += DisplayControl_Load;
        Disposed += DisplayControl_Disposed;
    }

    public Resolution Resolution { get; private set; }
    //public BgrResizer? BgrResizer { get; private set; }
    //public BgraResizer? BgraResizer { get; private set; }

    private byte[]? FrameBuffer1;
    private byte[]? FrameBuffer2;
    private bool FrameBufferSwitch;

    public bool Dirty { get; private set; }

    private byte[]? RenderBuffer => FrameBufferSwitch ? FrameBuffer1 : FrameBuffer2;
    private void SetInputFrameBuffer(byte[] value)
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

    private IDXGISwapChain1? _swapChain;
    private ID3D11Device? _device;
    private ID3D11DeviceContext? _context;
    private ID3D11Texture2D? _backBuffer;
    private ID3D11RenderTargetView? _renderTargetView;
    private Timer? _renderTimer;

    private ID3D11VertexShader? _vertexShader;
    private ID3D11PixelShader? _pixelShader;
    private ID3D11InputLayout? _inputLayout;
    private ID3D11Buffer? _vertexBuffer;
    private ID3D11ShaderResourceView? _shaderResourceView;
    private ID3D11SamplerState? _samplerState;


    private void DisplayControl_Load(object? sender, EventArgs e)
    {
        DisplayControl_Resize(this, EventArgs.Empty);

        InitializeD3D();
        InitializeShaders();
        InitializeVertexBuffer();
        InitializeSamplerState();

        _renderTimer = new Timer { Interval = 16 }; 
        _renderTimer.Tick += (s, ev) => RenderLoop();
        _renderTimer.Start();
    }

    private void InitializeD3D()
    {
        var swapChainDesc = new SwapChainDescription1
        {
            Width = Convert.ToUInt32(Math.Max(1, Resolution.Width)),
            Height = Convert.ToUInt32(Math.Max(1, Resolution.Height)),
            Format = Format.B8G8R8A8_UNorm,
            Stereo = false,
            SampleDescription = new SampleDescription(1, 0),
            BufferCount = 2,
            Scaling = Scaling.Stretch,
            SwapEffect = SwapEffect.Discard,
            AlphaMode = AlphaMode.Ignore,
            Flags = SwapChainFlags.None,

            BufferUsage = Usage.RenderTargetOutput
        };


        // --- AANGEPASTE SECTIE VOOR DEVICE CREATIE ---
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

        IDXGIFactory2 factory = DXGI.CreateDXGIFactory2<IDXGIFactory2>(true);
        D3D11.D3D11CreateDevice(
            null,
            DriverType.Warp,
            DeviceCreationFlags.BgraSupport | DeviceCreationFlags.Debug,
            featureLevels,
            out _device,
            out _context);

        if (_device == null || _context == null)
        {
            throw new InvalidOperationException("Failed to create D3D11 device and context.");
        }

        _swapChain = factory.CreateSwapChainForHwnd(_device, Handle, swapChainDesc);
        if (_swapChain == null)
        {
            throw new InvalidOperationException("Failed to create swap chain.");
        }

        CreateRenderTarget();
    }
    

    private void InitializeShaders()
    {
        if (_device == null) return;

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

        var inputElements = new[]
        {
    new InputElementDescription("POSITION", 0, Format.R32G32B32A32_Float, 0, 0),
    new InputElementDescription("TEXCOORD", 0, Format.R32G32_Float, 16, 0)
};
        _inputLayout = _device.CreateInputLayout(inputElements, vsByteCode.Span);

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

        var vertices = new[]
        {
            -1.0f,  1.0f, 0.0f, 1.0f, 0.0f, 0.0f,
             1.0f,  1.0f, 0.0f, 1.0f, 1.0f, 0.0f,
            -1.0f, -1.0f, 0.0f, 1.0f, 0.0f, 1.0f,
             1.0f, -1.0f, 0.0f, 1.0f, 1.0f, 1.0f
        };

        var bufferDesc = new BufferDescription
        {
            ByteWidth = (uint)(vertices.Length * sizeof(float)),
            Usage = ResourceUsage.Default,
            BindFlags = BindFlags.VertexBuffer,
            CPUAccessFlags = CpuAccessFlags.None
        };

        var vertexBufferData = new SubresourceData
        {
            DataPointer = Marshal.UnsafeAddrOfPinnedArrayElement(vertices, 0),
            RowPitch = 0,
            SlicePitch = 0
        };

        _vertexBuffer = _device.CreateBuffer(bufferDesc, vertexBufferData);
    }

    private static ushort[] GetIndexBuffer()
    {
        return
        [
            0, 1, 2,
            2, 1, 3
        ];
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
        //BgrResizer = new BgrResizer(Resolution);
        //BgraResizer = new BgraResizer(Resolution);
        FrameBuffer1 = new Frame(Resolution).Buffer;
        FrameBuffer2 = new Frame(Resolution).Buffer;
        ResizeD3D();
    }

    public void SetFrame(Frame frame)
    {
        //if (BgrResizer == null) return;

        if(frame.Resolution != Resolution)
        {
            Resolution = frame.Resolution;
            //BgrResizer = new BgrResizer(Resolution);
            //BgraResizer = new BgraResizer(Resolution);
            FrameBuffer1 = new Frame(Resolution).Buffer;
            FrameBuffer2 = new Frame(Resolution).Buffer;
            ResizeD3D();
        }

        //frame = BgrResizer.Resize(frame);
        var bgraBuffer = frame.Buffer.BgrToBgra();
        SetInputFrameBuffer(bgraBuffer);
    }
    public void SetFrame(bool[] frameData, Resolution frameResolution)
    {
        //if (BgraResizer == null) return;

        if (frameResolution != Resolution)
        {
            Resolution = frameResolution;
            //BgrResizer = new BgrResizer(Resolution);
            //BgraResizer = new BgraResizer(Resolution);
            FrameBuffer1 = new Frame(Resolution).Buffer;
            FrameBuffer2 = new Frame(Resolution).Buffer;
            ResizeD3D();
        }

        var bgraBuffer = frameData.BwToBgra();
        var bgraFrame = new Frame(bgraBuffer, frameResolution);
        //bgraFrame = BgraResizer.Resize(bgraFrame);
        SetInputFrameBuffer(bgraFrame.Buffer);
    }

    private void RenderLoop()
    {
        var textureData = RenderBuffer;
        if (textureData == null || !Dirty || _device == null || _context == null || _swapChain == null)
            return;

        var texDesc = new Texture2DDescription
        {
            Width = Convert.ToUInt32(Resolution.Width),
            Height = Convert.ToUInt32(Resolution.Height),
            MipLevels = 1,
            ArraySize = 1,
            Format = Format.B8G8R8A8_UNorm,
            SampleDescription = new SampleDescription(1, 0),
            Usage = ResourceUsage.Dynamic,
            BindFlags = BindFlags.ShaderResource,
            CPUAccessFlags = CpuAccessFlags.Write
        };

        using var texture = _device.CreateTexture2D(texDesc);

        var box = _context.Map(texture, 0, MapMode.WriteDiscard, MapFlags.None);
        Marshal.Copy(textureData, 0, box.DataPointer, textureData.Length);
        _context.Unmap(texture, 0);

        _shaderResourceView?.Dispose();
        _shaderResourceView = _device.CreateShaderResourceView(texture);

        _context.OMSetRenderTargets(_renderTargetView!);
        _context.ClearRenderTargetView(_renderTargetView, new Color4(0, 0, 0, 1));

        _context.VSSetShader(_vertexShader);
        _context.PSSetShader(_pixelShader);
        _context.IASetInputLayout(_inputLayout);

        _context.IASetVertexBuffers(0, 1, [_vertexBuffer!], [Convert.ToUInt32(24)], [Convert.ToUInt32(0)]);
        _context.IASetPrimitiveTopology(PrimitiveTopology.TriangleList);

        var indices = GetIndexBuffer();
        var indexBufferDesc = new BufferDescription
        {
            ByteWidth = (uint)(indices.Length * sizeof(ushort)),
            Usage = ResourceUsage.Default,
            BindFlags = BindFlags.IndexBuffer,
            CPUAccessFlags = CpuAccessFlags.None
        };

        var indexBufferData = new SubresourceData
        {
            DataPointer = Marshal.UnsafeAddrOfPinnedArrayElement(indices, 0),
            RowPitch = 0,
            SlicePitch = 0
        };

        using var indexBuffer = _device.CreateBuffer(indexBufferDesc, indexBufferData);
        _context.IASetIndexBuffer(indexBuffer, Format.R16_UInt, 0);

        _context.PSSetShaderResources(0, 1, [_shaderResourceView]);
        _context.PSSetSamplers(0, 1, [_samplerState!]);

        _context.DrawIndexed(6, 0, 0);

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
            _swapChain.ResizeBuffers(2, Convert.ToUInt32(Math.Max(1, Resolution.Width)), Convert.ToUInt32(Math.Max(1, Resolution.Height)), Format.B8G8R8A8_UNorm, SwapChainFlags.None);
            CreateRenderTarget();
        }
    }
    
    private void CreateRenderTarget()
    {
        if (_swapChain == null || _device == null)
        {
            throw new InvalidOperationException("Swap chain or device is null.");
        }

        _backBuffer = _swapChain.GetBuffer<ID3D11Texture2D>(0);
        if (_backBuffer == null)
        {
            throw new InvalidOperationException("Failed to retrieve back buffer from swap chain.");
        }

        var rtvDesc = new RenderTargetViewDescription
        {
            Format = Format.Unknown,
            ViewDimension = RenderTargetViewDimension.Texture2D,
            Texture2D = new Texture2DRenderTargetView { MipSlice = 0 }
        };
        _renderTargetView = _device.CreateRenderTargetView(_backBuffer, rtvDesc);
        // ↓  NIEUW: viewport zetten
        var vp = new Viewport(0, 0,
                              _backBuffer.Description.Width,
                              _backBuffer.Description.Height,
                              0.0f, 1.0f);
        _context!.RSSetViewport(vp);
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

}