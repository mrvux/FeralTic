using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SlimDX.Direct3D11;

using FeralTic.Resources.Geometry;

namespace FeralTic.DX11.Resources
{
    public class DX11InstancedVertexDrawer : DX11DefaultVertexDrawer, IInstancedDrawer
    {
        public DX11InstancedVertexDrawer()
        {
            this.InstanceCount = 1;
            this.StartInstanceLocation = 0;
        }

        public int InstanceCount { get; set; }
        public int StartInstanceLocation { get; set; }

        public override void Draw(DeviceContext ctx)
        {
            ctx.DrawInstanced(this.geom.VerticesCount, this.InstanceCount, 0, this.StartInstanceLocation);
        }
    }
}
