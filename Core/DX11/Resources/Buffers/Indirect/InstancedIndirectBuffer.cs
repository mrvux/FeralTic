using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SlimDX.Direct3D11;


namespace FeralTic.DX11.Resources
{
    public class InstancedIndirectBuffer : BaseIndirectBuffer<DrawInstancedArgs>
    {
        public InstancedIndirectBuffer(DX11RenderContext context, DrawInstancedArgs args) : base(context, args) { }

        public InstancedIndirectBuffer(DX11RenderContext context) : this(context, new DrawInstancedArgs(1, 1, 0, 0)) { }

        public void CopyInstanceCount(DeviceContext ctx, UnorderedAccessView uav)
        {
            ctx.CopyStructureCount(uav, this.Buffer, 4);
        }

        public void CopyVertexCount(DeviceContext ctx, UnorderedAccessView uav)
        {
            ctx.CopyStructureCount(uav, this.Buffer, 0);
        }
    }
}
