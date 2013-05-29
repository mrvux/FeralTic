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
        private delegate AbstractPrimitiveDescriptor CreatePrimitiveDelegate();


        private Dictionary<string, CreatePrimitiveDelegate> primitivecreator = new Dictionary<string,CreatePrimitiveDelegate>();

        private void InitializeDelegates()
        {
            this.primitivecreator.Add("Box", () => new Box());
            this.primitivecreator.Add("Cylinder", () => new Cylinder());
            this.primitivecreator.Add("Grid", () => new Grid());
            this.primitivecreator.Add("IcoGrid", () => new IcoGrid());
            this.primitivecreator.Add("Isocahedron", () => new Isocahedron());
            this.primitivecreator.Add("Octahedron", () => new Octahedron());
            this.primitivecreator.Add("Tetrahedron", () => new Tetrahedron());
            this.primitivecreator.Add("Quad", () => new Quad());
            this.primitivecreator.Add("RoundRect", () => new RoundRect());
            this.primitivecreator.Add("Segment", () => new Segment());
            this.primitivecreator.Add("SegmentZ", () => new SegmentZ());
            this.primitivecreator.Add("Sphere", () => new Sphere());
            this.primitivecreator.Add("Torus", () => new Torus());
        }

        public IDX11Geometry GetByPrimitiveType(string ptype, Dictionary<string,object> properties)
        {
            if (this.primitivecreator.ContainsKey(ptype))
            {
                AbstractPrimitiveDescriptor descriptor = this.primitivecreator[ptype]();
                descriptor.Initialize(properties);
                return descriptor.GetGeometry(this.context);
            }
            else
            {
                throw new Exception("Unknown Primitive Type");
            }
        }
    }

}
