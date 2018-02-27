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
    public class DX11CopyDestStructuredBuffer : DX11StructuredBuffer, IDX11ReadableStructureBuffer
    {
        public ShaderResourceView SRV { get; protected set; }

        public DX11CopyDestStructuredBuffer(Device dev, BufferDescription desc)
        {
            this.Stride = desc.StructureByteStride;
            this.Size = desc.SizeInBytes;
            this.ElementCount = this.Size / this.Stride;

            this.Buffer = new Buffer(dev, desc);
            this.SRV = new ShaderResourceView(dev, this.Buffer);
        }

        protected override void OnDispose()
        {
            if (this.SRV != null) { this.SRV.Dispose(); }
            base.OnDispose();
        }
    }
}
