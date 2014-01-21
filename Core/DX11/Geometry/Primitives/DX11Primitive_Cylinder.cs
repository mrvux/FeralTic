using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SlimDX;
using SlimDX.Direct3D11;

using FeralTic.DX11.Resources;

namespace FeralTic.DX11.Geometry
{
    public partial class DX11PrimitivesManager
    {
        public DX11IndexedGeometry Cylinder(float radius1, float radius2, float cycles, float length, int resX, int resY, bool caps)
        {
            DX11IndexedGeometry geom = new DX11IndexedGeometry(context);
            int vcount = resX * (resY + 1);
            int icount = vcount * 6;

            float lenstart = -length * 0.5f; //Start at half bottom
            float lenstep = (float)length / (float)resY;

            float y = lenstart;

            List<Pos4Norm3Tex2Vertex> verts = new List<Pos4Norm3Tex2Vertex>();
            List<int> inds = new List<int>();

            float phi = 0.0f;
            float inc = Convert.ToSingle((Math.PI * 2.0 * cycles) / (double)resX);

            float fres = Convert.ToSingle(resY);

            for (int i = 0; i < resY + 1; i++)
            {

                float ystep = (float)i / fres;

                float radius = Map(ystep, 0, 1, radius1, radius2);

                for (int j = 0; j < resX; j++)
                {
                    float x = Convert.ToSingle(radius1 * Math.Cos(phi)) * radius;
                    float z = Convert.ToSingle(radius1 * Math.Sin(phi)) * radius;

                    Pos4Norm3Tex2Vertex v = new Pos4Norm3Tex2Vertex();
                    v.Position = new Vector4(x, y, z, 1.0f);
                    v.Normals = new Vector3(x, 0.0f, z);
                    v.Normals.Normalize();

                    verts.Add(v);

                    phi += inc;
                }
                y += lenstep;
                phi = 0.0f;
            }

            int indstart;
            for (int i = 0; i < resY; i++)
            {
                indstart = resX * i;
                int j;
                for (j = 0; j < resX - 1; j++)
                {
                    inds.Add(indstart + j);
                    inds.Add(indstart + j + 1);
                    inds.Add(indstart + resX + j);

                    inds.Add(indstart + j + 1);
                    inds.Add(indstart + j + resX + 1);
                    inds.Add(indstart + j + resX);
                }

                inds.Add(indstart + j);
                inds.Add(indstart);
                inds.Add(indstart + resX + j);

                inds.Add(indstart);
                inds.Add(indstart + resX);
                inds.Add(indstart + j + resX);
            }

            DataStream ds = new DataStream(vcount * Pos4Norm3Tex2Vertex.VertexSize, true, true);
            ds.Position = 0;
            ds.WriteRange(verts.ToArray());
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

            var indexstream = new DataStream(inds.Count * 4, true, true);
            indexstream.WriteRange(inds.ToArray());
            indexstream.Position = 0;

            geom.VertexBuffer = vbuffer;
            geom.IndexBuffer = new DX11IndexBuffer(context, indexstream, false, true);
            geom.InputLayout = Pos4Norm3Tex2Vertex.Layout;
            geom.Topology = PrimitiveTopology.TriangleList;
            geom.VerticesCount = vcount;
            geom.VertexSize = Pos4Norm3Tex2Vertex.VertexSize;

            //Since cylinder can be a cone, max box is max of radius
            float maxrad = radius1 > radius2 ? radius1 : radius2;

            geom.HasBoundingBox = true;
            geom.BoundingBox = new BoundingBox(new Vector3(-maxrad, -lenstart, -maxrad), new Vector3(maxrad, lenstart, maxrad));

            return geom;
        }
    }
}
