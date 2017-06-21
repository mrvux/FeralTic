using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

using SlimDX;
using SlimDX.Direct3D11;

namespace FeralTic.DX11.Geometry
{
    [StructLayout(LayoutKind.Sequential)]
    public struct Quad3Vertex
    {
        public int Tri1;
        public int Tri2;
        public int Tri3;
        public int Tri4;
        public int Tri5;
        public int Tri6;

        private static InputElement[] layout;

        public static InputElement[] Layout
        {
            get
            {
                if (layout == null)
                {
                    layout = new InputElement[]
                    {
                        new InputElement("TRIANGLE",0,SlimDX.DXGI.Format.R32G32B32_UInt,0, 0),
                        new InputElement("TRIANGLE",1,SlimDX.DXGI.Format.R32G32B32_UInt, 12, 0)
                    };
                }
                return layout;
            }
        }

        public static int VertexSize
        {
            get { return Marshal.SizeOf(typeof(Quad3Vertex)); }
        }
    }
}
