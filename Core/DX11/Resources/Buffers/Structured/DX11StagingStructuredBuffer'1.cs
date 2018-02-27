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
    public class DX11StagingStructuredBuffer<T> : DX11StructuredBuffer<T> where T : struct
    {
        public DX11StagingStructuredBuffer(Device dev, int elementcount)
        {
            this.Size = elementcount * Marshal.SizeOf(typeof(T));
            this.ElementCount = elementcount;
            this.Stride = Marshal.SizeOf(typeof(T));

            BufferDescription bd = new BufferDescription()
            {
                BindFlags = BindFlags.None,
                CpuAccessFlags = CpuAccessFlags.Read | CpuAccessFlags.Write,
                OptionFlags = ResourceOptionFlags.StructuredBuffer,
                SizeInBytes = this.Size,
                StructureByteStride = Marshal.SizeOf(typeof(T)),
                Usage = ResourceUsage.Staging,
            };
            this.Buffer = new Buffer(dev, bd);
        }

        protected override void OnDispose()
        {

        }

        public DataStream MapForRead(DeviceContext ctx)
        {
            return ctx.MapSubresource(this.Buffer, MapMode.Read, MapFlags.None).Data;
        }

        public DataStream MapForWrite(DeviceContext ctx)
        {
            return ctx.MapSubresource(this.Buffer, MapMode.Write, MapFlags.None).Data;
        }

        public void UnMap(DeviceContext ctx)
        {
            ctx.UnmapSubresource(this.Buffer, 0);
        }
    }
}
