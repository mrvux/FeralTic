using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FeralTic.DX11.Resources;
using SlimDX;
using SlimDX.Direct3D11;

namespace FeralTic.DX11.Geometry
{
    public partial class DX11PrimitivesManager
    {
        public DX11IndexedGeometry Torus(int resX, int resY, float radius, float thick)
        {
            List<Pos4Norm3Tex2Vertex> vertices = new List<Pos4Norm3Tex2Vertex>();
            List<int> indices = new List<int>();

            float pi = (float)Math.PI;
            float pidiv2 = pi * 0.5f;
            float twopi = pi * 2.0f;

            int stride = resY + 1;

            for (int i = 0; i <= resY; i++)
            {
                float u = (float)i / (float)resY;

                float outerAngle = (float)i * twopi / (float)resY - pidiv2;

                Matrix transform = Matrix.Translation(radius, 0, 0) * Matrix.RotationY(outerAngle);

                for (int j = 0; j <= resX; j++)
                {
                    float v = 1 - (float)j / resX;

                    float innerAngle = (float)j * twopi / (float)resX + pi;
                    float dy = (float)Math.Sin(innerAngle);
                    float dx = (float)Math.Cos(innerAngle);

                    Pos4Norm3Tex2Vertex vertex = new Pos4Norm3Tex2Vertex();

                    Vector3 normal = new Vector3(dx, dy, 0);
                    Vector3 position = normal * thick / 2;
                    vertex.TexCoords = new Vector2(u, v);
                    vertex.Normals = Vector3.TransformNormal(normal, transform);

                    position = Vector3.TransformCoordinate(position, transform);
                    vertex.Position = new Vector4(position.X, position.Y, position.Z, 1.0f);

                    vertices.Add(vertex);

                    int nextI = (i + 1) % stride;
                    int nextJ = (j + 1) % stride;

                    indices.Add(j * stride + i);
                    indices.Add(j * stride + nextI);
                    indices.Add(nextJ * stride + i);

                    indices.Add(j * stride + nextI);
                    indices.Add(nextJ * stride + nextI);
                    indices.Add(nextJ * stride + i);
                }
            }

            DX11IndexedGeometry geom = DX11IndexedGeometry.CreateFrom<Pos4Norm3Tex2Vertex>(context, vertices.ToArray(), indices.ToArray(), Pos4Norm3Tex2Vertex.Layout);
            geom.HasBoundingBox = false;
            geom.BoundingBox = new BoundingBox(new Vector3(-radius), new Vector3(radius));

            return geom;
        }

    }
}
