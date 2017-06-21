using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FeralTic.DX11.Geometry;
using SlimDX.Direct3D11;

namespace FeralTic.DX11.Utils
{
    public static class VertexLayoutsHelpers
    {
        public const string VertexLayoutsEnumName = "DX11_Vertexlayouts_Presets";
        private static string[] enumEntries;

        private static readonly IReadOnlyDictionary<string, InputElement[]> elements;

        public static IReadOnlyList<string> Entries
        {
            get { return enumEntries; }
        }

        public static IReadOnlyDictionary<string, InputElement[]> Elements
        {
            get { return elements; }
        }

        static VertexLayoutsHelpers()
        {
            Dictionary<string, InputElement[]> data = new Dictionary<string, InputElement[]>();

            data.Add("Pos3", new InputElement[] { new InputElement("POSITION", 0, SlimDX.DXGI.Format.R32G32B32_Float, 0, 0) });
            data.Add("Pos4", new InputElement[] { new InputElement("POSITION", 0, SlimDX.DXGI.Format.R32G32B32A32_Float, 0, 0) });

            data.Add("Pos3Norm3", Pos3Norm3Vertex.Layout);
            data.Add("Pos4Norm3", Pos4Norm3Vertex.Layout);

            data.Add("Pos3Norm3Tex2", Pos3Norm3Tex2Vertex.Layout);
            data.Add("Pos4Norm3Tex2", Pos4Norm3Tex2Vertex.Layout);

            data.Add("Pos2Tex2", Pos2Tex2Vertex.Layout);
            data.Add("Pos3Tex2", Pos3Tex2Vertex.Layout);
            data.Add("Pos4Tex2", Pos4Tex2Vertex.Layout);

            data.Add("Indices3", Triangle3Vertex.Layout);
            data.Add("Quad3", Quad3Vertex.Layout);

            elements = data;
            enumEntries = data.Keys.ToArray();
        }


    }
}
