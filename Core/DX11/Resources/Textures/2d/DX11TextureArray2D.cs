using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SlimDX.Direct3D11;

namespace FeralTic.DX11.Resources
{
    public class DX11TextureArray2D : DX11Texture2D
    {
        public int ElemCnt { get { return desc.ArraySize; } }


        public DX11TextureArray2D(DX11RenderContext context, Texture2D resource)
        {
            this.context = context;
            this.desc = resource.Description;

            this.Resource = resource;

            ShaderResourceViewDescription srvd = new ShaderResourceViewDescription()
            {
                ArraySize = this.desc.ArraySize,
                FirstArraySlice = 0,
                Dimension = ShaderResourceViewDimension.Texture2DArray,
                Format = this.desc.Format,
                MipLevels = 1,
                MostDetailedMip = 0
            };

            this.SRV = new ShaderResourceView(context.Device, this.Resource, srvd);

        }

        public override void Dispose()
        {
            this.SRV.Dispose();
            this.Resource.Dispose();
        }
    }
}
