using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Buffer = SlimDX.Direct3D11.Buffer;

using SlimDX;
using SlimDX.Direct3D11;


using FeralTic.Resources.Geometry;
using System.Runtime.InteropServices;

namespace FeralTic.DX11.Resources
{
    public class DX11IndexedGeometry : DX11BaseGeometry
    {
        private IDX11GeometryDrawer<DX11IndexedGeometry> drawer;

        public IDX11GeometryDrawer<DX11IndexedGeometry> Drawer { get { return this.drawer; } }

        public Buffer VertexBuffer { get; set; }
        public int VerticesCount { get; set; }
        public int VertexSize { get; set; }
        public DX11IndexBuffer IndexBuffer { get; set; }

        private bool ownsvbo;
        private bool ownsido;

        public DX11IndexedGeometry(DX11RenderContext context)
            : base(context)
        {
            this.drawer = new DX11DefaultIndexedDrawer();
            this.drawer.Assign(this);

            this.ownsvbo = true;
            this.ownsido = true;
            //this.LargeIndexFormat = true;
        }

        public static DX11IndexedGeometry CreateFrom<T>(DX11RenderContext context, T[] vertices, int[] indices, InputElement[] layout) where T : struct
        {
            int vertexsize = Marshal.SizeOf(typeof(T));
            DX11IndexedGeometry geom = new DX11IndexedGeometry(context);

            DataStream ds = new DataStream(vertices.Length * vertexsize, true, true);
            ds.Position = 0;
            ds.WriteRange(vertices);
            ds.Position = 0;

            var vbuffer = new SlimDX.Direct3D11.Buffer(context.Device, ds, new BufferDescription()
            {
                BindFlags = BindFlags.VertexBuffer,
                CpuAccessFlags = CpuAccessFlags.None,
                OptionFlags = ResourceOptionFlags.None,
                SizeInBytes = (int)ds.Length,
                Usage = ResourceUsage.Default
            });

            ds.Dispose();


            var indexstream = new DataStream(indices.Length * 4, true, true);
            indexstream.WriteRange(indices);
            indexstream.Position = 0;

            geom.VertexBuffer = vbuffer;
            geom.IndexBuffer = new DX11IndexBuffer(context, indexstream, false, true);
            geom.InputLayout = layout;
            geom.Topology = PrimitiveTopology.TriangleList;
            geom.VerticesCount = vertices.Length;
            geom.VertexSize = vertexsize;
            geom.HasBoundingBox = false;
           // geom.BoundingBox = new BoundingBox(new Vector3(-radius), new Vector3(radius));

            return geom;
            
        }

        public DataStream LockVertexBuffer()
        {
            return this.context.CurrentDeviceContext.MapSubresource(this.VertexBuffer, MapMode.WriteDiscard, MapFlags.None).Data;
        }

        public void UnlockVertexBuffer()
        {
            this.context.CurrentDeviceContext.UnmapSubresource(this.VertexBuffer, 0);
        }

        public void AssignDrawer(IDX11GeometryDrawer<DX11IndexedGeometry> drawer)
        {
            this.drawer = drawer;
            this.drawer.Assign(this);
        }

        public void Draw(IDX11GeometryDrawer<DX11IndexedGeometry> drawer)
        {
            drawer.Draw(this.context.CurrentDeviceContext);
        }

        public DX11IndexedGeometry(DX11IndexedGeometry owner)
        {
            this.ownsvbo = false;
            this.ownsido = false;

            this.context = owner.context;
            this.drawer = owner.drawer;
            this.IndexBuffer = owner.IndexBuffer;
            this.InputLayout = owner.InputLayout;
            this.Topology = owner.Topology;
            this.VertexBuffer = owner.VertexBuffer;
            this.VertexSize = owner.VertexSize;
            this.VerticesCount = owner.VerticesCount;
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
            if (this.ownsvbo) { if (this.VertexBuffer != null) { this.VertexBuffer.Dispose(); } }
            if (this.ownsido) { if (this.IndexBuffer != null) { this.IndexBuffer.Dispose(); } }
        }

        public override IDX11Geometry ShallowCopy()
        {
            return new DX11IndexedGeometry(this);
        }
    }
}
