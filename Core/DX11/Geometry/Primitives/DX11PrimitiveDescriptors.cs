using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SlimDX;
using FeralTic.DX11.Resources;

namespace FeralTic.DX11.Geometry
{
    public abstract class AbstractPrimitiveDescriptor
    {
        public abstract string PrimitiveType { get; }
        public abstract void Initialize(Dictionary<string, object> properties);
        public abstract IDX11Geometry GetGeometry(DX11RenderContext context);
    }

    public class Box : AbstractPrimitiveDescriptor
    {
        public Vector3 Size { get; set; }

        public override string PrimitiveType { get { return "Box"; } }

        public override void Initialize(Dictionary<string, object> properties)
        {
            this.Size = (Vector3)properties["Size"];
        }

        public override IDX11Geometry GetGeometry(DX11RenderContext context)
        {
            return context.Primitives.Box(this);
        }
    }

    public class Cylinder : AbstractPrimitiveDescriptor
    {
        public float Radius1 { get; set; }
        public float Radius2 { get; set; }
        public float Cycles { get; set; }
        public float Length { get; set; }
        public int ResolutionX { get; set; }
        public int ResolutionY { get; set; }
        public bool Caps { get; set; }
        public bool CenterY { get; set; }

        public override string PrimitiveType { get { return "Cylinder"; } }

        public override void Initialize(Dictionary<string, object> properties)
        {
            this.Radius1 = (float)properties["Radius1"];
            this.Radius2 = (float)properties["Radius2"];
            this.Cycles = (float)properties["Cycles"];
            this.Length = (float)properties["Length"];
            this.ResolutionX = (int)properties["ResolutionX"];
            this.ResolutionY = (int)properties["ResolutionY"];
            this.Caps = (bool)properties["Caps"];
            this.CenterY = (bool)properties["CenterY"];
        }

        public override IDX11Geometry GetGeometry(DX11RenderContext context)
        {
            return context.Primitives.Cylinder(this);
        }
    }

    public class Grid : AbstractPrimitiveDescriptor
    {
        public Vector2 Size { get; set; }
        public int ResolutionX { get; set; }
        public int ResolutionY { get; set; }

        public override string PrimitiveType { get { return "Grid"; } }

        public override void Initialize(Dictionary<string, object> properties)
        {
            this.Size = (Vector2)properties["Size"];
            this.ResolutionX = (int)properties["ResolutionX"];
            this.ResolutionY = (int)properties["ResolutionY"];
        }

        public override IDX11Geometry GetGeometry(DX11RenderContext context)
        {
            return context.Primitives.Grid(this);
        }
    }

    public class IcoGrid : AbstractPrimitiveDescriptor
    {
        public Vector2 Size { get; set; }
        public int ResolutionX { get; set; }
        public int ResolutionY { get; set; }

        public override string PrimitiveType { get { return "IcoGrid"; } }

        public override void Initialize(Dictionary<string, object> properties)
        {
            this.Size = (Vector2)properties["Size"];
            this.ResolutionX = (int)properties["ResolutionX"];
            this.ResolutionY = (int)properties["ResolutionY"];
        }

        public override IDX11Geometry GetGeometry(DX11RenderContext context)
        {
            return context.Primitives.IcoGrid(this);
        }
    }

    public class IcoSphere : AbstractPrimitiveDescriptor
    {
        public float Radius { get; set; }
        public int SubDivisions { get; set; }

        public override string PrimitiveType { get { return "IcoSphere"; } }

        public override void Initialize(Dictionary<string, object> properties)
        {
            this.Radius = (float)properties["Radius"];
            this.SubDivisions = (int)properties["SubDivisions"];
        }

        public override IDX11Geometry GetGeometry(DX11RenderContext context)
        {
            return context.Primitives.IcoSphere(this);
        }
    }

    public class Isocahedron : AbstractPrimitiveDescriptor
    {
        public float Radius { get; set; }

        public override string PrimitiveType { get { return "Isocahedron"; } }

        public override void Initialize(Dictionary<string, object> properties)
        {
            this.Radius = (float)properties["Radius"];
        }

        public override IDX11Geometry GetGeometry(DX11RenderContext context)
        {
            return context.Primitives.Isocahedron(this);
        }
    }

    public class Octahedron : AbstractPrimitiveDescriptor
    {
        public float Radius { get; set; }

        public override string PrimitiveType { get { return "Octahedron"; } }

        public override void Initialize(Dictionary<string, object> properties)
        {
            this.Radius = (float)properties["Radius"];
        }

        public override IDX11Geometry GetGeometry(DX11RenderContext context)
        {
            return context.Primitives.Octahedron(this);
        }
    }

    public class Quad : AbstractPrimitiveDescriptor
    {
        public Vector2 Size { get; set; }

