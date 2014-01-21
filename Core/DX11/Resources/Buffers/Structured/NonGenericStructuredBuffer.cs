using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SlimDX.Direct3D11;
using Buffer = SlimDX.Direct3D11.Buffer;
using System.Runtime.InteropServices;
using SlimDX;

namespace FeralTic.DX11.Resources
{
    public unsafe abstract class DX11StructuredBuffer : IDX11StructuredBuffer
    {
        public Buffer Buffer { get; protected set; }

        public int Size { get; protected set; }

        public int Stride { get; protected set; }

        public int ElementCount { get; protected set; }

        protected virtual void OnDispose() { }

        public void Copy(DeviceContext ctx, DX11StructuredBuffer destination)
        {
            if (this.Size == destination.Size)
            {
                ctx.CopyResource(this.Buffer, destination.Buffer);
            }
            else
            {
                throw new Exception("Invalid Matching sizes");
            }
        }

        public void Dispose()
        {
            if (this.Buffer != null) { this.Buffer.Dispose(); }
        }
    }

    
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
    
    public class DX11RWStructuredBuffer : DX11StructuredBuffer, IDX11RWStructureBuffer
    {
        public eDX11BufferMode BufferType { get; protected set; }

        public UnorderedAccessView UAV { get; protected set; }

        public ShaderResourceView SRV { get; protected set; }

        public DX11RWStructuredBuffer(int elementcount, int stride, IntPtr uav, IntPtr srv)
        {
            this.ElementCount = elementcount;
            this.Stride = stride;
            this.UAV = UnorderedAccessView.FromPointer(uav);
            //this.UAV.Dispose();
            this.SRV = ShaderResourceView.FromPointer(srv);
            //this.SRV.Dispose();
        }


        public DX11RWStructuredBuffer(Device dev, int elementcount, int stride)
            : this(dev, elementcount,stride, eDX11BufferMode.Default)//Dynamic default buffer
        {

        }

        public DX11RWStructuredBuffer(Device dev, int elementcount,int stride, eDX11BufferMode mode)
        {
            this.Stride = stride;
            this.Size = elementcount * stride;
            this.ElementCount = elementcount;
            this.BufferType = mode;

            BufferDescription bd = new BufferDescription()
            {
                BindFlags = BindFlags.ShaderResource | BindFlags.UnorderedAccess,
                CpuAccessFlags = CpuAccessFlags.None,
                OptionFlags = ResourceOptionFlags.StructuredBuffer,
                SizeInBytes = this.Size,
                StructureByteStride = this.Stride,
                Usage = ResourceUsage.Default,
            };
            this.Buffer = new Buffer(dev, bd);
            this.SRV = new ShaderResourceView(dev, this.Buffer);

            UnorderedAccessViewDescription uavd = new UnorderedAccessViewDescription()
            {
                ElementCount = this.ElementCount,
                Format = SlimDX.DXGI.Format.Unknown,
                Dimension = UnorderedAccessViewDimension.Buffer,
                Flags = (UnorderedAccessViewBufferFlags)mode
            };

            this.UAV = new UnorderedAccessView(dev, this.Buffer, uavd);
        }

        public void Clear(DeviceContext ctx, int value)
        {
            int[] mask = new int[] { value, value, value, value };
            ctx.ClearUnorderedAccessView(this.UAV, mask);
        }

        public void ResetCounter(DeviceContext ctx)
        {
            int slot = -1;
            bool found = false;
            UnorderedAccessView[] uavs = ctx.ComputeShader.GetUnorderedAccessViews(0, 8);
            foreach (UnorderedAccessView ua in uavs)
            {
                slot++;
                if (ua == this.UAV)
                {
                    found = true;
                    break;
                }
            }
            if (found)
            {
                ctx.ComputeShader.SetUnorderedAccessView(this.UAV, slot, 0);
            }
        }

        protected override void OnDispose()
        {
            if (this.SRV != null) { this.SRV.Dispose(); }
            if (this.UAV != null) { this.UAV.Dispose(); }
            base.OnDispose();
        }
    }

    public class DX11StagingStructuredBuffer : DX11StructuredBuffer
    {
        public DX11StagingStructuredBuffer(Device dev, int elementcount, int stride)
        {
            this.Size = elementcount * stride;
            this.ElementCount = elementcount;
            this.Stride = stride;

            BufferDescription bd = new BufferDescription()
            {
                BindFlags = BindFlags.None,
                CpuAccessFlags = CpuAccessFlags.Read | CpuAccessFlags.Write,
                OptionFlags = ResourceOptionFlags.StructuredBuffer,
                SizeInBytes = this.Size,
                StructureByteStride = stride,
                Usage = ResourceUsage.Staging,
            };
            this.Buffer = new Buffer(dev, bd);
        }

        protected override void OnDispose()
        {
            base.OnDispose();
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
