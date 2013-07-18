using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SlimDX.Direct3D11;
using FeralTic.DX11.Resources;
using SlimDX;



namespace FeralTic.DX11.Geometry
{
    public partial class DX11PrimitivesManager
    {
        public DX11VertexGeometry LineStrip3d(List<Vector3> points, bool loop)
        {
            DX11VertexGeometry geom = new DX11VertexGeometry(context);

            int vcount = loop ? points.Count + 1 : points.Count;

            Pos3Tex2Vertex[] verts = new Pos3Tex2Vertex[vcount];

            float inc = loop ? 1.0f / (float)vcount : 1.0f / ((float)vcount + 1.0f);

            float curr = 0.0f;
            
                
            for (int i = 0; i < points.Count; i++)
            {
                verts[i].Position = points[i];
                verts[i].TexCoords.X = curr;
                curr += inc;
            }

            if (loop)
            {
                verts[points.Count].Position = points[0];
                verts[points.Count].TexCoords.X = 1.0f;
            }


            DataStream ds = new DataStream(vcount * Pos3Tex2Vertex.VertexSize, true, true);
            ds.Position = 0;
            ds.WriteRange(verts);
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

            geom.VertexBuffer = vbuffer;
            geom.InputLayout = Pos3Tex2Vertex.Layout;
            geom.Topology = PrimitiveTopology.LineStrip;
            geom.VerticesCount = vcount;
            geom.VertexSize = Pos3Tex2Vertex.VertexSize;

            geom.HasBoundingBox = false;

            return geom;
        }

        public DX11VertexGeometry LineStrip3d(List<Vector3> points,List<Vector3> directions, bool loop)
        {
            //Use direction verctor as normal, useful when we have analytical derivatives for direction
            DX11VertexGeometry geom = new DX11VertexGeometry(context);

            int ptcnt = Math.Max(points.Count, directions.Count);

            int vcount = loop ? ptcnt + 1 : ptcnt;

            Pos3Norm3Tex2Vertex[] verts = new Pos3Norm3Tex2Vertex[vcount];

            float inc = loop ? 1.0f / (float)vcount : 1.0f / ((float)vcount + 1.0f);

            float curr = 0.0f;


            for (int i = 0; i < ptcnt; i++)
            {
                verts[i].Position = points[i % points.Count];
                verts[i].Normals = directions[i % directions.Count];
                verts[i].TexCoords.X = curr;
                curr += inc;
            }

            if (loop)
            {
                verts[ptcnt].Position = points[0];
                verts[ptcnt].Normals = directions[0];
                verts[ptcnt].TexCoords.X = 1.0f;
            }


            DataStream ds = new DataStream(vcount * Pos3Tex2Vertex.VertexSize, true, true);
            ds.Position = 0;
            ds.WriteRange(verts);
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

            geom.VertexBuffer = vbuffer;
            geom.InputLayout = Pos3Norm3Tex2Vertex.Layout;
            geom.Topology = PrimitiveTopology.LineStrip;
            geom.VerticesCount = vcount;
            geom.VertexSize = Pos3Norm3Tex2Vertex.VertexSize;

            geom.HasBoundingBox = false;

            return geom;
        }

    }
}
