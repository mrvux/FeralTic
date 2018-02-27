using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using SlimDX;
using SlimDX.Direct3D11;

using Buffer = SlimDX.Direct3D11.Buffer;

namespace FeralTic.DX11.Resources
{

    public class DX11ImmutableStructuredBuffer<T> : DX11StructuredBuffer<T>, IDX11ReadableStructureBuffer where T : struct 
    {
        public ShaderResourceView SRV { get; protected set; }

        public DX11ImmutableStructuredBuffer(Device dev, T[] initialData, int elementCount)
        {
            this.Stride = Marshal.SizeOf(typeof(T));
            this.Size = elementCount * this.Stride;
            this.ElementCount = elementCount;

            BufferDescription bd = new BufferDescription()
            {
                BindFlags = BindFlags.ShaderResource,
                CpuAccessFlags = CpuAccessFlags.None,
                OptionFlags = ResourceOptionFlags.StructuredBuffer,
                SizeInBytes = this.Size,
                StructureByteStride = this.Stride,
                Usage = ResourceUsage.Immutable
            };

            DataStream ds = new DataStream(initialData, true, true);

            this.Buffer = new Buffer(dev, ds, bd);
            this.SRV = new ShaderResourceView(dev, this.Buffer);
        }

        protected override void OnDispose()
        {
            if (this.SRV != null) { this.SRV.Dispose(); }
        }
    }
}
