using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SlimDX.Direct3D11;

namespace FeralTic.DX11.Resources
{
    public class DX11DefaultVertexDrawer : IDX11GeometryDrawer<DX11VertexGeometry>
    {
        protected DX11VertexGeometry geom;

        public void Assign(DX11VertexGeometry geometry)
        {
            this.geom = geometry;
        }

        public void PrepareInputAssembler(DeviceContext ctx, InputLayout layout)
        {
            ctx.InputAssembler.InputLayout = layout;
            ctx.InputAssembler.SetVertexBuffers(0, new VertexBufferBinding(this.geom.VertexBuffer, this.geom.VertexSize, 0));
        }

        public virtual void Draw(DeviceContext ctx)
        {
            ctx.Draw(this.geom.VerticesCount, 0);
        }
    }
}