        public override string PrimitiveType { get { return "Quad"; } }

        public override void Initialize(Dictionary<string, object> properties)
        {
            this.Size = (Vector2)properties["Size"];
        }

        public override IDX11Geometry GetGeometry(DX11RenderContext context)
        {
            return context.Primitives.QuadNormals(this);
        }
    }

    public class QuadCross : Quad
    {
        public override string PrimitiveType { get { return "QuadCross"; } }
    }

    public class RoundRect : AbstractPrimitiveDescriptor
    {
        public RoundRect()
        {

        }

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

        public override string PrimitiveType { get { return "RoundRect"; } }

        public override void Initialize(Dictionary<string, object> properties)
        {
            this.InnerRadius = (Vector2)properties["InnerRadius"];
            this.OuterRadius = (float)properties["OuterRadius"];
            this.CornerResolution = (int)properties["CornerResolution"];
            this.EnableCenter = (bool)properties["EnableCenter"];
        }

        public override IDX11Geometry GetGeometry(DX11RenderContext context)
        {
            return context.Primitives.RoundRect(this);
        }

    }

    public class Segment : AbstractPrimitiveDescriptor
    {
        public float Phase { get; set; }
        public float Cycles { get; set; }
        public float InnerRadius { get; set; }
        public bool Flat { get; set; }
        public int Resolution { get; set; }

        public override string PrimitiveType { get { return "Segment"; } }

        public override void Initialize(Dictionary<string, object> properties)
        {
            this.Phase = (float)properties["Phase"];
            this.Cycles = (float)properties["Cycles"];
            this.InnerRadius = (float)properties["InnerRadius"];
            this.Resolution = (int)properties["Resolution"];
            this.Flat = (bool)properties["Flat"];
        }

        public override IDX11Geometry GetGeometry(DX11RenderContext context)
        {
            return context.Primitives.Segment(this);
        }
    }

    public class SegmentZ : AbstractPrimitiveDescriptor
    {
        public float Phase { get; set; }
        public float Cycles { get; set; }
        public float InnerRadius { get; set; }
        public float Z { get; set; }
        public int Resolution { get; set; }

        public override string PrimitiveType { get { return "SegmentZ"; } }

        public override void Initialize(Dictionary<string, object> properties)
        {
            this.Phase = (float)properties["Phase"];
            this.Cycles = (float)properties["Cycles"];
            this.InnerRadius = (float)properties["InnerRadius"];
            this.Resolution = (int)properties["Resolution"];
            this.Z = (float)properties["Z"];
        }

        public override IDX11Geometry GetGeometry(DX11RenderContext context)
        {
            return context.Primitives.SegmentZ(this);
        }
    }

    public class Sphere : AbstractPrimitiveDescriptor
    {
        public float Radius { get; set; }
        public int ResX { get; set; }
        public int ResY { get; set; }
        public float CyclesX { get; set; }
        public float CyclesY { get; set; }

        public override string PrimitiveType { get { return "Sphere"; } }

        public override void Initialize(Dictionary<string, object> properties)
        {
            this.Radius = (float)properties["Radius"];
            this.ResX = (int)properties["ResX"];
            this.ResY = (int)properties["ResY"];
            this.CyclesX = (float)properties["CyclesX"];
            this.CyclesY = (float)properties["CyclesY"];
        }

        public override IDX11Geometry GetGeometry(DX11RenderContext context)
        {
            return context.Primitives.Sphere(this);
        }
    }

    public class Tetrahedron : AbstractPrimitiveDescriptor
    {
        public float Radius { get; set; }

        public override string PrimitiveType { get { return "Tetrahedron"; } }

        public override void Initialize(Dictionary<string, object> properties)
        {
            this.Radius = (float)properties["Radius"];
        }

        public override IDX11Geometry GetGeometry(DX11RenderContext context)
        {
            return context.Primitives.Tetrahedron(this);
        }
    }

    public class Torus : AbstractPrimitiveDescriptor
    {
        public Torus()
        {
        }

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

        public override string PrimitiveType { get { return "Torus"; } }

        public override void Initialize(Dictionary<string, object> properties)
        {
            this.Radius = (float)properties["Radius"];
            this.ResolutionX = (int)properties["ResolutionX"];
            this.ResolutionY = (int)properties["ResolutionY"];

            this.Thickness = (float)properties["Thickness"];

            this.PhaseY = (float)properties["PhaseY"];
            this.PhaseX = (float)properties["PhaseX"];
            this.Rotation = (float)properties["Rotation"];
            this.CY = (float)properties["CY"];
        }

        public override IDX11Geometry GetGeometry(DX11RenderContext context)
        {
            return context.Primitives.Torus(this);
        }
    }




}
