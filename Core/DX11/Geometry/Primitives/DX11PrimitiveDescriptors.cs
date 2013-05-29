using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SlimDX;

namespace FeralTic.DX11.Geometry
{
    public class Box
    {
        public Vector3 Size;
    }

    public class Cylinder
    {
        public float Radius1 { get; set; }
        public float Radius2 { get; set; }
        public float Cycles { get; set; }
        public float Length { get; set; }
        public int ResolutionX { get; set; }
        public int ResolutionY { get; set; }
        public bool Caps { get; set; }
    }

    public class Grid
    {
        public Vector2 Size { get; set; }
        public int ResolutionX { get; set; }
        public int ResolutionY { get; set; }
    }

    public class IcoGrid
    {
        public Vector2 Size { get; set; }
        public int ResolutionX { get; set; }
        public int ResolutionY { get; set; }
    }

    public class Sphere
    {
        public float Radius { get; set; }
        public int ResX { get; set; }
        public int ResY { get; set; }
        public float CyclesX { get; set; }
        public float CyclesY { get; set; }
    }

    public class SegmentZ
    {
        public float Phase { get; set; }
        public float Cycles { get; set; }
        public float InnerRadius { get; set; }
        public float Z { get; set; }
        public int Resolution { get; set; }
    }


}
