using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FeralTic.DX11.Resources
{
    public class DX11OwnedTexture3D : DX11Texture3D
    {
        public DX11OwnedTexture3D(DX11RenderContext context) : base(context) { }

        public override void Dispose()
        {
            base.Dispose();
            if (this.SRV != null)
            {
                this.SRV.Dispose();
                this.SRV = null;
            }
            if (this.Resource != null)
            {
                this.Resource.Dispose();
                this.Resource = null;
            }
        }
    }
}
