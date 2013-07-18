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
        public DX11IndexedGeometry IcoSphere(IcoSphere settings)
        {
            float radius = settings.Radius;
            int subdiv = settings.SubDivisions;


            List<Pos3Norm3Tex2Vertex> vertices = new List<Pos3Norm3Tex2Vertex>();
            Pos3Norm3Tex2Vertex v = new Pos3Norm3Tex2Vertex();

            float t = (1.0f + (float)Math.Sqrt(5.0f)) / 2.0f;
            //TODO : Add texcoord
            v.TexCoords = new Vector2(0, 0);

            //Isocahedron
            v.Normals = Vector3.Normalize(new Vector3(-1, t, 0)); v.Position = v.Normals * radius; vertices.Add(v);
            v.Normals = Vector3.Normalize(new Vector3(1, t, 0)); v.Position = v.Normals * radius; vertices.Add(v);
            v.Normals = Vector3.Normalize(new Vector3(-1, -t, 0)); v.Position = v.Normals * radius; vertices.Add(v);
            v.Normals = Vector3.Normalize(new Vector3(1, -t, 0)); v.Position = v.Normals * radius; vertices.Add(v);

            v.Normals = Vector3.Normalize(new Vector3(0, -1, t)); v.Position = v.Normals * radius; vertices.Add(v);
            v.Normals = Vector3.Normalize(new Vector3(0, 1, t)); v.Position = v.Normals * radius; vertices.Add(v);
            v.Normals = Vector3.Normalize(new Vector3(0, -1, -t)); v.Position = v.Normals * radius; vertices.Add(v);
            v.Normals = Vector3.Normalize(new Vector3(0, 1, -t)); v.Position = v.Normals * radius; vertices.Add(v);

            v.Normals = Vector3.Normalize(new Vector3(t, 0, -1)); v.Position = v.Normals * radius; vertices.Add(v);
            v.Normals = Vector3.Normalize(new Vector3(t, 0, 1)); v.Position = v.Normals * radius; vertices.Add(v);
            v.Normals = Vector3.Normalize(new Vector3(-t, 0, -1)); v.Position = v.Normals * radius; vertices.Add(v);
            v.Normals = Vector3.Normalize(new Vector3(-t, 0, 1)); v.Position = v.Normals * radius; vertices.Add(v);

            int[] inds = new int[] { 
                0,11,5,
                0,5,1,
                0,1,7,
                0,7,10,
                0,10,11,
                1,5,9,
                5,11,4,
                11,10,2,
                10,7,6,
                7,1,8,
                3,9,4,
                3,4,2,
                3,2,6,
                3,6,8,
                3,8,9,
                4,9,5,
                2,4,11,
                6,2,10,
                8,6,7,
                9,8,1 };

            List<int> indices = new List<int>(inds);

            Pos3Norm3Tex2Vertex newvertex = new Pos3Norm3Tex2Vertex();

            for (int iter = 0; iter < subdiv; iter++)
            {
                List<int> newindices = new List<int>();

                for (int i = 0; i < indices.Count / 3; i++)
                {
                    //Grab Indices
                    int i1 = indices[i * 3];
                    int i2 = indices[i * 3 + 1];
                    int i3 = indices[i * 3 + 2];

                    //Get Vertices
                    Pos3Norm3Tex2Vertex v1 = vertices[i1];
                    Pos3Norm3Tex2Vertex v2 = vertices[i2];
                    Pos3Norm3Tex2Vertex v3 = vertices[i3];

                    Vector3 e1 = (v1.Position + v2.Position) * 0.5f;
                    Vector3 e2 = (v2.Position + v3.Position) * 0.5f;
                    Vector3 e3 = (v3.Position + v1.Position) * 0.5f;

                    int ie1 = vertices.Count;
                    int ie2 = vertices.Count + 1;
                    int ie3 = vertices.Count + 2;

                    //Push 3 new vertices
                    newvertex.Normals = Vector3.Normalize(e1);
                    newvertex.Position = newvertex.Normals * radius;
                    vertices.Add(newvertex);
                    newvertex.Normals = Vector3.Normalize(e2);
                    newvertex.Position = newvertex.Normals * radius;
                    vertices.Add(newvertex);
                    newvertex.Normals = Vector3.Normalize(e3);
                    newvertex.Position = newvertex.Normals * radius;
                    vertices.Add(newvertex);

                    //Push 4 triangles
                    newindices.Add(ie1); newindices.Add(ie2); newindices.Add(ie3);
                    newindices.Add(i1); newindices.Add(ie1); newindices.Add(ie3);
                    newindices.Add(i2); newindices.Add(ie2); newindices.Add(ie1);
                    newindices.Add(i3); newindices.Add(ie3); newindices.Add(ie2);
                }

                //Swap
                indices = newindices;
            }

            DX11IndexedGeometry geom = DX11IndexedGeometry.CreateFrom<Pos3Norm3Tex2Vertex>(context, vertices.ToArray(), indices.ToArray(), Pos3Norm3Tex2Vertex.Layout);
            geom.HasBoundingBox = true;
            geom.BoundingBox = new BoundingBox(new Vector3(-radius), new Vector3(radius));
            geom.Tag = settings;
            geom.PrimitiveType = settings.PrimitiveType;

            return geom;
        }

    }
}
