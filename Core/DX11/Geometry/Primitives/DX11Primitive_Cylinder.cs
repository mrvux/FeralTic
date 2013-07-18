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
        public DX11IndexedGeometry Cylinder(Cylinder settings)
        {
            float radius1 = settings.Radius1;
            float radius2 = settings.Radius2;
            float cycles = settings.Cycles;
            float length = settings.Length;
            int resX = settings.ResolutionX;
            int resY = settings.ResolutionY;
            bool caps = settings.Caps;

            DX11IndexedGeometry geom = new DX11IndexedGeometry(context);
            geom.Tag = settings;
            geom.PrimitiveType = settings.PrimitiveType;

            int vcount = resX * (resY + 1);
            int icount = vcount * 6;

            if (caps)
            {
                //Add vertices (+1 for center)
                vcount += (resX + 1 * 2);

                //Add Triangles (on each side
                icount += (resX * 6);
            }

            float lenstart = -length * 0.5f; //Start at half bottom
            float lenstep = (float)length / (float)resY;

            float y = lenstart;

            List<Pos4Norm3Tex2Vertex> verts = new List<Pos4Norm3Tex2Vertex>();
            List<int> inds = new List<int>();

            float phi = 0.0f;
            float inc = Convert.ToSingle((Math.PI * 2.0 * cycles) / (double)resX);

            float fres = Convert.ToSingle(resY);

            #region Add Vertices tube
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
                    v.Normals = new Vector3(x, 0.0f, y);
                    v.Normals.Normalize();

                    verts.Add(v);

                    phi += inc;
                }
                y += lenstep;
                phi = 0.0f;
            }
            #endregion

            #region Add Indices Tube
            int indstart;
            for (int i = 0; i < resY; i++)
            {
                indstart = resX * i;
                int j;
                for (j = 0; j < resX - 1; j++)
                {
                    inds.Add(indstart + j);
                    inds.Add(indstart + resX + j);
                    inds.Add(indstart + j + 1);


                    inds.Add(indstart + j + 1);
                    inds.Add(indstart + j + resX);
                    inds.Add(indstart + j + resX + 1);

                }

                inds.Add(indstart + j);
                inds.Add(indstart + resX + j);
                inds.Add(indstart);

                inds.Add(indstart + j + resX);
                inds.Add(indstart + resX);
                inds.Add(indstart);
            }

            if (caps)
            {
                indstart = verts.Count;
                y = -length * 0.5f;

                Pos4Norm3Tex2Vertex v = new Pos4Norm3Tex2Vertex();
                v.Position = new Vector4(0, y, 0, 1.0f);
                v.Normals = new Vector3(0.0f, -1.0f, 0.0f);
                v.Normals.Normalize();
                verts.Add(v);

                phi = 0.0f;

                for (int j = 0; j < resX; j++)
                {
                    float x = Convert.ToSingle(radius1 * Math.Cos(phi)) * radius1;
                    float z = Convert.ToSingle(radius1 * Math.Sin(phi)) * radius1;

                    v.Position = new Vector4(x, y, z, 1.0f);
                    verts.Add(v);

                    phi += inc;
                }

                for (int j = 1; j < resX + 1; j++)
                {
                    inds.Add(indstart);
                    inds.Add(indstart + j);

                    if (j == resX)
                    {
                        inds.Add(indstart + 1);
                    }
                    else
                    {
                        inds.Add(indstart + j + 1);
                    }
                }

                indstart += (resX + 1);
                y = length * 0.5f;

                v = new Pos4Norm3Tex2Vertex();
                v.Position = new Vector4(0, y, 0, 1.0f);
                v.Normals = new Vector3(0.0f, 1.0f, 0.0f);
                v.Normals.Normalize();
                verts.Add(v);

                phi = 0.0f;

                for (int j = 0; j < resX; j++)
                {
                    float x = Convert.ToSingle(radius1 * Math.Cos(phi)) * radius2;
                    float z = Convert.ToSingle(radius1 * Math.Sin(phi)) * radius2;

                    v.Position = new Vector4(x, y, z, 1.0f);
                    verts.Add(v);

                    phi += inc;
                }

                for (int j = 1; j < resX + 1; j++)
                {
                    inds.Add(indstart + j);
                    inds.Add(indstart);


                    if (j == resX)
                    {
                        inds.Add(indstart + 1);
                    }
                    else
                    {
                        inds.Add(indstart + j + 1);
                    }
                }
            }
            #endregion

            DataStream ds = new DataStream(verts.Count * Pos4Norm3Tex2Vertex.VertexSize, true, true);
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
