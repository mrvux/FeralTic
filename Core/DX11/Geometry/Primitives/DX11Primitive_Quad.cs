using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SlimDX;
using SlimDX.Direct3D11;

using FeralTic.DX11.Resources;
using FeralTic.DX11.Utils;

namespace FeralTic.DX11.Geometry
{
    public partial class DX11PrimitivesManager
    {
        public DX11IndexedGeometry QuadNormals(Quad settings)
        {
            DX11IndexedGeometry geom = new DX11IndexedGeometry(context);
            geom.Tag = settings;
            geom.PrimitiveType = settings.PrimitiveType;

            Vector2 size = settings.Size;

            DataStream ds = new DataStream(4 * Pos4Norm3Tex2Vertex.VertexSize, true, true);
            ds.Position = 0;

            float sx = 0.5f * size.X;
            float sy = 0.5f * size.Y;

            Pos4Norm3Tex2Vertex v = new Pos4Norm3Tex2Vertex();
            v.Position = new Vector4(-sx, sy, 0.0f, 1.0f);
            v.Normals = new Vector3(0, 0, 1);
            v.TexCoords = new Vector2(0, 0);
            ds.Write<Pos4Norm3Tex2Vertex>(v);

            v.Position = new Vector4(sx, sy, 0.0f, 1.0f);
            v.TexCoords = new Vector2(1, 0);
            ds.Write<Pos4Norm3Tex2Vertex>(v);

            v.Position = new Vector4(-sx, -sy, 0.0f, 1.0f);
            v.TexCoords = new Vector2(0, 1);
            ds.Write<Pos4Norm3Tex2Vertex>(v);

            v.Position = new Vector4(sx, -sy, 0.0f, 1.0f);
            v.TexCoords = new Vector2(1, 1);
            ds.Write<Pos4Norm3Tex2Vertex>(v);

            var vertices = BufferHelper.CreateVertexBuffer(context, ds, true);

            var indexstream = new DataStream(24, true, true);
            indexstream.WriteRange(new int[] { 0, 1, 3, 3, 2, 0 });


            geom.VertexBuffer = vertices;
            geom.IndexBuffer = new DX11IndexBuffer(context, indexstream, false, true);
            geom.InputLayout = Pos4Norm3Tex2Vertex.Layout;
            geom.Topology = PrimitiveTopology.TriangleList;
            geom.VerticesCount = 4;
            geom.VertexSize = Pos4Norm3Tex2Vertex.VertexSize;

            geom.HasBoundingBox = true;
            geom.BoundingBox = new BoundingBox(new Vector3(-sx, -sy, 0.0f), new Vector3(sx, sy, 0.0f));

            return geom;
        }

        public DX11IndexedGeometry QuadCross(Quad settings)
        {
            DX11IndexedGeometry geom = new DX11IndexedGeometry(context);
            geom.Tag = settings;
            geom.PrimitiveType = settings.PrimitiveType;

            Vector2 size = settings.Size;

            DataStream ds = new DataStream(8 * Pos4Norm3Tex2Vertex.VertexSize, true, true);
            ds.Position = 0;

            float sx = 0.5f * size.X;
            float sy = 0.5f * size.Y;

            Pos4Norm3Tex2Vertex v = new Pos4Norm3Tex2Vertex();
            v.Position = new Vector4(-sx, sy, 0.0f, 1.0f);
            v.Normals = new Vector3(0, 0, 1);
            v.TexCoords = new Vector2(0, 0);
            ds.Write<Pos4Norm3Tex2Vertex>(v);

            v.Position = new Vector4(sx, sy, 0.0f, 1.0f);
            v.TexCoords = new Vector2(1, 0);
            ds.Write<Pos4Norm3Tex2Vertex>(v);

            v.Position = new Vector4(-sx, -sy, 0.0f, 1.0f);
            v.TexCoords = new Vector2(0, 1);
            ds.Write<Pos4Norm3Tex2Vertex>(v);

            v.Position = new Vector4(sx, -sy, 0.0f, 1.0f);
            v.TexCoords = new Vector2(1, 1);
            ds.Write<Pos4Norm3Tex2Vertex>(v);

            v.Position = new Vector4(-sx, 0.0f, sy, 1.0f);
            v.Normals = new Vector3(0, 1 ,0 );
            v.TexCoords = new Vector2(0, 0);
            ds.Write<Pos4Norm3Tex2Vertex>(v);

            v.Position = new Vector4(sx,  0.0f,sy, 1.0f);
            v.TexCoords = new Vector2(1, 0);
            ds.Write<Pos4Norm3Tex2Vertex>(v);

            v.Position = new Vector4(-sx, 0.0f, -sy, 1.0f);
            v.TexCoords = new Vector2(0, 1);
            ds.Write<Pos4Norm3Tex2Vertex>(v);

            v.Position = new Vector4(sx, 0.0f, -sy, 1.0f);
            v.TexCoords = new Vector2(1, 1);
            ds.Write<Pos4Norm3Tex2Vertex>(v);

            var vertices = BufferHelper.CreateVertexBuffer(context, ds, true);

            var indexstream = new DataStream(48, true, true);
            indexstream.WriteRange(new int[] { 0, 1, 3, 3, 2, 0, 4, 5,6,6,5,4 });


            geom.VertexBuffer = vertices;
            geom.IndexBuffer = new DX11IndexBuffer(context, indexstream, false, true);
            geom.InputLayout = Pos4Norm3Tex2Vertex.Layout;
            geom.Topology = PrimitiveTopology.TriangleList;
            geom.VerticesCount = 8;
            geom.VertexSize = Pos4Norm3Tex2Vertex.VertexSize;

            geom.HasBoundingBox = true;
            geom.BoundingBox = new BoundingBox(new Vector3(-sx, -sy, 0.0f), new Vector3(sx, sy, 0.0f));

            return geom;
        }

