using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SlimDX.Direct3D11;

namespace FeralTic.DX11.Resources
{
    /// <summary>
    /// Just a no geometry as specified. So will work in cases where 
    /// only SV_VertexId or SV_InstanceId is required
    /// </summary>
    public class DX11NullGeometry : DX11BaseGeometry
    {
        private IDX11GeometryDrawer<DX11NullGeometry> drawer;


        public IDX11GeometryDrawer<DX11NullGeometry> Drawer { get { return this.drawer; } }


        public DX11NullGeometry(DX11RenderContext context) : base(context)
        {
            this.drawer = new DX11NullDrawer();
            this.drawer.Assign(this);

            this.InputLayout = new InputElement[0];
            this.Topology = PrimitiveTopology.PointList;
        }

        public DX11NullGeometry(DX11RenderContext context, int VertexCount)
            : base(context)
        {
            DX11NullDrawer d = new DX11NullDrawer();
            d.VertexCount = VertexCount;
           
            this.drawer = d;
            this.drawer.Assign(this);
            

            this.InputLayout = new InputElement[0];
            this.Topology = PrimitiveTopology.PointList;
        }

        public DX11NullGeometry(DX11RenderContext context, IDX11GeometryDrawer<DX11NullGeometry> drawer)
            : base(context)
        {
            this.drawer = drawer;
            this.drawer.Assign(this);
            this.InputLayout = new InputElement[0];
            this.Topology = PrimitiveTopology.PointList;
        }

        public DX11NullGeometry(DX11NullGeometry owner)
        {
            this.context = owner.context;
            this.drawer = owner.drawer;
            this.InputLayout = owner.InputLayout;
            this.Topology = owner.Topology;
            this.HasBoundingBox = owner.HasBoundingBox;
            this.BoundingBox = owner.BoundingBox;
        }

        public void AssignDrawer(IDX11GeometryDrawer<DX11NullGeometry> drawer)
        {
            this.drawer = drawer;
            this.drawer.Assign(this);
        }

        public override void Bind(DeviceContext ctx, InputLayout layout)
        {
            ctx.InputAssembler.PrimitiveTopology = this.Topology;
            this.drawer.PrepareInputAssembler(ctx, null);
        }

        public override void Bind(InputLayout layout)
        {
            Bind(context.CurrentDeviceContext, null);
        }

        public override void Draw()
        {
            Draw(this.context.CurrentDeviceContext);
        }

        public override void Draw(DeviceContext ctx)
        {
            this.drawer.Draw(ctx);
        }

        public override void Dispose()
        {

        }

        public override IDX11Geometry ShallowCopy()
        {
            return new DX11NullGeometry(this);
        }
    }
}
