using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SlimDX.Direct3D11;

namespace FeralTic.DX11.Resources
{
    public class DX11IndexedIndirectDrawer : IDX11GeometryDrawer<DX11IndexedGeometry>
    {
        private DX11IndexedGeometry geom;
        private IndexedIndirectBuffer indbuffer;

        public IndexedIndirectBuffer IndirectArgs { get { return this.indbuffer; } }

        public void Assign(DX11IndexedGeometry geometry)
        {
            this.geom = geometry;
        }

        public void Update(DX11RenderContext context, int defaultinstancecount)
        {
            if (this.indbuffer != null) { this.indbuffer.Dispose(); }

            DrawIndexedInstancedArgs args = new DrawIndexedInstancedArgs();
            args.BaseVertexLocation = 0;
            args.IndicesCount = this.geom.IndexBuffer.IndicesCount;
            args.InstanceCount = defaultinstancecount;
            args.StartIndexLocation = 0;
            args.StartInstanceLocation = 0;

            this.indbuffer = new IndexedIndirectBuffer(context, args);

        }

        public void PrepareInputAssembler(DeviceContext ctx, InputLayout layout)
        {
            ctx.InputAssembler.InputLayout = layout;
            ctx.InputAssembler.SetVertexBuffers(0, new VertexBufferBinding(this.geom.VertexBuffer, this.geom.VertexSize, 0));
            geom.IndexBuffer.Bind();
        }

        public void Draw(DeviceContext ctx)
        {
            ctx.DrawIndexedInstancedIndirect(this.indbuffer.Buffer, 0);
        }

        public void Dispose()
        {
            if (this.indbuffer != null) { this.indbuffer.Dispose(); }
        }
    }
}
