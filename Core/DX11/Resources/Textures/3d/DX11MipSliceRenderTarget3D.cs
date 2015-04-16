using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SlimDX.Direct3D11;

namespace FeralTic.DX11.Resources
{
    public class DX11MipSliceRenderTarget3D : DX11Texture3D, IDX11RenderTargetView,IDX11RWResource, IDisposable
    {
        public DX11MipSliceRenderTarget3D(DX11RenderContext context, DX11Texture3D texture, int mipindex, int w, int h, int d) : base(context)
        {
            this.Resource = texture.Resource;

            RenderTargetViewDescription rtd = new RenderTargetViewDescription()
            {
                Dimension = RenderTargetViewDimension.Texture3D,
                Format = texture.Format,
                MipSlice = mipindex,
                DepthSliceCount = d           
            };


            UnorderedAccessViewDescription uavd = new UnorderedAccessViewDescription()
            {
                Dimension = UnorderedAccessViewDimension.Texture3D,
                Format = texture.Format,
                MipSlice = mipindex,
                FirstDepthSlice = 0,
                DepthSliceCount = d
            };

            ShaderResourceViewDescription srvd = new ShaderResourceViewDescription();
            srvd.Dimension = ShaderResourceViewDimension.Texture3D;
            srvd.MipLevels = 1;
            srvd.MostDetailedMip = mipindex;
            srvd.Format = texture.Format;
            srvd.ArraySize = d;
            srvd.FirstArraySlice = 0;



            this.RTV = new RenderTargetView(context.Device, texture.Resource, rtd);
            this.SRV = new ShaderResourceView(context.Device, texture.Resource, srvd);
            this.UAV = new UnorderedAccessView(context.Device, texture.Resource, uavd);

            this.Width = w;
            this.Height = h;
            this.Depth = d;
        }

        public RenderTargetView RTV { get; protected set; }
        public UnorderedAccessView UAV { get; protected set; }

        public override void Dispose()
        {
            this.RTV.Dispose();
            this.SRV.Dispose();
            this.UAV.Dispose();
        }
    }
}
