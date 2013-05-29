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
        public DX11IndexedGeometry Tetrahedron(Tetrahedron settings)
        {
            Vector3 size = settings.Size;

            DX11IndexedGeometry geom = new DX11IndexedGeometry(context);
            geom.Tag = settings;
            geom.PrimitiveType = settings.PrimitiveType;
            geom.VerticesCount = 4;
            geom.InputLayout = Pos3Norm3Tex2Vertex.Layout;
            geom.VertexSize = Pos3Norm3Tex2Vertex.VertexSize;
            geom.Topology = PrimitiveTopology.TriangleList;

            // This is the golden ratio
            float t = (1.0f + (float)Math.Sqrt(5.0f)) / 2.0f;

            DataStream ds = new DataStream(4 * Pos3Norm3Tex2Vertex.VertexSize, false, true);

            Pos3Norm3Tex2Vertex v = new Pos3Norm3Tex2Vertex();

            // TODO fibo 2012-03-21:
	    // should be nice to have the four tetrahedra embedded in a dodecahedron
	    // that's why I just commented the coordinates of the dodecahedron points
            //v.Position = new Vector3(-1, t, 0); v.Normals = v.Position; v.TexCoords = new Vector2(0, 0); ds.Write<Pos3Norm3Tex2Vertex>(v);
            v.Position = new Vector3(1, t, 0); v.Normals = v.Position; v.TexCoords = new Vector2(0, 0); ds.Write<Pos3Norm3Tex2Vertex>(v);
            //v.Position = new Vector3(-1, -t, 0); v.Normals = v.Position; v.TexCoords = new Vector2(0, 0); ds.Write<Pos3Norm3Tex2Vertex>(v);
            //v.Position = new Vector3(1, -t, 0); v.Normals = v.Position; v.TexCoords = new Vector2(0, 0); ds.Write<Pos3Norm3Tex2Vertex>(v);

            //v.Position = new Vector3(0, -1, t); v.Normals = v.Position; v.TexCoords = new Vector2(0, 0); ds.Write<Pos3Norm3Tex2Vertex>(v);
            v.Position = new Vector3(0, 1, t); v.Normals = v.Position; v.TexCoords = new Vector2(0, 0); ds.Write<Pos3Norm3Tex2Vertex>(v);
            //v.Position = new Vector3(0, -1, -t); v.Normals = v.Position; v.TexCoords = new Vector2(0, 0); ds.Write<Pos3Norm3Tex2Vertex>(v);
            //v.Position = new Vector3(0, 1, -t); v.Normals = v.Position; v.TexCoords = new Vector2(0, 0); ds.Write<Pos3Norm3Tex2Vertex>(v);

            //v.Position = new Vector3(t, 0, -1); v.Normals = v.Position; v.TexCoords = new Vector2(0, 0); ds.Write<Pos3Norm3Tex2Vertex>(v);
            v.Position = new Vector3(t, 0, 1); v.Normals = v.Position; v.TexCoords = new Vector2(0, 0); ds.Write<Pos3Norm3Tex2Vertex>(v);
            //v.Position = new Vector3(-t, 0, -1); v.Normals = v.Position; v.TexCoords = new Vector2(0, 0); ds.Write<Pos3Norm3Tex2Vertex>(v);
            //v.Position = new Vector3(-t, 0, 1); v.Normals = v.Position; v.TexCoords = new Vector2(0, 0); ds.Write<Pos3Norm3Tex2Vertex>(v);

            geom.VertexBuffer = BufferHelper.CreateVertexBuffer(context, ds, true);

            var indexstream = new DataStream(12 * 4, true, true);

            int[] inds = new int[] { 
                0,1,2,
                1,2,3,
                2,3,0,
                3,0,1 };

            indexstream.WriteRange(inds);
            
            geom.IndexBuffer = new DX11IndexBuffer(context, indexstream, false, true);

            geom.HasBoundingBox = true;
            geom.BoundingBox = new BoundingBox(new Vector3(-1, -1, -1), new Vector3(1, 1, 1));
            return geom;
        }
    }
}
