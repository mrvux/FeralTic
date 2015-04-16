using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SlimDX.Direct3D11;

namespace FeralTic.DX11.Resources
{
    public class DX11MipSliceRenderTarget2D : DX11Texture2D, IDX11RenderTargetView,IDX11RWResource, IDisposable
    {
        public DX11MipSliceRenderTarget2D(DX11RenderContext context, DX11Texture2D texture, int mipindex, int w, int h)
        {
            this.context = context;
            this.Resource = texture.Resource;

            RenderTargetViewDescription rtd = new RenderTargetViewDescription()
            {
                Dimension = RenderTargetViewDimension.Texture2D,
                Format = texture.Format,
                MipSlice = mipindex
            };

            UnorderedAccessViewDescription uavd = new UnorderedAccessViewDescription()
            {
                Dimension = UnorderedAccessViewDimension.Texture2D,
                Format = texture.Format,
                MipSlice = mipindex,
            };

            ShaderResourceViewDescription srvd = new ShaderResourceViewDescription();
            srvd.Dimension = ShaderResourceViewDimension.Texture2D;
            srvd.MipLevels = 1;
            srvd.MostDetailedMip = mipindex;
            srvd.Format = texture.Format;

            this.SRV = new ShaderResourceView(context.Device, texture.Resource, srvd);
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
