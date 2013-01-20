using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SlimDX.Direct3D11;

namespace FeralTic.DX11.Resources
{
    public class DX11InstancedIndexedDrawer : DX11DefaultIndexedDrawer, IInstancedDrawer
    {
        public DX11InstancedIndexedDrawer()
        {
            this.InstanceCount = 1;
            this.StartInstanceLocation = 0;
            this.BaseVertexLocation = 0;
            this.StartIndexLocation = 0;

        }

        public int InstanceCount { get; set; }
        public int StartIndexLocation { get; set; }
        public int StartInstanceLocation { get; set; }
        public int BaseVertexLocation { get; set; }

        public override void Draw(DeviceContext ctx)
        {
            ctx.DrawIndexedInstanced(this.geom.IndexBuffer.IndicesCount, this.InstanceCount, this.StartIndexLocation, this.BaseVertexLocation, this.StartInstanceLocation);
        }
    }
}
