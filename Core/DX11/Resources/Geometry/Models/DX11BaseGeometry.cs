using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SlimDX.Direct3D11;
using SlimDX;


namespace FeralTic.DX11.Resources
{
    public abstract class DX11BaseGeometry : IDX11Geometry
    {
        protected DX11RenderContext context;

        public DX11BaseGeometry(DX11RenderContext context, bool resourceowner = true)
        {
            this.context = context;
        }

        internal DX11BaseGeometry()
        {

        }

        /// <summary>
        /// Vertex Input Layout
        /// </summary>
        public InputElement[] InputLayout { get; set; }


        public PrimitiveTopology Topology
        {
            get; set;
        }

        public bool ValidateLayout(EffectPass pass, out InputLayout layout)
        {
            layout = null;
            try
            {
                layout = new InputLayout(context.Device, pass.Description.Signature, this.InputLayout);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public BoundingBox BoundingBox { get; set; }

        public bool HasBoundingBox { get; set; }

        public abstract void Draw();

        public abstract void Bind(InputLayout layout);

        public abstract void Dispose();

        public abstract IDX11Geometry ShallowCopy();

        public object Tag { get; set; }

        public string PrimitiveType { get; set; }
    }
}
