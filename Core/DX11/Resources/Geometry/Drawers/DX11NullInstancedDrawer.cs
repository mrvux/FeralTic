using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FeralTic.DX11.Resources
{
    public class DX11NullInstancedDrawer : DX11NullDrawer
    {
        public int InstanceCount { get; set; }

        public DX11NullInstancedDrawer() : base()
        {
            this.InstanceCount = 1;
        }

        public override void Draw(SlimDX.Direct3D11.DeviceContext ctx)
        {
            ctx.DrawInstanced(this.VertexCount, this.InstanceCount, 0, 0);
        }
    }
}
