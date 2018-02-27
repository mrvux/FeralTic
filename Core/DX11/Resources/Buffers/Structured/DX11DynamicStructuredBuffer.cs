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
    public class DX11DynamicStructuredBuffer : DX11StructuredBuffer, IDX11ReadableStructureBuffer
    {
        public ShaderResourceView SRV { get; protected set; }

        public DX11DynamicStructuredBuffer(Device dev, int cnt, int stride)
        {
            this.Stride = stride;
            this.Size = cnt * stride;
            this.ElementCount = cnt;

            BufferDescription bd = new BufferDescription()
            {
                BindFlags = BindFlags.ShaderResource,
                CpuAccessFlags = CpuAccessFlags.Write,
                OptionFlags = ResourceOptionFlags.StructuredBuffer,
                SizeInBytes = this.Size,
                StructureByteStride = this.Stride,
                Usage = ResourceUsage.Dynamic
            };

            this.Buffer = new Buffer(dev, bd);
            this.SRV = new ShaderResourceView(dev, this.Buffer);
        }

        public DX11DynamicStructuredBuffer(Device dev, Buffer buffer, int cnt) //Dynamic default buffer
        {
            this.Size = cnt;
            this.Buffer = buffer;
            this.Stride = buffer.Description.StructureByteStride;
            this.SRV = new ShaderResourceView(dev, this.Buffer);
        }

        protected override void OnDispose()
        {
            if (this.SRV != null) { this.SRV.Dispose(); }
            base.OnDispose();
        }

        public void WriteData(IntPtr ptr)
        {
            DeviceContext ctx = this.Buffer.Device.ImmediateContext;
            DataBox db = ctx.MapSubresource(this.Buffer, MapMode.WriteDiscard, MapFlags.None);
            db.Data.WriteRange(ptr, this.Size);
            ctx.UnmapSubresource(this.Buffer, 0);
        }

        public DataStream MapForWrite(DeviceContext ctx)
        {
            return ctx.MapSubresource(this.Buffer, MapMode.WriteDiscard, MapFlags.None).Data;
        }

        public void UnMap(DeviceContext ctx)
        {
            ctx.UnmapSubresource(this.Buffer, 0);
        }
    }
}
