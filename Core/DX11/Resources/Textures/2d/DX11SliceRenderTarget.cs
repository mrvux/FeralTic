using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SlimDX.Direct3D11;

namespace FeralTic.DX11.Resources
{
    public class DX11SliceRenderTarget : IDX11RenderTargetView, IDisposable
    {
        private DX11Texture2D parent;
        private DX11RenderContext context;

        public DX11SliceRenderTarget(DX11RenderContext context, DX11Texture2D texture, int sliceindex)
        {
            this.context = context;
            this.parent = texture;

            RenderTargetViewDescription rtd = new RenderTargetViewDescription()
            {
                ArraySize = 1,
                Dimension = RenderTargetViewDimension.Texture2DArray,
                Format = texture.Format,
                MipSlice = 0,
                FirstArraySlice = sliceindex
            };

            this.RTV = new RenderTargetView(context.Device, this.parent.Resource, rtd);
        }

        public RenderTargetView RTV { get; protected set; }

        public int Width { get { return this.parent.Width; } }
        public int Height { get { return this.parent.Height; } }

        public void Dispose()
        {
            this.RTV.Dispose();
        }
    }
}
