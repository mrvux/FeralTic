using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SlimDX.Direct3D11;

namespace FeralTic.DX11.Resources
{
    public class DX11NullDispatcher : IDX11GeometryDrawer<DX11NullGeometry>
    {
        private DX11NullGeometry geom;

        public int X { get; set; }
        public int Y { get; set; }
        public int Z { get; set; }

        public DX11NullDispatcher()
        {
            this.X = 1;
            this.Y = 1;
            this.Z = 1;
        }

        public void Assign(DX11NullGeometry geometry)
        {
            this.geom = geometry;
        }

        public void PrepareInputAssembler(DeviceContext ctx, InputLayout layout)
        {

        }

        public void Draw(DeviceContext ctx)
        {
            //ctx.ComputeShader.
            ctx.Dispatch(X, Y, Z);
        }
    }
}
