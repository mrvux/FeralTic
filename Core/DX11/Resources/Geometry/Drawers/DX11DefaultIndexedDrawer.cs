using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SlimDX.Direct3D11;

namespace FeralTic.DX11.Resources
{
    public class DX11DefaultIndexedDrawer : IDX11GeometryDrawer<DX11IndexedGeometry>
    {
        protected DX11IndexedGeometry geom;

        public void Assign(DX11IndexedGeometry geometry)
        {
            this.geom = geometry;
        }

        public void PrepareInputAssembler(DeviceContext ctx, InputLayout layout)
        {
            ctx.InputAssembler.InputLayout = layout;
            ctx.InputAssembler.SetVertexBuffers(0, new VertexBufferBinding(this.geom.VertexBuffer, this.geom.VertexSize, 0));
            geom.IndexBuffer.Bind();
        }

        public virtual void Draw(DeviceContext ctx)
        {
            ctx.DrawIndexed(this.geom.IndexBuffer.IndicesCount, 0, 0);
        }
    }

    public class DX11DefaultIndexOnlyDrawer : IDX11GeometryDrawer<DX11IndexOnlyGeometry>
    {
        protected DX11IndexOnlyGeometry geom;

        public void Assign(DX11IndexOnlyGeometry geometry)
        {
            this.geom = geometry;
        }

        public void PrepareInputAssembler(DeviceContext ctx, InputLayout layout)
        {
            VertexBufferBinding vb = new VertexBufferBinding();
            ctx.InputAssembler.SetVertexBuffers(0, vb);
            geom.IndexBuffer.Bind();
        }

        public virtual void Draw(DeviceContext ctx)
        {
            ctx.DrawIndexed(this.geom.IndexBuffer.IndicesCount, 0, 0);
        }
    }
}
