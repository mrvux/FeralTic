using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SlimDX.Direct3D11;

using Buffer = SlimDX.Direct3D11.Buffer;
using SlimDX;


namespace FeralTic.DX11.Resources
{
    public class DX11VertexBuffer : IDX11Resource, IDisposable
    {
        private DX11RenderContext context;

        public Buffer Buffer { get; protected set; }
        public int VerticesCount { get; protected set; }
        public int VertexSize { get; protected set; }

        public bool AllowStreamOutput { get; private set; }
        public int TotalSize { get; private set; }

        /// <summary>
        /// Vertex Input Layout
        /// </summary>
        public InputElement[] InputLayout { get; set; }

        public DX11VertexBuffer(DX11RenderContext context, int verticescount, int vertexsize, bool allowstreamout)
        {
            this.context = context;
            this.TotalSize = verticescount * vertexsize;
            this.AllowStreamOutput = allowstreamout;

            BindFlags flags = BindFlags.VertexBuffer;

            if (allowstreamout)
            {
                flags |= BindFlags.StreamOutput;
            }

            BufferDescription bd = new BufferDescription()
            {
                BindFlags = flags,
                CpuAccessFlags = CpuAccessFlags.None,
                OptionFlags = ResourceOptionFlags.None,
                SizeInBytes = this.TotalSize,
                Usage = ResourceUsage.Default
            };

            this.Buffer = new SlimDX.Direct3D11.Buffer(context.Device, bd);
            this.VertexSize = vertexsize;
            this.VerticesCount = verticescount;
        }

        public DX11VertexBuffer(DX11RenderContext context, DataStream initial,int verticescount,int vertexsize, bool dynamic, bool dispose)
        {
            this.context = context;

            BufferDescription bd = new BufferDescription()
            {
                BindFlags = BindFlags.VertexBuffer,
                CpuAccessFlags = dynamic ? CpuAccessFlags.Write : CpuAccessFlags.None,
                OptionFlags = ResourceOptionFlags.None,
                SizeInBytes = (int)initial.Length,
                Usage = dynamic ? ResourceUsage.Dynamic : ResourceUsage.Immutable,
            };

            initial.Position = 0;
            this.Buffer = new SlimDX.Direct3D11.Buffer(context.Device, initial, bd);
            this.VertexSize = vertexsize;
            this.VerticesCount = verticescount;

            if (dispose) { initial.Dispose(); }
        }

        public void Bind(InputLayout layout, int slot = 0)
        {
            this.context.CurrentDeviceContext.InputAssembler.InputLayout = layout;
            this.context.CurrentDeviceContext.InputAssembler.SetVertexBuffers(slot, new VertexBufferBinding(this.Buffer, this.VertexSize,0));
        }

        public void Dispose()
        {
            if (this.Buffer != null) { this.Buffer.Dispose(); }
        }
    }
}
