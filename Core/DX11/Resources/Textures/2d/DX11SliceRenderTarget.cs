using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SlimDX.Direct3D11;

namespace FeralTic.DX11.Resources
{
    public class DX11SliceRenderTarget : DX11Texture2D , IDX11RenderTargetView, IDisposable
    {
        private DX11Texture2D parent;

        public DX11SliceRenderTarget(DX11RenderContext context, DX11Texture2D texture, int sliceindex)
        {
            this.isowner = false;
            this.context = context;
            this.parent = texture;
            this.desc = texture.Description;

            RenderTargetViewDescription rtd = new RenderTargetViewDescription()
            {
                ArraySize = 1,
                Dimension = RenderTargetViewDimension.Texture2DArray,
                Format = texture.Format,
                MipSlice = 0,
                FirstArraySlice = sliceindex,
            };

            this.RTV = new RenderTargetView(context.Device, this.parent.Resource, rtd);

            ShaderResourceViewDescription srvd = new ShaderResourceViewDescription()
            {
                ArraySize = 1,
                Dimension = ShaderResourceViewDimension.Texture2DArray,
                Format = texture.Format,
                FirstArraySlice = sliceindex,
                MipLevels = 1,
                MostDetailedMip = 0
            };

            this.SRV = new ShaderResourceView(context.Device, this.parent.Resource, srvd);
        }

        public RenderTargetView RTV { get; protected set; }

        public override void Dispose()
        {
            this.SRV.Dispose();
            this.RTV.Dispose();
        }
    }
}
