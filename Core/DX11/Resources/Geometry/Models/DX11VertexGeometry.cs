using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SlimDX.Direct3D11;

using Buffer = SlimDX.Direct3D11.Buffer;


namespace FeralTic.DX11.Resources
{
    /// <summary>
    /// Geometry with a single vertex buffer
    /// </summary>
    public class DX11VertexGeometry : DX11BaseGeometry
    {    
        private IDX11GeometryDrawer<DX11VertexGeometry> drawer;
        private bool ownsvbo;

        public IDX11GeometryDrawer<DX11VertexGeometry> Drawer { get { return this.drawer; } }

        public DX11VertexGeometry(DX11RenderContext context)
            : base(context)
        {
            this.drawer = new DX11DefaultVertexDrawer();
            this.drawer.Assign(this);
            this.ownsvbo = true;
        }

        public DX11VertexGeometry(DX11VertexGeometry owner)
        {
            this.ownsvbo = false;

            this.context = owner.context;
            this.drawer = owner.drawer;
            this.InputLayout = owner.InputLayout;
            this.Topology = owner.Topology;
            this.VertexBuffer = owner.VertexBuffer;
            this.VertexSize = owner.VertexSize;
            this.VerticesCount = owner.VerticesCount;
            this.HasBoundingBox = owner.HasBoundingBox;
            this.BoundingBox = owner.BoundingBox;
        }


        public void AssignDrawer(IDX11GeometryDrawer<DX11VertexGeometry> drawer)
        {
            this.drawer = drawer;
            this.drawer.Assign(this);
        }

        public Buffer VertexBuffer { get; set; }
        public int VerticesCount { get; set; }
        public int VertexSize { get; set; }

        public override void Draw()
        {
            Draw(this.context.CurrentDeviceContext);
        }

        public override void Draw(DeviceContext ctx)
        {
            this.drawer.Draw(ctx);
        }

        public override void Bind(DeviceContext ctx, InputLayout layout)
        {
            ctx.InputAssembler.PrimitiveTopology = this.Topology;
            this.drawer.PrepareInputAssembler(ctx, layout);
        }

        public override void Bind(InputLayout layout)
        {
            Bind(context.CurrentDeviceContext, layout);
        }
        public override void Dispose()
        {
            if (this.ownsvbo)
            {
                if (this.VertexBuffer != null) { this.VertexBuffer.Dispose(); }
            }
        }

        public override IDX11Geometry ShallowCopy()
        {
            return new DX11VertexGeometry(this);
        }


        public static DX11VertexGeometry StreamOut(DX11RenderContext context, int vertexCount, int vertexSize, bool autoDrawer)
        {
            DX11VertexBuffer vertexBuffer = DX11VertexBuffer.CreateStreamOutput(context, vertexCount, vertexSize, false);
            var vg = new DX11VertexGeometry(context);

            if (autoDrawer)
            {
                vg.drawer = new DX11VertexAutoDrawer();
            }
            vg.ownsvbo = true;

            vg.VertexBuffer = vertexBuffer.Buffer;
            vg.VerticesCount = vertexBuffer.VertexCount;
            vg.VertexSize = vertexBuffer.VertexSize;
            vg.HasBoundingBox = false;
            vg.PrimitiveType = "StreamOut";
            vg.Topology = PrimitiveTopology.TriangleList;
            return vg;
        }
    }
}
