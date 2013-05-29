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

    public class Isocahedron
    {
        public Vector3 Size { get; set; }
    }

    public class Octahedron
    {
        public Vector3 Size { get; set; }
    }

    public class Quad
    {
        public Vector2 Size { get; set; }
    }

    public class RoundRect
    {
        public RoundRect(Vector2 inner, float outer, int ires, bool enablecenter)
        {
            this.CornerResolution = ires;
            this.InnerRadius = inner;
            this.OuterRadius = outer;
            this.EnableCenter = enablecenter;
        }

        public Vector2 InnerRadius { get; set;}
        public float OuterRadius { get; set;}
        public int CornerResolution { get; set;}
        public bool EnableCenter { get; set; }

    }

    public class Segment
    {
        public float Phase { get; set; }
        public float Cycles { get; set; }
        public float InnerRadius { get; set; }
        public bool Flat { get; set; }
        public int Resolution { get; set; }
    }

    public class SegmentZ
    {
        public float Phase { get; set; }
        public float Cycles { get; set; }
        public float InnerRadius { get; set; }
        public float Z { get; set; }
        public int Resolution { get; set; }
    }

    public class Sphere
    {
        public float Radius { get; set; }
        public int ResX { get; set; }
        public int ResY { get; set; }
        public float CyclesX { get; set; }
        public float CyclesY { get; set; }
    }

    public class Tetrahedron
    {
        public Vector3 Size { get; set; }
    }

    public class Torus
    {
        public Torus(int resX, int resY, float radius, float thick, float phasey, float phasex, float rot, float cy)
        {
            this.ResolutionX = resX;
            this.ResolutionY = resY;
            this.Radius = radius;
            this.Thickness = thick;
            this.PhaseX = phasex;
            this.PhaseY = phasey;
            this.Rotation = rot;
            this.CY = cy;
        }

        public int ResolutionX { get; set; }
        public int ResolutionY { get; set; }
        public float Radius { get; set; }
        public float Thickness { get; set; }
        public float PhaseY { get; set; }
        public float PhaseX { get; set; }
        public float Rotation { get; set; }
        public float CY { get; set; }
    }




}
