using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SlimDX.Direct3D11;

namespace FeralTic.DX11.Resources
{
    public class DX11TextureCube : DX11Texture2D
    {
        public DX11TextureCube(DX11RenderContext context, Texture2D resource)
        {
            this.context = context;
            this.desc = resource.Description;

            this.Resource = resource;

            ShaderResourceViewDescription srvd = new ShaderResourceViewDescription()
            {
                ArraySize = 6,
                FirstArraySlice = 0,
                Dimension = ShaderResourceViewDimension.TextureCube,
                Format = this.desc.Format,
                MipLevels = 1,
                MostDetailedMip = 0,
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
