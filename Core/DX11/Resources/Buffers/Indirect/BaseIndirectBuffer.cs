using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

using SlimDX.Direct3D11;
using SlimDX;
using Buffer = SlimDX.Direct3D11.Buffer;


namespace FeralTic.DX11.Resources
{
    public class BaseIndirectBuffer<T> where T : struct
    {
        private DX11RenderContext context;
        private Buffer staging;

        private DX11RWStructuredBuffer m_structuredBuffer;

        public Buffer Buffer { get; protected set; }

        public UnorderedAccessView UAV { get { return this.m_structuredBuffer.UAV; } }

        public IDX11RWStructureBuffer RWBuffer { get { return this.m_structuredBuffer; } }

        public BaseIndirectBuffer(DX11RenderContext context, T args)
        {
            this.context = context;

            int size = Marshal.SizeOf(args);

            BufferDescription bd = new BufferDescription();
            bd.Usage = ResourceUsage.Default;
            bd.StructureByteStride = 0;
            bd.SizeInBytes = size;
            bd.CpuAccessFlags = CpuAccessFlags.None;
            bd.BindFlags = BindFlags.None;
            bd.OptionFlags = ResourceOptionFlags.DrawIndirect;

            DataStream dsb = new DataStream(size, false, true);
            dsb.Position = 0;
            dsb.Write<T>(args);
            dsb.Position = 0;

            this.Buffer = new SlimDX.Direct3D11.Buffer(context.Device, dsb, bd);
            dsb.Dispose();

            m_structuredBuffer = new DX11RWStructuredBuffer(context.Device, size / 4, 4);
        }

        public void UpdateBuffer()
        {
            this.context.CurrentDeviceContext.CopyResource(this.m_structuredBuffer.Buffer, this.Buffer);
        }

        public void Dispose()
        {
            this.Buffer.Dispose();
        }

        public T RetrieveArgs()
        {

            if (staging == null)
            {
                int size = Marshal.SizeOf(typeof(T));

                BufferDescription bd = new BufferDescription();
                bd.Usage = ResourceUsage.Staging;
                bd.StructureByteStride = 0;
                bd.SizeInBytes = size;
                bd.CpuAccessFlags = CpuAccessFlags.Read;
                bd.BindFlags = BindFlags.None;
                bd.OptionFlags = ResourceOptionFlags.None;

                staging = new SlimDX.Direct3D11.Buffer(context.Device, bd);
            }

            this.context.CurrentDeviceContext.CopyResource(this.Buffer, staging);

            DataBox db = this.context.CurrentDeviceContext.MapSubresource(staging, MapMode.Read, MapFlags.None);
            T data = db.Data.Read<T>();
            this.context.CurrentDeviceContext.UnmapSubresource(staging, 0);

            return data;
        }
    }
}
