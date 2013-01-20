using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SlimDX.Direct3D11;

namespace FeralTic.DX11.Resources
{
    public interface IDX11Resource : IDisposable
    {
    }

    /// <summary>
    /// Read resources are readable via shaders, so they need an SRV
    /// </summary>
    public interface IDX11ReadableResource : IDX11Resource
    {
        ShaderResourceView SRV { get; }
    }

    /// <summary>
    /// Read write resources are readable/writeable via compute, so they provide SRV/UAV
    /// </summary>
    public interface IDX11RWResource : IDX11ReadableResource, IDisposable
    {
        UnorderedAccessView UAV { get; }
    }

    public interface IDX11RenderTargetView : IDX11Resource
    {
        RenderTargetView RTV { get; }
        int Width { get; }
        int Height { get; }
    }

    public interface IDX11DepthStencil : IDX11Resource
    {
        DepthStencilView DSV { get; }
        DepthStencilView ReadOnlyDSV { get; }
        int Width { get; }
        int Height { get; }
    }
}
