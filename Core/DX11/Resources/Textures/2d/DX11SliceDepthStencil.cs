using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SlimDX.Direct3D11;
using SlimDX.DXGI;

namespace FeralTic.DX11.Resources
{
    public class DX11SliceDepthStencil : IDX11DepthStencil, IDisposable
    {
        private DX11Texture2D parent;
        private DX11RenderContext context;

        public DepthStencilView DSV { get; protected set; }
        public DepthStencilView ReadOnlyDSV { get { return null; } }

        public int Width { get { return this.parent.Width; } }
        public int Height { get { return this.parent.Height; } }

        public DX11SliceDepthStencil(DX11RenderContext context, DX11Texture2D texture, int sliceindex, Format depthformat)
        {
            this.context = context;
            this.parent = texture;

            DepthStencilViewDescription dsvd = new DepthStencilViewDescription()
            {
                ArraySize = 1,
                Dimension = DepthStencilViewDimension.Texture2DArray,
                Format = depthformat,
                MipSlice = 0,
                FirstArraySlice = sliceindex
            };

            this.DSV = new DepthStencilView(context.Device, this.parent.Resource, dsvd);
        }

        public void Dispose()
        {
            this.DSV.Dispose();
        }



    }
}
