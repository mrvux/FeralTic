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
            this.Stride = Marshal.SizeOf(typeof(T));

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

            this.UAV = new UnorderedAccessView(dev, this.Buffer, uavd);
        }

        protected override void OnDispose()
        {
            if (this.SRV != null) { this.SRV.Dispose(); }
            if (this.UAV != null) { this.UAV.Dispose(); }
        }

        public void Clear(DeviceContext ctx, int value)
        {
            int[] mask = new int[] { value, value, value, value };
            ctx.ClearUnorderedAccessView(this.UAV, mask);
        }
    }
}
