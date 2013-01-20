using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SlimDX.Direct3D11;

using FeralTic.DX11.Resources;

namespace FeralTic.Resources.Geometry
{
    public class DX11NullIndirectDispatcher : IDX11GeometryDrawer<DX11NullGeometry>
    {
        private DX11NullGeometry geom;
        public DispatchIndirectBuffer IndirectArgs { get; set; }


        public void PrepareInputAssembler(DeviceContext ctx, InputLayout layout)
        {

        }

        public void Draw(DeviceContext ctx)
        {
            //ctx.ComputeShader.
            ctx.DispatchIndirect(this.IndirectArgs.Buffer, 0);
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