        public DX11IndexedGeometry QuadColor(Vector2 size)
        {
            DX11IndexedGeometry geom = new DX11IndexedGeometry(context);

            DataStream ds = new DataStream(4 * Pos4Col4Tex2Vertex.VertexSize, true, true);
            ds.Position = 0;

            float sx = 0.5f * size.X;
            float sy = 0.5f * size.Y;

            Pos4Col4Tex2Vertex v = new Pos4Col4Tex2Vertex();
            v.Position = new Vector4(-sx, sy, 0.0f, 1.0f);
            v.Color = new Vector4(1, 1, 1, 1);
            v.TexCoords = new Vector2(0, 0);
            ds.Write<Pos4Col4Tex2Vertex>(v);

            v.Position = new Vector4(sx, sy, 0.0f, 1.0f);
            v.TexCoords = new Vector2(1, 0);
            ds.Write<Pos4Col4Tex2Vertex>(v);

            v.Position = new Vector4(-sx, -sy, 0.0f, 1.0f);
            v.TexCoords = new Vector2(0, 1);
            ds.Write<Pos4Col4Tex2Vertex>(v);

            v.Position = new Vector4(sx, -sy, 0.0f, 1.0f);
            v.TexCoords = new Vector2(1, 1);
            ds.Write<Pos4Col4Tex2Vertex>(v);

            var vertices = BufferHelper.CreateDynamicVertexBuffer(context, ds, true);

            var indexstream = new DataStream(24, true, true);
            indexstream.WriteRange(new int[] { 0, 1, 3, 3, 2, 0 });


            geom.VertexBuffer = vertices;
            geom.IndexBuffer = new DX11IndexBuffer(context, indexstream, false, true);
            geom.InputLayout = Pos4Col4Tex2Vertex.Layout;
            geom.Topology = PrimitiveTopology.TriangleList;
            geom.VerticesCount = 4;
            geom.VertexSize = Pos4Col4Tex2Vertex.VertexSize;

            geom.HasBoundingBox = true;
            geom.BoundingBox = new BoundingBox(new Vector3(-sx, -sy, 0.0f), new Vector3(sx, sy, 0.0f));

            return geom;
        }

        private DX11IndexedGeometry QuadTextured()
        {
            DX11IndexedGeometry geom = new DX11IndexedGeometry(this.context);

            DataStream ds = new DataStream(4 * Pos4Tex2Vertex.VertexSize, true, true);
            ds.Position = 0;

            float sx = 1.0f;
            float sy = 1.0f;

            Pos4Tex2Vertex v = new Pos4Tex2Vertex();
            v.Position = new Vector4(-sx, sy, 0.0f, 1.0f);
            v.TexCoords = new Vector2(0, 0);
            ds.Write<Pos4Tex2Vertex>(v);

            v.Position = new Vector4(sx, sy, 0.0f, 1.0f);
            v.TexCoords = new Vector2(1, 0);
            ds.Write<Pos4Tex2Vertex>(v);

            v.Position = new Vector4(-sx, -sy, 0.0f, 1.0f);
            v.TexCoords = new Vector2(0, 1);
            ds.Write<Pos4Tex2Vertex>(v);

            v.Position = new Vector4(sx, -sy, 0.0f, 1.0f);
            v.TexCoords = new Vector2(1, 1);
            ds.Write<Pos4Tex2Vertex>(v);

            ds.Position = 0;

            var vertices = new SlimDX.Direct3D11.Buffer(context.Device, ds, new BufferDescription()
            {
                BindFlags = BindFlags.VertexBuffer,
                CpuAccessFlags = CpuAccessFlags.None,
                OptionFlags = ResourceOptionFlags.None,
                SizeInBytes = (int)ds.Length,
                Usage = ResourceUsage.Default
            });

            ds.Dispose();

            var indexstream = new DataStream(24, true, true);
            indexstream.WriteRange(new int[] { 0, 1, 3, 3, 2, 0 });
            indexstream.Position = 0;

            geom.VertexBuffer = vertices;
            geom.IndexBuffer = new DX11IndexBuffer(context, indexstream, false, true);


            geom.InputLayout = Pos4Tex2Vertex.Layout;
            geom.Topology = PrimitiveTopology.TriangleList;
            geom.VerticesCount = 4;
            geom.VertexSize = Pos4Tex2Vertex.VertexSize;

            geom.HasBoundingBox = true;
            geom.BoundingBox = new BoundingBox(new Vector3(-sx, -sy, 0.0f), new Vector3(sx, sy, 0.0f));

            return geom;
        }
    }
}
