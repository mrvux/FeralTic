using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SlimDX.Direct3D11;


namespace FeralTic.DX11.Resources
{
    public class DispatchIndirectBuffer : BaseIndirectBuffer<DispatchArgs>
    {
        public DispatchIndirectBuffer(DX11RenderContext context, DispatchArgs args) : base(context, args) { }

        public DispatchIndirectBuffer(DX11RenderContext context) : this(context, new DispatchArgs(1, 1, 1)) { }

    }
}
