using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SlimDX.Direct3D11;

namespace FeralTic.DX11.Resources
{
    public class DX11MipSliceRenderTarget : IDX11RenderTargetView, IDisposable
    {
        private DX11RenderContext context;

        public DX11MipSliceRenderTarget(DX11RenderContext context, DX11Texture2D texture, int mipindex, int w, int h)
        {
            this.context = context;
            this.Width = w;
            this.Height = h;
            this.Depth = 1;

            RenderTargetViewDescription rtd = new RenderTargetViewDescription()
            {
                Dimension = RenderTargetViewDimension.Texture2D,
                Format = texture.Format,
                MipSlice = mipindex
            };

            ShaderResourceViewDescription srvd = new ShaderResourceViewDescription();
            srvd.Dimension = ShaderResourceViewDimension.Texture2D;
            srvd.MipLevels = 1;
            srvd.MostDetailedMip = mipindex;
            srvd.Format = texture.Format;


            this.RTV = new RenderTargetView(context.Device, texture.Resource, rtd);
            this.SRV = new ShaderResourceView(context.Device, texture.Resource, srvd);
            
        }

        public DX11MipSliceRenderTarget(DX11RenderContext context, DX11Texture3D texture, int mipindex,int w, int h,int d)
        {
            this.context = context;

            RenderTargetViewDescription rtd = new RenderTargetViewDescription()
            {
                Dimension = RenderTargetViewDimension.Texture3D,
                Format = texture.Format,
                MipSlice = mipindex,
                DepthSliceCount = d           
            };

            ShaderResourceViewDescription srvd = new ShaderResourceViewDescription();
            srvd.Dimension = ShaderResourceViewDimension.Texture3D;
            srvd.MipLevels = 1;
            srvd.MostDetailedMip = mipindex;
            srvd.Format = texture.Format;

            this.RTV = new RenderTargetView(context.Device, texture.Resource, rtd);
            this.SRV = new ShaderResourceView(context.Device, texture.Resource, srvd);

            this.Width = w;
            this.Height = h;
            this.Depth = d;
        }

        public RenderTargetView RTV { get; protected set; }
        public ShaderResourceView SRV { get; protected set; }

        public int Width { get; protected set; }
        public int Height { get; protected set; }
        public int Depth { get; protected set; }

        public void Dispose()
        {
            this.RTV.Dispose();
            this.SRV.Dispose();
        }
    }
}
