using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SlimDX.Direct3D11;
using Buffer = SlimDX.Direct3D11.Buffer;

using FeralTic.DX11.Resources;

namespace FeralTic.Resources.Geometry
{
    public class DX11BufferDispatcher : IDX11GeometryDrawer<DX11NullGeometry>
    {
        private DX11NullGeometry geom;
        public Buffer DispatchBuffer { get; set; }
        public int Offet { get; set; }

        public void PrepareInputAssembler(DeviceContext ctx, InputLayout layout)
        {

        }

        public void Draw(DeviceContext ctx)
        {
            ctx.DispatchIndirect(this.DispatchBuffer, this.Offet);
        }

        public void Dispose()
        {

        }

        public void Assign(DX11NullGeometry geometry)
        {
            this.geom = geometry;
        }
    }
}
