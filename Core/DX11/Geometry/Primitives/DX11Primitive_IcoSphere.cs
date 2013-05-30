/* IcoSphere, minimal version taken from SharpDX */
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
            Dictionary<UndirectedEdge, int> subdividedEdges = new Dictionary<UndirectedEdge, int>();

            Vector3[] OctahedronVertices = new Vector3[]
            {
                new Vector3( 0,  1,  0), // 0 top
                new Vector3( 0,  0, -1), // 1 front
                new Vector3( 1,  0,  0), // 2 right
                new Vector3( 0,  0,  1), // 3 back
                new Vector3(-1,  0,  0), // 4 left
                new Vector3( 0, -1,  0), // 5 bottom
            };

            int[] OctahedronIndices = new int[]
            {
                0, 1, 2, // top front-right face
                0, 2, 3, // top back-right face
                0, 3, 4, // top back-left face
                0, 4, 1, // top front-left face
                5, 1, 4, // bottom front-left face
                5, 4, 3, // bottom back-left face
                5, 3, 2, // bottom back-right face
                5, 2, 1, // bottom front-right face
            };

            List<Vector3> vertexPositions = new List<Vector3>();

            List<Pos3Norm3Vertex> vertices = new List<Pos3Norm3Vertex>();
            List<int> indexList = new List<int>();

            /*const int northPoleIndex = 0;
            const int southPoleIndex = 5;*/

            for (int iSubdivision = 0; iSubdivision < subdiv; ++iSubdivision)
            {
                // The new index collection after subdivision.
                var newIndices = new List<int>();
                subdividedEdges.Clear();

                int triangleCount = indexList.Count / 3;
                for (int iTriangle = 0; iTriangle < triangleCount; ++iTriangle)
                {
                    // For each edge on this triangle, create a new vertex in the middle of that edge.
                    // The winding order of the triangles we output are the same as the winding order of the inputs.

                    // Indices of the vertices making up this triangle
                    int iv0 = indexList[iTriangle * 3 + 0];
                    int iv1 = indexList[iTriangle * 3 + 1];
                    int iv2 = indexList[iTriangle * 3 + 2];

                    //// The existing vertices
                    //Vector3 v0 = vertexPositions[iv0];
                    //Vector3 v1 = vertexPositions[iv1];
                    //Vector3 v2 = vertexPositions[iv2];

                    // Get the new vertices
                    Vector3 v01; // vertex on the midpoint of v0 and v1
                    Vector3 v12; // ditto v1 and v2
                    Vector3 v20; // ditto v2 and v0
                    int iv01; // index of v01
                    int iv12; // index of v12
                    int iv20; // index of v20

                    // Add/get new vertices and their indices
                    DivideEdge(subdividedEdges,vertexPositions, iv0, iv1, out v01, out iv01);
                    DivideEdge(subdividedEdges, vertexPositions, iv1, iv2, out v12, out iv12);
                    DivideEdge(subdividedEdges, vertexPositions, iv0, iv2, out v20, out iv20);

                    newIndices.Add(iv0);
                    newIndices.Add(iv01);
                    newIndices.Add(iv20);

                    // b
                    newIndices.Add(iv20);
                    newIndices.Add(iv12);
                    newIndices.Add(iv2);

                    // d
                    newIndices.Add(iv20);
                    newIndices.Add(iv01);
                    newIndices.Add(iv12);

                    // d
                    newIndices.Add(iv01);
                    newIndices.Add(iv1);
                    newIndices.Add(iv12);
                }

                indexList.Clear();
                indexList.AddRange(newIndices);
            }

            // Now that we've completed subdivision, fill in the final vertex collection
            vertices = new List<Pos3Norm3Vertex>(vertexPositions.Count);
            for (int i = 0; i < vertexPositions.Count; i++)
            {

                Pos3Norm3Vertex pn = new Pos3Norm3Vertex();
                pn.Position= vertexPositions[i];
                pn.Normals = Vector3.Normalize(pn.Position);
                vertices.Add(pn);
            }

            DX11IndexedGeometry geom = DX11IndexedGeometry.CreateFrom<Pos3Norm3Vertex>(context, vertices.ToArray(), indexList.ToArray(), Pos3Norm3Vertex.Layout);
            geom.HasBoundingBox = true;
            geom.BoundingBox = new BoundingBox(new Vector3(-radius), new Vector3(radius));
            geom.Tag = settings;
            geom.PrimitiveType = settings.PrimitiveType;

            return geom;
        }

        // Function that, when given the index of two vertices, creates a new vertex at the midpoint of those vertices.
        private void DivideEdge(Dictionary<UndirectedEdge, int> subdividedEdges,List<Vector3> vertexPositions, int i0, int i1, out Vector3 outVertex, out int outIndex)
        {
            var edge = new UndirectedEdge(i0, i1);

            // Check to see if we've already generated this vertex
            if (subdividedEdges.TryGetValue(edge, out outIndex))
            {
                // We've already generated this vertex before
                outVertex = vertexPositions[outIndex]; // and the vertex itself
            }
            else
            {
                // Haven't generated this vertex before: so add it now

                // outVertex = (vertices[i0] + vertices[i1]) / 2
                outVertex = (vertexPositions[i0] + vertexPositions[i1]) * 0.5f;
                outIndex = vertexPositions.Count;
                vertexPositions.Add(outVertex);

                // Now add it to the map.
                subdividedEdges[edge] = outIndex;
            }
        }

        // An undirected edge between two vertices, represented by a pair of indexes into a vertex array.
        // Becuse this edge is undirected, (a,b) is the same as (b,a).
        private struct UndirectedEdge : IEquatable<UndirectedEdge>
        {
            public UndirectedEdge(int item1, int item2)
            {
                // Makes an undirected edge. Rather than overloading comparison operators to give us the (a,b)==(b,a) property,
                // we'll just ensure that the larger of the two goes first. This'll simplify things greatly.
                Item1 = Math.Max(item1, item2);
                Item2 = Math.Min(item1, item2);
            }

            public readonly int Item1;

            public readonly int Item2;

            public bool Equals(UndirectedEdge other)
            {
                return Item1 == other.Item1 && Item2 == other.Item2;
            }

            public override bool Equals(object obj)
            {
                if (ReferenceEquals(null, obj)) return false;
                return obj is UndirectedEdge && Equals((UndirectedEdge)obj);
            }

            public override int GetHashCode()
            {
                unchecked
                {
                    return (Item1.GetHashCode() * 397) ^ Item2.GetHashCode();
                }
            }

            public static bool operator ==(UndirectedEdge left, UndirectedEdge right)
            {
                return left.Equals(right);
            }

            public static bool operator !=(UndirectedEdge left, UndirectedEdge right)
            {
                return !left.Equals(right);
            }
        }

    }
}
