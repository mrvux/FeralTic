using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SlimDX.Direct3D11;
using Buffer = SlimDX.Direct3D11.Buffer;
using SlimDX;

namespace FeralTic.DX11.Resources
{
    public struct DX11RawBufferFlags
    {
        public bool AllowIndexBuffer { get; set; }
        public bool AllowVertexBuffer { get; set; }
        public bool AllowArgumentBuffer { get; set; }
    }

    public class DX11RawBuffer:  IDX11RWResource, IDX11Buffer
    {
        public UnorderedAccessView UAV { get; protected set; }

        public ShaderResourceView SRV { get; protected set; }

        public Buffer Buffer { get; protected set; }

        public int Size { get; protected set; }

        public DX11RawBuffer(DX11RenderContext context, Buffer buffer)
        {
            this.Size = buffer.Description.SizeInBytes;
            this.Buffer = buffer;

            ShaderResourceViewDescription srvd = new ShaderResourceViewDescription()
            {
                Format = SlimDX.DXGI.Format.R32_Typeless,
                Dimension = ShaderResourceViewDimension.ExtendedBuffer,
                Flags = ShaderResourceViewExtendedBufferFlags.RawData,
                ElementCount = this.Size / 4
            };
            this.SRV = new ShaderResourceView(context.Device, this.Buffer, srvd);
        }

        public DX11RawBuffer(Device dev, int size, DX11RawBufferFlags flags= new DX11RawBufferFlags())
        {
            this.Size = size;

            BufferDescription bd = new BufferDescription()
            {
                BindFlags = BindFlags.ShaderResource | BindFlags.UnorderedAccess,
                CpuAccessFlags = CpuAccessFlags.None,
                OptionFlags = ResourceOptionFlags.RawBuffer,
                SizeInBytes = this.Size,
                Usage = ResourceUsage.Default,
            };

            if (flags.AllowIndexBuffer)
            {
                bd.BindFlags |= BindFlags.IndexBuffer;
            }

            if (flags.AllowVertexBuffer)
            {
                bd.BindFlags |= BindFlags.VertexBuffer;
            }

            if (flags.AllowArgumentBuffer)
            {
                bd.OptionFlags |= ResourceOptionFlags.DrawIndirect;
            }


            this.Buffer = new Buffer(dev, bd);

            ShaderResourceViewDescription srvd = new ShaderResourceViewDescription()
            {
                Format = SlimDX.DXGI.Format.R32_Typeless,
                Dimension = ShaderResourceViewDimension.ExtendedBuffer,
                Flags = ShaderResourceViewExtendedBufferFlags.RawData,
                ElementCount = size / 4
            };
            this.SRV = new ShaderResourceView(dev, this.Buffer, srvd);

            UnorderedAccessViewDescription uavd = new UnorderedAccessViewDescription()
            {
                Format = SlimDX.DXGI.Format.R32_Typeless,
                Dimension = UnorderedAccessViewDimension.Buffer,
                Flags = UnorderedAccessViewBufferFlags.RawData,
                ElementCount = size / 4
            };

            this.UAV = new UnorderedAccessView(dev, this.Buffer, uavd);
        }

        public void Dispose()
        {
            if (this.SRV != null) { this.SRV.Dispose(); }
            if (this.UAV != null) { this.UAV.Dispose(); }
            if (this.Buffer != null) { this.Buffer.Dispose(); }
        }
    }

    public class DX11DynamicRawBuffer : IDX11ReadableResource
    {
        private DX11RenderContext renderContext;

        public ShaderResourceView SRV { get; protected set; }

        public Buffer Buffer { get; protected set; }

        public int Size { get; protected set; }

        public DX11DynamicRawBuffer(DX11RenderContext renderContext, int size)
        {
            this.renderContext = renderContext;
            this.Size = size;

            BufferDescription bd = new BufferDescription()
            {
                BindFlags = BindFlags.ShaderResource,
                CpuAccessFlags = CpuAccessFlags.Write,
                OptionFlags = ResourceOptionFlags.RawBuffer,
                SizeInBytes = this.Size,
                Usage = ResourceUsage.Dynamic,
            };
            this.Buffer = new Buffer(this.renderContext.Device, bd);

            ShaderResourceViewDescription srvd = new ShaderResourceViewDescription()
            {
                Format = SlimDX.DXGI.Format.R32_Typeless,
                Dimension = ShaderResourceViewDimension.ExtendedBuffer,
                Flags = ShaderResourceViewExtendedBufferFlags.RawData,
                ElementCount = size / 4
            };
            this.SRV = new ShaderResourceView(this.renderContext.Device, this.Buffer, srvd);
        }

        public void WriteData(IntPtr ptr, int size)
        {
            DeviceContext ctx = this.renderContext.CurrentDeviceContext;
            DataBox db = ctx.MapSubresource(this.Buffer, MapMode.WriteDiscard, MapFlags.None);
            db.Data.WriteRange(ptr, size);
            ctx.UnmapSubresource(this.Buffer, 0);
        }

        public void Dispose()
        {
            if (this.SRV != null) { this.SRV.Dispose(); }
            if (this.Buffer != null) { this.Buffer.Dispose(); }
        }
    }

    public class DX11StagingRawBuffer : IDX11ReadableResource
    {
        public ShaderResourceView SRV { get; protected set; }

        public Buffer Buffer { get; protected set; }

        public int Size { get; protected set; }

        public DX11StagingRawBuffer(Device dev, int size)
        {
            this.Size = size;

            BufferDescription bd = new BufferDescription()
            {
                BindFlags = BindFlags.None,
                CpuAccessFlags = CpuAccessFlags.Read | CpuAccessFlags.Write,
                OptionFlags = ResourceOptionFlags.None,
                SizeInBytes = this.Size,
                Usage = ResourceUsage.Staging,
            };
            this.Buffer = new Buffer(dev, bd);

        }

        public void Dispose()
        {

            if (this.Buffer != null) { this.Buffer.Dispose(); }
        }

        public DataStream MapForRead(DeviceContext ctx)
        {
            return ctx.MapSubresource(this.Buffer, MapMode.Read, MapFlags.None).Data;
        }

        public void UnMap(DeviceContext ctx)
        {
            ctx.UnmapSubresource(this.Buffer, 0);
        }
    }
}
