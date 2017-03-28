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
        public DX11VertexGeometry LineStrip3d(List<Vector3> points, bool loop, bool buildAdjacency = false)
        {
            if (points.Count == 0)
            {
                //Build zero line
                points = new List<Vector3>(2);
                points.Add(Vector3.Zero);
                points.Add(Vector3.Zero);
            }
            else if (points.Count == 1)
            {
                var oldPoints = points;
                //Build zero line using current vertex
                points = new List<Vector3>(2);
                points.Add(oldPoints[0]);
                points.Add(oldPoints[0]);
            }

            DX11VertexGeometry geom = new DX11VertexGeometry(context);

            //Line has N vertex count, we add 1 in case of loop to close, and we add 2 more in case of adjacency
            int lineVertexCount = loop ? points.Count + 1 : points.Count;
            int totalVertexCount = buildAdjacency ? lineVertexCount + 2 : lineVertexCount;
            int startWriteIndex = buildAdjacency ? 1 : 0;

            Pos3Tex2Vertex[] verts = new Pos3Tex2Vertex[totalVertexCount];

            float uvStep = loop ? 1.0f / (float)totalVertexCount : 1.0f / ((float)totalVertexCount + 1.0f);
            float currentUv = 0.0f;
            
            for (int i = 0; i < points.Count; i++)
            {
                verts[startWriteIndex+i].Position = points[i];
                verts[startWriteIndex+i].TexCoords.X = currentUv;
                currentUv += uvStep;
            }

            //Add first point to close the loop
            if (loop)
            {
                verts[startWriteIndex+points.Count].Position = points[0];
                verts[startWriteIndex+points.Count].TexCoords.X = 1.0f;
            }

            if (buildAdjacency)
            {
                if (loop)
                {
                    //Add last point from the line as first vertex
                    verts[0] = verts[lineVertexCount - 1];

                    //Add second point as loop = last point
                    verts[0] = verts[1];
                }
                else
                {
                    //In that case, we just extend the lines from beginning and end, first point we reverse that dir
                    Vector3 originDir = points[1] - points[0];
                    verts[0].Position = points[0] - originDir;
                    verts[0].TexCoords = new Vector2(0.0f, 0.0f);

                    //Last point to previous
                    Vector3 destDir = points[points.Count-1] - points[points.Count-2];
                    verts[verts.Length - 1].Position = points[points.Count - 1] + destDir;
                    verts[verts.Length - 1].TexCoords = new Vector2(1.0f, 0.0f);
                }
            }


            DataStream ds = new DataStream(totalVertexCount * Pos3Tex2Vertex.VertexSize, true, true);
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
            geom.Topology = buildAdjacency ? PrimitiveTopology.LineStripWithAdjacency : PrimitiveTopology.LineStrip;
            geom.VerticesCount = totalVertexCount;
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
