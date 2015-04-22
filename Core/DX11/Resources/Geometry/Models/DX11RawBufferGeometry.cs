using SlimDX;
using SlimDX.Direct3D11;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FeralTic.DX11.Resources
{
    public class RawBufferGeometry : IDX11Geometry
    {
        public class Properties
        {
            public bool AllowIndexBuffer;
            public int IndexBufferOffset;

            public int DrawOffset;

            public int[] VertexBufferStrides;
            public int[] VertexBufferOffsets;
        }

        private readonly DX11RenderContext context;
        private IDX11Buffer buffer;

        private Properties props = new Properties();

        public Properties Prop
        {
            get
            {
                return this.props;
            }
        }

        public IDX11Buffer Buffer
        {
            set { this.buffer = value; }
        }

        public RawBufferGeometry(DX11RenderContext context)
        {
            this.context = context;
        }

        private RawBufferGeometry(RawBufferGeometry self)
        {
            this.buffer = self.buffer;
            this.context = self.context;
            this.BoundingBox = self.BoundingBox;
            this.InputLayout = self.InputLayout;
            this.props = self.props;
            this.Tag = self.Tag;
            this.Topology = self.Topology;
        }

        public BoundingBox BoundingBox
        {
            get; set;
        }

        public bool HasBoundingBox
        {
            get { return false; }
            set { }
        }

        public InputElement[] InputLayout
        {
            get; set;
        }

        public string PrimitiveType
        {
            get { return "RawBuffer"; }
            set { }
        }

        public object Tag
        {
            get; set;
        }

        public PrimitiveTopology Topology
        {
            get; set;
        }

        public void Bind(InputLayout layout)
        {
            this.Bind(this.context.CurrentDeviceContext, layout);
        }

        public void Bind(DeviceContext ctx, InputLayout layout)
        {
            ctx.InputAssembler.InputLayout = layout;
            ctx.InputAssembler.PrimitiveTopology = this.Topology;

            if (this.props.AllowIndexBuffer)
            {
                ctx.InputAssembler.SetIndexBuffer(this.buffer.Buffer, SlimDX.DXGI.Format.R32_UInt, this.props.IndexBufferOffset);
            }
            else
            {
                ctx.InputAssembler.SetIndexBuffer(null, SlimDX.DXGI.Format.R32_UInt, 0);
            }

            VertexBufferBinding[] bindings = new VertexBufferBinding[this.props.VertexBufferOffsets.Length];
            for (int i = 0; i < this.props.VertexBufferOffsets.Length; i++)
            {
                VertexBufferBinding vb = new VertexBufferBinding(this.buffer.Buffer, this.props.VertexBufferStrides[i], this.props.VertexBufferOffsets[i]);
                bindings[i] = vb;
            }

            ctx.InputAssembler.SetVertexBuffers(0, bindings);
        }

        public void Dispose()
        {

        }

        public void Draw()
        {
            this.Draw(this.context.CurrentDeviceContext);
        }

        public void Draw(DeviceContext ctx)
        {
            if (this.props.AllowIndexBuffer)
            {
                ctx.DrawIndexedInstancedIndirect(this.buffer.Buffer, this.Prop.DrawOffset);
            }
            else
            {
                ctx.DrawInstancedIndirect(this.buffer.Buffer, this.Prop.DrawOffset);
            }
        }

        public IDX11Geometry ShallowCopy()
        {
            return new RawBufferGeometry(this);
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
    }
}
