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
        private Dictionary<string, Type> primitivetypes = new Dictionary<string, Type>();

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

            this.primitivetypes.Add("Box", typeof(Box));
            this.primitivetypes.Add("Cylinder", typeof(Cylinder));
            this.primitivetypes.Add("Grid", typeof(Grid));
            this.primitivetypes.Add("IcoGrid", typeof(IcoGrid));
            this.primitivetypes.Add("Isocahedron", typeof(Isocahedron));
            this.primitivetypes.Add("Octahedron", typeof(Octahedron));
            this.primitivetypes.Add("Tetrahedron", typeof(Tetrahedron));
            this.primitivetypes.Add("Quad", typeof(Quad));
            this.primitivetypes.Add("RoundRect", typeof(RoundRect));
            this.primitivetypes.Add("Segment", typeof(Segment));
            this.primitivetypes.Add("SegmentZ", typeof(SegmentZ));
            this.primitivetypes.Add("Sphere", typeof(Sphere));
            this.primitivetypes.Add("Torus", typeof(Torus));
        }

        public Type GetDescriptorType(string ptype)
        {
            if (this.primitivetypes.ContainsKey(ptype))
            {
                return this.primitivetypes[ptype];
            }
            else
            {
                throw new Exception("Unknown Primitive Type");
            }
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
