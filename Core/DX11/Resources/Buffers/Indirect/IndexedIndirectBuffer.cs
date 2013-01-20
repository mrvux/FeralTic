using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SlimDX.Direct3D11;

namespace FeralTic.DX11.Resources
{
    public class IndexedIndirectBuffer : BaseIndirectBuffer<DrawIndexedInstancedArgs>
    {
        public IndexedIndirectBuffer(DX11RenderContext context, DrawIndexedInstancedArgs args) : base(context, args) { }

        public IndexedIndirectBuffer(DX11RenderContext context) : this(context, new DrawIndexedInstancedArgs(1, 1, 0, 0, 0)) { }

        public void CopyInstanceCount(DeviceContext ctx, UnorderedAccessView uav)
        {
            ctx.CopyStructureCount(uav, this.Buffer, 4);
        }

        public void CopyIndicesCount(DeviceContext ctx, UnorderedAccessView uav)
        {
            ctx.CopyStructureCount(uav, this.Buffer, 0);
        }
    }
}
