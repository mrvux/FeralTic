using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SlimDX.Direct3D11;

namespace FeralTic.DX11.Resources
{
    public class DX11NullIndirectDrawer : DX11NullDrawer
    {
        private InstancedIndirectBuffer indbuffer;

        public InstancedIndirectBuffer IndirectArgs { get { return this.indbuffer; } }

        public void Update(DX11RenderContext context,int defaultvertexcount, int defaultinstancecount)
        {
            if (this.indbuffer != null) { this.indbuffer.Dispose(); }

            DrawInstancedArgs args = new DrawInstancedArgs();
            args.InstanceCount = defaultinstancecount;
            args.StartInstanceLocation = 0;
            args.StartVertexLocation = 0;
            args.VertexCount = defaultvertexcount;

            this.indbuffer = new InstancedIndirectBuffer(context, args);

        }

        public override void Draw(DeviceContext ctx)
        {
            ctx.DrawInstancedIndirect(this.indbuffer.Buffer, 0);
        }

        public void Dispose()
        {
            if (this.indbuffer != null) { this.indbuffer.Dispose(); }
        }
    }
}
