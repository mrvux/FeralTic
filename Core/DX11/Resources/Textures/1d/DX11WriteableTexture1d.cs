using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SlimDX.Direct3D11;
using SlimDX.DXGI;
using Device = SlimDX.Direct3D11.Device;

namespace FeralTic.DX11.Resources
{
    public class DX11WriteableTexture1d: DX11Texture1D, IDX11RWResource
    {
        public DX11WriteableTexture1d(DX11RenderContext context, int width, Format format) : base(context)
        {
            Texture1DDescription desc = new Texture1DDescription()
            {
                ArraySize = 1,
                BindFlags = BindFlags.ShaderResource | BindFlags.UnorderedAccess,
                CpuAccessFlags = CpuAccessFlags.None,
                Format = format,
                MipLevels = 1,
                OptionFlags = ResourceOptionFlags.None,
                Usage = ResourceUsage.Default,
                Width = width,
            };

            this.Resource = new Texture1D(context.Device, desc);

            this.SRV = new ShaderResourceView(context.Device, this.Resource);
            this.UAV = new UnorderedAccessView(context.Device, this.Resource);
        }

        public override void Dispose()
        {
            this.SRV.Dispose();
            this.UAV.Dispose();
            this.Resource.Dispose();
        }

        public UnorderedAccessView UAV
        {
            get;
            private set;
        }
    }
}

