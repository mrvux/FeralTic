using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SlimDX;
using SlimDX.Direct3D11;

using FeralTic.DX11.Utils;
using FeralTic.DX11.Resources;



namespace FeralTic.DX11.Geometry
{
    public partial class DX11PrimitivesManager
    {
        public DX11IndexedGeometry Sphere(Sphere settings)
        {
            float radius = settings.Radius;
            int resX = settings.ResX;
            int resY = settings.ResY;
            float cx = settings.CyclesX;
            float cy = settings.CyclesY;

            List<Pos4Norm3Tex2Vertex> vertices = new List<Pos4Norm3Tex2Vertex>();
            List<int> indices = new List<int>();

            float pi = (float)Math.PI;
            float pidiv2 = pi * 0.5f;
            float twopi = pi * 2.0f;

            

            for (int i = 0; i <= resY; i++)
            {
                float v = 1 - (float)i / resY;

                float latitude = (i * pi / resY) - pidiv2;

                float dy = (float)Math.Sin(latitude * cy);
                float dxz = (float)Math.Cos(latitude * cy);

                // Create a single ring of vertices at this latitude.
                for (int j = 0; j <= resX; j++)
                {
                    float u = (float)j / resX;

                    float longitude = j * twopi / resX;

                    float dx = (float)Math.Sin(longitude * cx);
                    float dz = (float)Math.Cos(longitude * cx);

                    dx *= dxz;
                    dz *= dxz;

                    Pos4Norm3Tex2Vertex vertex = new Pos4Norm3Tex2Vertex();
                    vertex.Position = new Vector4(dx * radius, dy * radius, dz * radius, 1.0f);
                    vertex.Normals = Vector3.Normalize(new Vector3(vertex.Position.X, vertex.Position.Y, vertex.Position.Z));
                    vertex.TexCoords = new Vector2(u,v);

                    vertices.Add(vertex);
                }
            }

            int stride = resX + 1;

            for (int i = 0; i < resY; i++)
            {
                for (int j = 0; j <= resX; j++)
                {
                    int nextI = i + 1;
                    int nextJ = (j + 1) % stride;

                    indices.Add(i * stride + j);
                    indices.Add(i * stride + nextJ);
                    indices.Add(nextI * stride + j);

                    indices.Add(i * stride + nextJ);
                    indices.Add(nextI * stride + nextJ);
                    indices.Add(nextI * stride + j);
                }
            }

            DX11IndexedGeometry geom = DX11IndexedGeometry.CreateFrom<Pos4Norm3Tex2Vertex>(context, vertices.ToArray(), indices.ToArray(), Pos4Norm3Tex2Vertex.Layout);               
            geom.HasBoundingBox = true;
            geom.BoundingBox = new BoundingBox(new Vector3(-radius), new Vector3(radius));
            geom.Tag = settings;
            geom.PrimitiveType = "Sphere";

            return geom;
        }

    }
}
