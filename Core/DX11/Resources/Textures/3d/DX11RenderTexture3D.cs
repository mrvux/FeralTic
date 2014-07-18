using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SlimDX.Direct3D11;
using SlimDX.DXGI;

namespace FeralTic.DX11.Resources
{
    public class DX11RenderTexture3D : DX11Texture3D, IDX11RenderTargetView, IDX11RWResource
    {

        public UnorderedAccessView UAV { get; protected set; }
        public RenderTargetView RTV { get; protected set; }

        public DX11RenderTexture3D(DX11RenderContext context, Texture3D tex, ShaderResourceView srv, UnorderedAccessView uav)
            : base(context)
        {
            this.Resource = tex;

            this.SRV = srv;

            this.UAV = uav;
        }

        public DX11RenderTexture3D(DX11RenderContext context, Texture3D tex)
            : base(context)
        {
            this.Resource = tex;

            this.SRV = new ShaderResourceView(context.Device, tex);

            this.UAV = new UnorderedAccessView(context.Device, tex);
        }

        public DX11RenderTexture3D(DX11RenderContext context, int w, int h, int d, Format format)
            : base(context)
        {
            Texture3DDescription desc = new Texture3DDescription()
            {
                BindFlags = BindFlags.UnorderedAccess | BindFlags.ShaderResource | BindFlags.RenderTarget,
                CpuAccessFlags = CpuAccessFlags.None,
                Depth = d,
                Format = format,
                Height = h,
                MipLevels = 1,
                OptionFlags = ResourceOptionFlags.None,
                Usage = ResourceUsage.Default,
                Width = w
            };

            RenderTargetViewDescription rtvd = new RenderTargetViewDescription();
            rtvd.Format = format;
            rtvd.Dimension = RenderTargetViewDimension.Texture3D;
            rtvd.MipSlice = 0;
            rtvd.FirstDepthSlice = 0;
            rtvd.DepthSliceCount = d;

            this.Resource = new Texture3D(context.Device, desc);
            this.SRV = new ShaderResourceView(context.Device, this.Resource);
            this.UAV = new UnorderedAccessView(context.Device, this.Resource);
            this.RTV = new RenderTargetView(context.Device, this.Resource, rtvd);

            this.Width = desc.Width;
            this.Height = desc.Height;
            this.Format = desc.Format;
            this.Depth = desc.Depth;
        }


        public override void  Dispose()
        {
            if (this.SRV != null) { this.SRV.Dispose(); }
            if (this.RTV != null) { this.RTV.Dispose(); }
            if (this.UAV != null) { this.UAV.Dispose(); }
            if (this.Resource != null) { this.Resource.Dispose(); }
        }



    }
}
