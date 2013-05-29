using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SlimDX.Direct3D11;
using SlimDX;

using FeralTic.DX11.Resources;
using FeralTic.DX11.Utils;


namespace FeralTic.DX11.Geometry
{
    public partial class DX11PrimitivesManager
    {
        public DX11IndexedGeometry Isocahedron(Isocahedron settings)
        {
            DX11IndexedGeometry geom = new DX11IndexedGeometry(context);
            geom.PrimitiveType = settings.PrimitiveType;
            geom.Tag = settings;
            geom.VerticesCount = 12;
            geom.InputLayout = Pos3Norm3Tex2Vertex.Layout;
            geom.VertexSize = Pos3Norm3Tex2Vertex.VertexSize;
            geom.Topology = PrimitiveTopology.TriangleList;

            float t = (1.0f + (float)Math.Sqrt(5.0f)) / 2.0f;

            DataStream ds = new DataStream(12 * Pos3Norm3Tex2Vertex.VertexSize, false, true);

            Pos3Norm3Tex2Vertex v = new Pos3Norm3Tex2Vertex();

            v.Position = Vector3.Normalize(new Vector3(-1, t, 0))*0.5f; v.Normals = v.Position*2.0f; v.TexCoords = new Vector2(0, 0); ds.Write<Pos3Norm3Tex2Vertex>(v);
            v.Position = Vector3.Normalize(new Vector3(1, t, 0)) * 0.5f; v.Normals = v.Position * 2.0f; v.TexCoords = new Vector2(0, 0); ds.Write<Pos3Norm3Tex2Vertex>(v);
            v.Position = Vector3.Normalize(new Vector3(-1, -t, 0)) * 0.5f; v.Normals = v.Position * 2.0f; v.TexCoords = new Vector2(0, 0); ds.Write<Pos3Norm3Tex2Vertex>(v);
            v.Position = Vector3.Normalize(new Vector3(1, -t, 0)) * 0.5f; v.Normals = v.Position * 2.0f; v.TexCoords = new Vector2(0, 0); ds.Write<Pos3Norm3Tex2Vertex>(v);

            v.Position = Vector3.Normalize(new Vector3(0, -1, t)) * 0.5f; v.Normals = v.Position * 2.0f; v.TexCoords = new Vector2(0, 0); ds.Write<Pos3Norm3Tex2Vertex>(v);
            v.Position = Vector3.Normalize(new Vector3(0, 1, t)) * 0.5f; v.Normals = v.Position * 2.0f; v.TexCoords = new Vector2(0, 0); ds.Write<Pos3Norm3Tex2Vertex>(v);
            v.Position = Vector3.Normalize(new Vector3(0, -1, -t)) * 0.5f; v.Normals = v.Position * 2.0f; v.TexCoords = new Vector2(0, 0); ds.Write<Pos3Norm3Tex2Vertex>(v);
            v.Position = Vector3.Normalize(new Vector3(0, 1, -t)) * 0.5f; v.Normals = v.Position * 2.0f; v.TexCoords = new Vector2(0, 0); ds.Write<Pos3Norm3Tex2Vertex>(v);

            v.Position = Vector3.Normalize(new Vector3(t, 0, -1)) * 0.5f; v.Normals = v.Position * 2.0f; v.TexCoords = new Vector2(0, 0); ds.Write<Pos3Norm3Tex2Vertex>(v);
            v.Position = Vector3.Normalize(new Vector3(t, 0, 1)) * 0.5f; v.Normals = v.Position * 2.0f; v.TexCoords = new Vector2(0, 0); ds.Write<Pos3Norm3Tex2Vertex>(v);
            v.Position = Vector3.Normalize(new Vector3(-t, 0, -1)) * 0.5f; v.Normals = v.Position * 2.0f; v.TexCoords = new Vector2(0, 0); ds.Write<Pos3Norm3Tex2Vertex>(v);
            v.Position = Vector3.Normalize(new Vector3(-t, 0, 1)) * 0.5f; v.Normals = v.Position * 2.0f; v.TexCoords = new Vector2(0, 0); ds.Write<Pos3Norm3Tex2Vertex>(v);

            

            geom.VertexBuffer = BufferHelper.CreateVertexBuffer(context, ds, true);

            var indexstream = new DataStream(60 * 4, true, true);

            int[] inds = new int[] { 
                0,11,5,
                0,5,1,
                0,1,7,
                0,7,10,
                0,10,11,
                1,5,9,
                5,11,4,
                11,10,2,
                10,7,6,
                7,1,8,
                3,9,4,
                3,4,2,
                3,2,6,
                3,6,8,
                3,8,9,
                4,9,5,
                2,4,11,
                6,2,10,
                8,6,7,
                9,8,1 };

            indexstream.WriteRange(inds);
            
            geom.IndexBuffer = new DX11IndexBuffer(context, indexstream, false, true);

            geom.HasBoundingBox = true;
            geom.BoundingBox = new BoundingBox(new Vector3(-1, -1, -1), new Vector3(1, 1, 1));
            return geom;
        }
    }
}
