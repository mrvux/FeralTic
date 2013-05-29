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
        public DX11IndexedGeometry SegmentZ(SegmentZ settings)
        {
            int res = settings.Resolution;
            float cycles = settings.Cycles;
            float phase = settings.Phase;
            float inner = settings.InnerRadius;
            float z = settings.Z;

            DX11IndexedGeometry geom = new DX11IndexedGeometry(context);
            geom.Tag = settings;
            geom.PrimitiveType = "SegmentZ";

            int vcount = res * 2;
            int icount = (res - 1) * 6;

            float inc = Convert.ToSingle((Math.PI * 2.0 * cycles) / (res - 1.0));
            float phi = Convert.ToSingle(phase * (Math.PI * 2.0));

            List<Pos4Norm3Tex2Vertex> vlist = new List<Pos4Norm3Tex2Vertex>();
            List<int> ilist = new List<int>();

            Pos4Norm3Tex2Vertex innerv = new Pos4Norm3Tex2Vertex();
            innerv.Normals = new Vector3(0.0f, 0.0f, 1.0f);

            Pos4Norm3Tex2Vertex outerv = new Pos4Norm3Tex2Vertex();
            outerv.Normals = new Vector3(0.0f, 0.0f, 1.0f);

            Pos4Norm3Tex2Vertex[] vertices = new Pos4Norm3Tex2Vertex[res * 2];

            #region Append front face
            for (int i = 0; i < res; i++)
            {
                float x = Convert.ToSingle(0.5 * inner * Math.Cos(phi));
                float y = Convert.ToSingle(0.5 * inner * Math.Sin(phi));

                innerv.Position = new Vector4(x, y, z, 1.0f);

                x = Convert.ToSingle(0.5 * Math.Cos(phi));
                y = Convert.ToSingle(0.5 * Math.Sin(phi));

                outerv.Position = new Vector4(x, y, z, 1.0f);

                vertices[i] = innerv;
                vertices[i + res] = outerv;
                phi += inc;
            }

            int indstep = 0;
            int[] indices = new int[icount];
            for (int i = 0; i < res - 1; i++)
            {
                //Triangle from low to high
                indices[indstep] = i;
                indices[indstep + 1] = res + i;
                indices[indstep + 2] = i + 1;


                //Triangle from high to low
                indices[indstep + 3] = i + 1;
                indices[indstep + 4] = res + i;
                indices[indstep + 5] = res + i + 1;

                indstep += 6;
            }

            vlist.AddRange(vertices);
            ilist.AddRange(indices);
            #endregion

            #region Append Back Face
            //Second layer just has Z inverted
            for (int i = 0; i < res * 2; i++)
            {
                vertices[i].Position.Z = -vertices[i].Position.Z;
                vertices[i].Normals.Z = -vertices[i].Normals.Z;
                phi += inc;
            }

            //Here we also flip triangles for cull
            indstep = 0;
            int offset = res * 2;
            for (int i = offset; i < offset + res - 1; i++)
            {
                //Triangle from low to high
                indices[indstep] = i;
                indices[indstep + 2] = res + i;
                indices[indstep + 1] = i + 1;


                //Triangle from high to low
                indices[indstep + 3] = i + 1;
                indices[indstep + 5] = res + i;
                indices[indstep + 4] = res + i + 1;

                indstep += 6;
            }

            vlist.AddRange(vertices);
            ilist.AddRange(indices);
            #endregion


            //We need to append new set of indices, as we want nice normals
            #region Append Outer
            phi = Convert.ToSingle(phase * (Math.PI * 2.0));
            for (int i = 0; i < res; i++)
            {
                float x = Convert.ToSingle(0.5 * Math.Cos(phi));
                float y = Convert.ToSingle(0.5 * Math.Sin(phi));

                innerv.Position = new Vector4(x, y, z, 1.0f);
                outerv.Position = new Vector4(x, y, -z, 1.0f);
                innerv.Normals = Vector3.Normalize(new Vector3(innerv.Position.X, innerv.Position.Y, 0.0f));
                outerv.Normals = Vector3.Normalize(new Vector3(innerv.Position.X, innerv.Position.Y, 0.0f));

                vertices[i] = innerv;
                vertices[i + res] = outerv;
                phi += inc;
            }

            indstep = 0;
            offset += (res * 2);
            for (int i = offset; i < offset + res - 1; i++)
            {
                //Triangle from low to high
                indices[indstep] = i;
                indices[indstep + 1] = res + i;
                indices[indstep + 2] = i + 1;


                //Triangle from high to low
                indices[indstep + 3] = i + 1;
                indices[indstep + 4] = res + i;
                indices[indstep + 5] = res + i + 1;

                indstep += 6;
            }

            vlist.AddRange(vertices);
            ilist.AddRange(indices);
            #endregion

            #region Append Inner
            phi = Convert.ToSingle(phase * (Math.PI * 2.0));
            for (int i = 0; i < res; i++)
            {
                float x = Convert.ToSingle(0.5 * inner * Math.Cos(phi));
                float y = Convert.ToSingle(0.5 * inner * Math.Sin(phi));

                innerv.Position = new Vector4(x, y, z, 1.0f);
                outerv.Position = new Vector4(x, y, -z, 1.0f);
                innerv.Normals = -Vector3.Normalize(new Vector3(innerv.Position.X, innerv.Position.Y, 0.0f));
                outerv.Normals = -Vector3.Normalize(new Vector3(innerv.Position.X, innerv.Position.Y, 0.0f));

                vertices[i] = innerv;
                vertices[i + res] = outerv;
                phi += inc;
            }

            indstep = 0;
            offset += (res * 2);
            for (int i = offset; i < offset + res - 1; i++)
            {
                //Triangle from low to high
                indices[indstep] = i;
                indices[indstep + 2] = res + i;
                indices[indstep + 1] = i + 1;


                //Triangle from high to low
                indices[indstep + 3] = i + 1;
                indices[indstep + 5] = res + i;
                indices[indstep + 4] = res + i + 1;

                indstep += 6;
            }

            vlist.AddRange(vertices);
            ilist.AddRange(indices);
            #endregion

            #region Append Border

            //Append border low (quad)
            phi = Convert.ToSingle(phase * (Math.PI * 2.0));
            float x2 = Convert.ToSingle(0.5 * inner * Math.Cos(phi));
            float y2 = Convert.ToSingle(0.5 * inner * Math.Sin(phi));

            float x3 = Convert.ToSingle(0.5 * Math.Cos(phi));
            float y3 = Convert.ToSingle(0.5 * Math.Sin(phi));

            Pos4Norm3Tex2Vertex q1 = new Pos4Norm3Tex2Vertex();
            Pos4Norm3Tex2Vertex q2 = new Pos4Norm3Tex2Vertex();
            Pos4Norm3Tex2Vertex q3 = new Pos4Norm3Tex2Vertex();
            Pos4Norm3Tex2Vertex q4 = new Pos4Norm3Tex2Vertex();

            q1.Position = new Vector4(x2, y2, z, 1.0f);
            q2.Position = new Vector4(x2, y2, -z, 1.0f);
            q3.Position = new Vector4(x3, y3, z, 1.0f);
            q4.Position = new Vector4(x3, y3, -z, 1.0f);

            Vector3 e1 = new Vector3(q2.Position.X - q1.Position.X,
                q2.Position.Y - q1.Position.Y,
                q2.Position.Z - q1.Position.Z);

            Vector3 e2 = new Vector3(q3.Position.X - q2.Position.X,
                q3.Position.Y - q2.Position.Y,
                q3.Position.Z - q2.Position.Z);

            Vector3 n = Vector3.Cross(e1, e2);
            q1.Normals = n;
            q2.Normals = n;
            q3.Normals = n;
            q4.Normals = n;

            vlist.Add(q1); vlist.Add(q2); vlist.Add(q3); vlist.Add(q4);

            offset += (res * 2);
            ilist.Add(offset); ilist.Add(offset + 1); ilist.Add(offset + 2);
            ilist.Add(offset + 2); ilist.Add(offset + 1); ilist.Add(offset + 3);


            offset += 4;

            //Totally crapply unoptimized, but phi can be negative
            phi = Convert.ToSingle(phase * (Math.PI * 2.0));
            for (int i = 0; i < res - 1; i++)
            {
                phi += inc;
            }

            x2 = Convert.ToSingle(0.5 * inner * Math.Cos(phi));
            y2 = Convert.ToSingle(0.5 * inner * Math.Sin(phi));

            x3 = Convert.ToSingle(0.5 * Math.Cos(phi));
            y3 = Convert.ToSingle(0.5 * Math.Sin(phi));

            q1.Position = new Vector4(x2, y2, z, 1.0f);
            q2.Position = new Vector4(x2, y2, -z, 1.0f);
            q3.Position = new Vector4(x3, y3, z, 1.0f);
            q4.Position = new Vector4(x3, y3, -z, 1.0f);

            e1 = new Vector3(q2.Position.X - q1.Position.X,
                q2.Position.Y - q1.Position.Y,
                q2.Position.Z - q1.Position.Z);

            e2 = new Vector3(q3.Position.X - q2.Position.X,
                q3.Position.Y - q2.Position.Y,
                q3.Position.Z - q2.Position.Z);

            n = Vector3.Cross(e2, e1);
            q1.Normals = n;
            q2.Normals = n;
            q3.Normals = n;
            q4.Normals = n;

            vlist.Add(q1); vlist.Add(q2); vlist.Add(q3); vlist.Add(q4);

            ilist.Add(offset); ilist.Add(offset + 2); ilist.Add(offset + 1);
            ilist.Add(offset + 2); ilist.Add(offset + 3); ilist.Add(offset + 1);



            #endregion

            DataStream ds = new DataStream(vlist.Count * Pos4Norm3Tex2Vertex.VertexSize, true, true);
            ds.Position = 0;
            ds.WriteRange(vlist.ToArray());
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

            var indexstream = new DataStream(ilist.Count * 4, true, true);
            indexstream.WriteRange(ilist.ToArray());
            indexstream.Position = 0;

            geom.VertexBuffer = vbuffer;
            geom.IndexBuffer = new DX11IndexBuffer(context, indexstream, false, true);
            geom.InputLayout = Pos4Norm3Tex2Vertex.Layout;
            geom.Topology = PrimitiveTopology.TriangleList;
            geom.VerticesCount = vlist.Count;
            geom.VertexSize = Pos4Norm3Tex2Vertex.VertexSize;

            geom.HasBoundingBox = false;

            return geom;
        }
    }
}
