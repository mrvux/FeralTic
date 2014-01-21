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
        public DX11IndexedGeometry RoundRect(RoundRect settings)
        {
            Vector2 inner = settings.InnerRadius;
           float outer = settings.OuterRadius;
           int ires = settings.CornerResolution;

            DX11IndexedGeometry geom = new DX11IndexedGeometry(context);
            geom.PrimitiveType = settings.PrimitiveType;
            geom.Tag = settings;
            List<Pos4Norm3Tex2Vertex> vl = new List<Pos4Norm3Tex2Vertex>();
            List<int> il = new List<int>();

            int idx = 0;

            float ucy = Convert.ToSingle(inner.Y + outer);
            float ucx = Convert.ToSingle(inner.X + outer);

            float mx = ucx * 2.0f;
            float my = ucy * 2.0f;

            //Need 1 quad for center
            if (settings.EnableCenter)
            {
                idx = SetQuad(vl, il, 0.0f, 0.0f, inner.X, inner.Y, idx, mx, my);
            }

            //Need 2 quads up/down
            idx = SetQuad(vl, il, 0.0f, ucy, inner.X, (float)outer, idx, mx, my);
            idx = SetQuad(vl, il, 0.0f, -ucy, inner.X, (float)outer, idx, mx, my);

            //Need 2 quads left/right
            idx = SetQuad(vl, il, -ucx, 0.0f, (float)outer, inner.Y, idx, mx, my);
            idx = SetQuad(vl, il, ucx, 0.0f, (float)outer, inner.Y, idx, mx, my);

            float radius = (float)outer * 2.0f;

            //Add the 4 corners
            idx = SetSegment(vl, il, inner.X, inner.Y, 0.0f, radius, ires, idx, mx, my);
            idx = SetSegment(vl, il, -inner.X, inner.Y, 0.25f, radius, ires, idx, mx, my);
            idx = SetSegment(vl, il, -inner.X, -inner.Y, 0.5f, radius, ires, idx, mx, my);
            idx = SetSegment(vl, il, inner.X, -inner.Y, 0.75f, radius, ires, idx, mx, my);

            DataStream ds = new DataStream(vl.Count * Pos4Norm3Tex2Vertex.VertexSize, true, true);
            ds.Position = 0;
            ds.WriteRange(vl.ToArray());
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

            var indexstream = new DataStream(il.Count * 4, true, true);
            indexstream.WriteRange(il.ToArray());
            indexstream.Position = 0;

            geom.VertexBuffer = vbuffer;
            geom.IndexBuffer = new DX11IndexBuffer(context, indexstream, false, true);
            geom.InputLayout = Pos4Norm3Tex2Vertex.Layout;
            geom.Topology = PrimitiveTopology.TriangleList;
            geom.VerticesCount = vl.Count;
            geom.VertexSize = Pos4Norm3Tex2Vertex.VertexSize;

            geom.HasBoundingBox = false;


            return geom;
        }

        private int SetQuad(List<Pos4Norm3Tex2Vertex> verts, List<int> inds, float cx, float cy, float sx, float sy, int lastindex, float mx, float my)
        {
            Pos4Norm3Tex2Vertex innerv = new Pos4Norm3Tex2Vertex();
            innerv.Normals = new Vector3(0.0f, 0.0f, 1.0f);

            innerv.Position = new Vector4(cx - sx, cy - sy, 0.0f, 1.0f);
            innerv = TexCoord(innerv, mx, my);
            verts.Add(innerv);

            innerv.Position = new Vector4(cx + sx, cy + sy, 0.0f, 1.0f);
            innerv = TexCoord(innerv, mx, my);
            verts.Add(innerv);

            innerv.Position = new Vector4(cx - sx, cy + sy, 0.0f, 1.0f);
            innerv = TexCoord(innerv, mx, my);
            verts.Add(innerv);

            innerv.Position = new Vector4(cx + sx, cy - sy, 0.0f, 1.0f);
            innerv = TexCoord(innerv, mx, my);
            verts.Add(innerv);

            int[] idxarray = new int[] { 0, 2, 1, 0, 1, 3 };
            for (int i = 0; i < idxarray.Length; i++) { idxarray[i] += lastindex; }
            inds.AddRange(idxarray);

            return lastindex + 4;
        }

        private int SetSegment(List<Pos4Norm3Tex2Vertex> verts, List<int> inds, float cx, float cy,
            float phase, float radius, int ires, int lastindex, float mx, float my)
        {
            //Center vertex
            Pos4Norm3Tex2Vertex innerv = new Pos4Norm3Tex2Vertex();
            innerv.Normals = new Vector3(0.0f, 0.0f, 1.0f);

            innerv.Position = new Vector4(cx, cy, 0.0f, 1.0f);
            innerv = TexCoord(innerv, mx, my);
            verts.Add(innerv);

            double inc = (Math.PI / 2.0) / (double)ires;
            double phi = phase * (Math.PI * 2.0);

            //Build triangle strip here
            for (int i = 0; i < ires + 1; i++)
            {
                float x = Convert.ToSingle(cx + radius * Math.Cos(phi));
                float y = Convert.ToSingle(cy + radius * Math.Sin(phi));

                innerv.Position = new Vector4(x, y, 0.0f, 1.0f);
                innerv = TexCoord(innerv, mx, my);
                verts.Add(innerv);

                phi += inc;
            }

            for (int i = 0; i < ires; i++)
            {
                inds.Add(lastindex);
                inds.Add(lastindex + i + 2);
                inds.Add(lastindex + i + 1);
            }

            return lastindex + ires + 2;
        }

        private Pos4Norm3Tex2Vertex TexCoord(Pos4Norm3Tex2Vertex v, float mx, float my)
        {
            v.TexCoords = new Vector2(0.5f + (v.Position.X / (mx * 2.0f)), 0.5f + (v.Position.Y / (my * 2.0f)));
            return v;
        }
    }
}
