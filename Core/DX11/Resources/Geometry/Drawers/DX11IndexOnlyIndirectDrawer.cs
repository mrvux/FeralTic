using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SlimDX.Direct3D11;

namespace FeralTic.DX11.Resources
{
    public class DX11IndexOnlyIndirectDrawer : IDX11GeometryDrawer<DX11IndexOnlyGeometry>
    {
        private DX11IndexOnlyGeometry geom;
        public IndexedIndirectBuffer IndirectArgs { get; set; }

        public void Assign(DX11IndexOnlyGeometry geometry)
        {
            this.geom = geometry;
        }

        public void Update(DX11RenderContext context, int defaultinstancecount)
        {
            /*if (this.indbuffer != null) { this.indbuffer.Dispose(); }

            DrawIndexedInstancedArgs args = new DrawIndexedInstancedArgs();
            args.BaseVertexLocation = 0;
            args.IndicesCount = this.geom.IndexBuffer.IndicesCount;
            args.InstanceCount = defaultinstancecount;
            args.StartIndexLocation = 0;
            args.StartInstanceLocation = 0;

            this.indbuffer = new IndexedIndirectBuffer(context, args);*/

        }

        public void PrepareInputAssembler(DeviceContext ctx, InputLayout layout)
        {
            ctx.InputAssembler.InputLayout = null;
            VertexBufferBinding vb = new VertexBufferBinding();
            ctx.InputAssembler.SetVertexBuffers(0, vb);
            this.geom.IndexBuffer.Bind();
        }

        public void Draw(DeviceContext ctx)
        {
            ctx.DrawIndexedInstancedIndirect(this.IndirectArgs.Buffer, 0);
        }

        public void Dispose()
        {
            if (this.IndirectArgs != null) { this.IndirectArgs.Dispose(); }
        }
    }
}
