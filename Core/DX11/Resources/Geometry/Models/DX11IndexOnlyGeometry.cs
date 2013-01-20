using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SlimDX.Direct3D11;

using Buffer = SlimDX.Direct3D11.Buffer;

namespace FeralTic.DX11.Resources
{
    public class DX11IndexOnlyGeometry : DX11BaseGeometry
    {
        public DX11IndexBuffer IndexBuffer { get; set; }

        public bool LargeIndexFormat { get; set; }

        private bool ownsido;

        //public IndexedIndirectBuffer IndirectDrawer { get; set; }

        private IDX11GeometryDrawer<DX11IndexOnlyGeometry> drawer;

        public IDX11GeometryDrawer<DX11IndexOnlyGeometry> Drawer { get { return this.drawer; } }

        public void AssignDrawer(IDX11GeometryDrawer<DX11IndexOnlyGeometry> drawer)
        {
            this.drawer = drawer;
            this.drawer.Assign(this);
        }


        public DX11IndexOnlyGeometry(DX11RenderContext context)
            : base(context)
        {
            this.drawer = new DX11DefaultIndexOnlyDrawer();
            this.drawer.Assign(this);

            this.ownsido = true;
            this.LargeIndexFormat = true;
            this.InputLayout = new InputElement[0];
            this.Topology = PrimitiveTopology.TriangleList;
        }

        internal DX11IndexOnlyGeometry(DX11IndexOnlyGeometry owner)
        {
            this.ownsido = false;

            this.context = owner.context;
            this.drawer = owner.drawer;
            this.InputLayout = owner.InputLayout;
            this.Topology = owner.Topology;
            this.IndexBuffer = owner.IndexBuffer;
            this.HasBoundingBox = owner.HasBoundingBox;
            this.BoundingBox = owner.BoundingBox;
        }

        public override void Draw()
        {
            this.drawer.Draw(this.context.CurrentDeviceContext);
        }

        public override void Bind(InputLayout layout)
        {
            this.context.CurrentDeviceContext.InputAssembler.PrimitiveTopology = this.Topology;
            this.drawer.PrepareInputAssembler(this.context.CurrentDeviceContext, layout);
        }

        public override void Dispose()
        {
            if (this.ownsido) { if (this.IndexBuffer != null) { this.IndexBuffer.Dispose(); } }
        }

        public override IDX11Geometry ShallowCopy()
        {
            return new DX11IndexOnlyGeometry(this);
        }
    }
}
