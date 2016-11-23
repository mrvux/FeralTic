using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SlimDX;
using SlimDX.Direct3D11;

namespace FeralTic.DX11.Resources
{
    public class DX11InvalidGeometry : IDX11Geometry
    {
        public BoundingBox BoundingBox
        {
            get;set;
        }

        public bool HasBoundingBox
        {
            get;set;
        }

        public InputElement[] InputLayout
        {
            get;set;
        }

        public string PrimitiveType
        {
            get
            {
                return "Invalid";
            }
            set { }
        }

        public object Tag
        {
            get;set;
        }

        public PrimitiveTopology Topology
        {
            get { return PrimitiveTopology.Undefined; }
            set { }
        }

        public void Bind(InputLayout layout)
        {

        }

        public void Bind(DeviceContext ctx, InputLayout layout)
        {

        }

        public void Dispose()
        {

        }

        public void Draw()
        {

        }

        public void Draw(DeviceContext ctx)
        {

        }

        public IDX11Geometry ShallowCopy()
        {
            return this;
        }

        public bool ValidateLayout(EffectPass pass, out InputLayout layout)
        {
            layout = null;
            return false;
        }
    }
}
