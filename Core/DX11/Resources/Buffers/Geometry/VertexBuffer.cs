using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SlimDX.Direct3D11;

using Buffer = SlimDX.Direct3D11.Buffer;
using SlimDX;
using SlimDX.D3DCompiler;

namespace FeralTic.DX11.Resources
{
    public class DX11VertexBuffer : IDX11Resource, IDisposable
    {
        private DX11RenderContext context;

        private Buffer buffer;
        private readonly int vertexCount;
        private readonly int vertexSize;

        public Buffer Buffer
        {
            get { return this.buffer; }
        }

        public int VertexCount
        {
            get { return this.vertexCount; }
        }

        public int VertexSize
        {
            get { return this.vertexSize; }
        }

        public bool AllowStreamOutput { get; private set; }
        public int TotalSize { get; private set; }

        /// <summary>
        /// Vertex Input Layout
        /// </summary>
        public InputElement[] InputLayout { get; set; }

        private DX11VertexBuffer(DX11RenderContext context, BufferDescription bufferDescription, int vertexCount, int vertexSize, DataStream initialData)
        {
            if (!bufferDescription.BindFlags.HasFlag(BindFlags.VertexBuffer))
                throw new ArgumentException("bufferDescription", "Sohuld have VertexBuffer bind flag");
            if (vertexCount <= 0)
                throw new ArgumentOutOfRangeException("vertexCount", "must be greater than 0");
            if (vertexSize <= 0)
                throw new ArgumentOutOfRangeException("vertexSize", "must be greater than 0");

            this.context = context;
            this.buffer = new SlimDX.Direct3D11.Buffer(context.Device, initialData, bufferDescription);
            this.vertexSize = vertexSize;
            this.vertexCount = vertexCount;
        }

        public static DX11VertexBuffer CreateDynamic(DX11RenderContext context, int vertexCount, int vertexSize, bool allowRawView)
        {
            BindFlags bindFlags = BindFlags.VertexBuffer;
            ResourceOptionFlags optionFlags = ResourceOptionFlags.None;
            if (allowRawView)
            {
                bindFlags |= BindFlags.ShaderResource;
                optionFlags |= ResourceOptionFlags.RawBuffer;
            }

            BufferDescription desc = new BufferDescription()
            {
                BindFlags = bindFlags,
                CpuAccessFlags = CpuAccessFlags.Write,
                OptionFlags = optionFlags,
                SizeInBytes = vertexCount * vertexSize,
                Usage = ResourceUsage.Dynamic
            };

            return new DX11VertexBuffer(context, desc, vertexCount, vertexSize, null);
        }

        public static DX11VertexBuffer CreateImmutable(DX11RenderContext context, int vertexCount, int vertexSize, DataStream initialData, bool allowRawView)
        {
            if (initialData == null)
                throw new ArgumentNullException("initialData");

            BindFlags bindFlags = BindFlags.VertexBuffer;
            ResourceOptionFlags optionFlags = ResourceOptionFlags.None;
            if (allowRawView)
            {
                bindFlags |= BindFlags.ShaderResource;
                optionFlags |= ResourceOptionFlags.RawBuffer;
            }

            BufferDescription desc = new BufferDescription()
            {
                BindFlags = bindFlags,
                CpuAccessFlags = CpuAccessFlags.None,
                OptionFlags = optionFlags,
                SizeInBytes = vertexCount * vertexSize,
                Usage = ResourceUsage.Immutable
            };

            return new DX11VertexBuffer(context, desc, vertexCount, vertexSize, initialData);
        }

        public static DX11VertexBuffer CreateStreamOutput(DX11RenderContext context, int vertexCount, int vertexSize, bool allowRawView)
        {
            BindFlags bindFlags = BindFlags.VertexBuffer | BindFlags.StreamOutput;
            ResourceOptionFlags optionFlags = ResourceOptionFlags.None;
            if (allowRawView)
            {
                bindFlags |= BindFlags.ShaderResource;
                optionFlags |= ResourceOptionFlags.RawBuffer;
            }

            BufferDescription desc = new BufferDescription()
            {
                BindFlags = bindFlags,
                CpuAccessFlags = CpuAccessFlags.None,
                OptionFlags = optionFlags,
                SizeInBytes = vertexCount * vertexSize,
                Usage = ResourceUsage.Default
            };

            return new DX11VertexBuffer(context, desc, vertexCount, vertexSize, null);
        }

        public void Bind(InputLayout layout, int slot = 0)
        {
            this.context.CurrentDeviceContext.InputAssembler.InputLayout = layout;
            this.context.CurrentDeviceContext.InputAssembler.SetVertexBuffers(slot, new VertexBufferBinding(this.Buffer, this.VertexSize,0));
        }

        public bool TryValidateLayout(ShaderSignature shaderSignature, out InputLayout result)
        {
            try
            {
                result = new InputLayout(this.context.Device, shaderSignature, this.InputLayout);
                return true;
            }
            catch
            {
                result = null;
                return false;
            }
        }

        public void Dispose()
        {
            if (this.Buffer != null) { this.Buffer.Dispose(); }
        }
    }
}
