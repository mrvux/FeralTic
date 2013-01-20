using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SlimDX.Direct3D11;

namespace FeralTic.DX11.Resources
{
    public class DX11NullDrawer : IDX11GeometryDrawer<DX11NullGeometry>
    {
        private DX11NullGeometry geom;

        public int VertexCount { get; set; }

        public DX11NullDrawer()
        {
            this.VertexCount = 1;
        }
        
        public void Assign(DX11NullGeometry geometry)
        {
            this.geom = geometry;
        }

        public void PrepareInputAssembler(DeviceContext ctx, InputLayout layout)
        {
            ctx.InputAssembler.InputLayout = null;
            ctx.InputAssembler.SetIndexBuffer(null, SlimDX.DXGI.Format.Unknown, 0);
            VertexBufferBinding vb = new VertexBufferBinding();
            ctx.InputAssembler.SetVertexBuffers(0, vb);
        }

        public virtual void Draw(DeviceContext ctx)
        {
            ctx.Draw(this.VertexCount,0);
        }
    }
}
