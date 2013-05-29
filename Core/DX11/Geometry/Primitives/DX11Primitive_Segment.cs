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
        public DX11IndexedGeometry Segment(Segment settings)
        {
            float phase = settings.Phase;
            float cycles = settings.Cycles; 
            float inner = settings.InnerRadius; 
            int res = settings.Resolution;
            bool flat = settings.Flat;

            DX11IndexedGeometry geom = new DX11IndexedGeometry(context);
            geom.Tag = settings;
            geom.PrimitiveType = settings.PrimitiveType;
            int vcount = res * 2;
            int icount = (res - 1) * 6;

            DataStream ds = new DataStream(vcount * Pos4Norm3Tex2Vertex.VertexSize, true, true);
            ds.Position = 0;

            float inc = Convert.ToSingle((Math.PI * 2.0 * cycles) / (res - 1.0));
            float phi = Convert.ToSingle(phase * (Math.PI * 2.0));

            Pos4Norm3Tex2Vertex innerv = new Pos4Norm3Tex2Vertex();
            innerv.Normals = new Vector3(0.0f, 0.0f, 1.0f);

            Pos4Norm3Tex2Vertex outerv = new Pos4Norm3Tex2Vertex();
            outerv.Normals = new Vector3(0.0f, 0.0f, 1.0f);

            Pos4Norm3Tex2Vertex[] vertices = new Pos4Norm3Tex2Vertex[res * 2];

            for (int i = 0; i < res; i++)
            {
                float x = Convert.ToSingle(0.5 * inner * Math.Cos(phi));
                float y = Convert.ToSingle(0.5 * inner * Math.Sin(phi));

                innerv.Position = new Vector4(x, y, 0.0f, 1.0f);

                if (flat)
                {
                    innerv.TexCoords = new Vector2(0.5f - x, 0.5f - y);
                }
                else
                {
                    innerv.TexCoords = new Vector2((1.0f * (float)i) / ((float)res - 1.0f), 0.0f);
                }

                x = Convert.ToSingle(0.5 * Math.Cos(phi));
                y = Convert.ToSingle(0.5 * Math.Sin(phi));

                outerv.Position = new Vector4(x, y, 0.0f, 1.0f);

                if (flat)
                {
                    outerv.TexCoords = new Vector2(0.5f - x, 0.5f - y);
                }
                else
                {
                    outerv.TexCoords = new Vector2((1.0f * (float)i) / ((float)res - 1.0f), 1.0f);
                }

                vertices[i] = innerv;
                vertices[i + res] = outerv;
                phi += inc;
            }

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

            int indstep = 0;
            int[] indices = new int[icount];
            for (int i = 0; i < res - 1; i++)
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

            var indexstream = new DataStream(icount * 4, true, true);
            indexstream.WriteRange(indices);
            indexstream.Position = 0;

            geom.VertexBuffer = vbuffer;
            geom.IndexBuffer = new DX11IndexBuffer(context, indexstream, false, true);
            geom.InputLayout = Pos4Norm3Tex2Vertex.Layout;
            geom.Topology = PrimitiveTopology.TriangleList;
            geom.VerticesCount = vcount;
            geom.VertexSize = Pos4Norm3Tex2Vertex.VertexSize;

            geom.HasBoundingBox = false;

            return geom;
        }
    }
}
