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
        public DX11IndexedGeometry IcoGrid(Vector2 size, int resX, int resY)
        {
            float fdispx = -0.5f;
            float fdispy = (float)Math.Sqrt(0.75);
            List<Pos4Norm3Tex2Vertex> verts = new List<Pos4Norm3Tex2Vertex>();

            Pos4Norm3Tex2Vertex v = new Pos4Norm3Tex2Vertex();
            v.Normals = new Vector3(0, 0, -1);
            v.TexCoords = new Vector2(0, 0);

            float maxx = float.MinValue;
            float maxy = float.MinValue;

            bool bdisp = true;
            float y = 0.0f;
            for (int i = 0; i < resX; i++)
            {
                bdisp = !bdisp;

                int cnt = resY;

                for (int j = 0; j < cnt; j++)
                {
                    if (bdisp)
                    {
                        v.Position = new Vector4((float)j - 0.5f, y, 0.0f, 1.0f);

                        if (v.Position.X > maxx)
                        {
                            maxx = v.Position.X;
                        }
                        if (v.Position.Y > maxy)
                        {
                            maxy = v.Position.Y;
                        }
                    }
                    else
                    {
                        v.Position = new Vector4((float)j, y, 0.0f, 1.0f);

                        if (v.Position.X > maxx)
                        {
                            maxx = v.Position.X;
                        }
                        if (v.Position.Y > maxy)
                        {
                            maxy = v.Position.Y;
                        }
                    }
                    verts.Add(v);
                }
                y += fdispy;
            }

            float invmx = 1.0f / maxx;
            float invmy = 1.0f / maxy;
            for (int i = 0; i < verts.Count; i++)
            {
                Pos4Norm3Tex2Vertex v2 = verts[i];
                v2.Position.X *= invmx;
                v2.Position.X -= 0.5f;
                v2.Position.Y *= invmy;
                v2.Position.Y -= 0.5f;
                verts[i] = v2;
            }

            List<int> inds = new List<int>();

            bool bflip = true;
            int nextrow = resY;
            int a = 1;
            for (int i = 0; i < resX - 1; i++)
            {
                bflip = !bflip;
                for (int j = 0; j < resY - 1; j++)
                {
                    int linestart = i * resY;
                    int lineup = (i + 1) * resY;
                    if (!bflip)
                    {
                        inds.Add(lineup + j);
                        inds.Add(linestart + j);
                        inds.Add(lineup + j + 1);

                        inds.Add(linestart + j);
                        inds.Add(linestart + j + 1);
                        inds.Add(lineup + j + 1);
                    }
                    else
                    {
                        inds.Add(linestart + j);
                        inds.Add(linestart + j + 1);
                        inds.Add(lineup + j);

                        inds.Add(lineup + j);
                        inds.Add(linestart + j + 1);
                        inds.Add(lineup + j + 1);
                    }
                }
                a += nextrow;
            }

            DX11IndexedGeometry geom = new DX11IndexedGeometry(context);

            DataStream vertexstream = new DataStream(verts.Count * Pos4Norm3Tex2Vertex.VertexSize, true, true);
            vertexstream.Position = 0;
            vertexstream.WriteRange(verts.ToArray());
            vertexstream.Position = 0;

            var vertices = BufferHelper.CreateDynamicVertexBuffer(context, vertexstream, true);

            var indexstream = new DataStream(inds.Count * 4, true, true);
            indexstream.WriteRange(inds.ToArray());
            indexstream.Position = 0;

            geom.VertexBuffer = vertices;
            geom.IndexBuffer = new DX11IndexBuffer(context, indexstream, false, true);
            geom.InputLayout = Pos4Norm3Tex2Vertex.Layout;
            geom.Topology = PrimitiveTopology.TriangleList;
            geom.VerticesCount = verts.Count;
            geom.VertexSize = Pos4Norm3Tex2Vertex.VertexSize;

            geom.HasBoundingBox = false;

            return geom;
        }
    }
}
