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
    public class DX11DynamicStructuredBuffer<T> : DX11StructuredBuffer<T>, IDX11ReadableStructureBuffer where T : struct
    {
        public ShaderResourceView SRV { get; protected set; }

        private DX11RenderContext context;


        public DX11DynamicStructuredBuffer(DX11RenderContext context, int cnt)
        {
            this.context = context;
            this.Size = cnt * Marshal.SizeOf(typeof(T));
            this.ElementCount = cnt;
            this.Stride = Marshal.SizeOf(typeof(T));

            BufferDescription bd = new BufferDescription()
            {
                BindFlags = BindFlags.ShaderResource,
                CpuAccessFlags = CpuAccessFlags.Write,
                OptionFlags = ResourceOptionFlags.StructuredBuffer,
                SizeInBytes = this.Size,
                StructureByteStride = Marshal.SizeOf(typeof(T)),
                Usage = ResourceUsage.Dynamic
            };

            this.Buffer = new Buffer(context.Device, bd);
            this.SRV = new ShaderResourceView(context.Device, this.Buffer);
        }

        public DX11DynamicStructuredBuffer(DX11RenderContext context, Buffer buffer, int cnt) //Dynamic default buffer
        {
            this.context = context;
            this.Size = cnt;
            this.Buffer = buffer;
            this.SRV = new ShaderResourceView(context.Device, this.Buffer);
            this.Stride = Marshal.SizeOf(typeof(T));
        }

        public DX11DynamicStructuredBuffer(DX11RenderContext context, IntPtr initial, int cnt) //Dynamic default buffer
        {
            this.context = context;
            this.Size = cnt * Marshal.SizeOf(typeof(T));
            this.ElementCount = cnt;
            this.Stride = Marshal.SizeOf(typeof(T));

            BufferDescription bd = new BufferDescription()
            {
                BindFlags = BindFlags.ShaderResource,
                CpuAccessFlags = CpuAccessFlags.None,
                OptionFlags = ResourceOptionFlags.StructuredBuffer,
                SizeInBytes = this.Size,
                StructureByteStride = Marshal.SizeOf(typeof(T)),
                Usage = ResourceUsage.Immutable
            };

            DataStream ds = new DataStream(initial, this.Size, true, false);
            ds.Position = 0;

            this.Buffer = new Buffer(context.Device, ds, bd);
            this.SRV = new ShaderResourceView(context.Device, this.Buffer);
        }

        public void WriteData(T[] data)
        {
            WriteData(data, 0, data.Length);
        }

        public void WriteData(T[] data, int offset, int count)
        {
            DeviceContext ctx = this.context.CurrentDeviceContext;
            DataBox db = ctx.MapSubresource(this.Buffer, MapMode.WriteDiscard, MapFlags.None);
            db.Data.WriteRange(data, offset, count);
            ctx.UnmapSubresource(this.Buffer, 0);
        }

        public void WriteData(IntPtr ptr)
        {
            DeviceContext ctx = this.context.CurrentDeviceContext;
            DataBox db = ctx.MapSubresource(this.Buffer, MapMode.WriteDiscard, MapFlags.None);
            db.Data.WriteRange(ptr, this.Size);
            ctx.UnmapSubresource(this.Buffer, 0);
        }

        protected override void OnDispose()
        {
            if (this.SRV != null) { this.SRV.Dispose(); }
        }
    }
}
