using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

using SlimDX;
using SlimDX.Direct3D11;

using Buffer = SlimDX.Direct3D11.Buffer;


namespace FeralTic.DX11.Resources
{
    public enum eDX11BufferMode { Default = 0, Append = 2, Counter = 4 }

    public interface IDX11StructuredBuffer : IDX11Resource 
    {
        Buffer Buffer { get; }
        int ElementCount { get; }
        int Stride { get; }
    }

    public interface IDX11ReadableStructureBuffer : IDX11ReadableResource, IDX11StructuredBuffer { }

    public interface IDX11RWStructureBuffer : IDX11RWResource, IDX11StructuredBuffer, IDX11ReadableStructureBuffer 
    {
        eDX11BufferMode BufferType { get; }
        void Clear(DeviceContext ctx, int value);
        void ResetCounter(DeviceContext ctx);
    }

    public unsafe abstract class DX11StructuredBuffer<T> : IDX11StructuredBuffer where T : struct
    {
        public Buffer Buffer { get; protected set; }

        public int Size { get; protected set; }

        public int ElementCount { get; protected set; }

        public int Stride { get; protected set; }

        protected abstract void OnDispose();

        public void Copy(DeviceContext ctx, DX11StructuredBuffer<T> destination)
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

        public virtual void Dispose()
        {
            this.OnDispose();
            if (this.Buffer != null) { this.Buffer.Dispose(); }
        }
    }

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

            this.Buffer = new Buffer(context.Device,ds, bd);
            this.SRV = new ShaderResourceView(context.Device, this.Buffer);
        }

        public void WriteData(T[] data)
        {
            DeviceContext ctx = this.context.CurrentDeviceContext;
            DataBox db = ctx.MapSubresource(this.Buffer, MapMode.WriteDiscard, MapFlags.None);
            db.Data.WriteRange(data);
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


    public class DX11RWStructuredBuffer<T> : DX11StructuredBuffer<T>, IDX11RWStructureBuffer where T : struct
    {
        public UnorderedAccessView UAV { get; protected set; }

        public ShaderResourceView SRV { get; protected set; }

        public eDX11BufferMode BufferType { get; protected set; }

        public DX11RWStructuredBuffer(Device dev, int elementcount)
            : this(dev, elementcount, eDX11BufferMode.Default)//Dynamic default buffer
        {

        }

        public DX11RWStructuredBuffer(Device dev, int elementcount, eDX11BufferMode mode)
        {
            this.Size = elementcount * Marshal.SizeOf(typeof(T));
            this.ElementCount = elementcount;
            this.BufferType = mode;

            BufferDescription bd = new BufferDescription()
            {
                BindFlags = BindFlags.ShaderResource | BindFlags.UnorderedAccess,
                CpuAccessFlags = CpuAccessFlags.None,
                OptionFlags = ResourceOptionFlags.StructuredBuffer,
                SizeInBytes = this.Size,
                StructureByteStride = Marshal.SizeOf(typeof(T)),
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

            //if (allowappend) { uavd.Flags = UnorderedAccessViewBufferFlags.AllowAppend; }

            //Counter
            //uavd.Flags |= (UnorderedAccessViewBufferFlags)4;

            this.UAV = new UnorderedAccessView(dev, this.Buffer, uavd);
        }

        protected override void OnDispose()
        {
            if (this.SRV != null) { this.SRV.Dispose(); }
            if (this.UAV != null) { this.UAV.Dispose(); }
        }

        public void Clear(DeviceContext ctx, int value)
        {
            int[] mask = new int [] {value,value,value,value};
            ctx.ClearUnorderedAccessView(this.UAV, mask);
            //BoundingBox b = new BoundingBox(
        }

        public void ResetCounter(DeviceContext ctx)
        {
            int[] zeros = { 0, 0, 0, 0, 0, 0, 0, 0 };
            ctx.ComputeShader.SetUnorderedAccessViews(new UnorderedAccessView[1] { this.UAV }, 0, 1, zeros);
        }


    }

    
    public class DX11StagingStructuredBuffer<T> : DX11StructuredBuffer<T> where T : struct
    {
        public DX11StagingStructuredBuffer(Device dev, int elementcount)
        {
            this.Size = elementcount * Marshal.SizeOf(typeof(T));
            this.ElementCount = elementcount;

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
