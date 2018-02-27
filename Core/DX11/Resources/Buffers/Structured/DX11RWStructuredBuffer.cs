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
            : this(dev, elementcount, stride, eDX11BufferMode.Default)//Dynamic default buffer
        {

        }

        public DX11RWStructuredBuffer(Device dev, int elementcount, int stride, eDX11BufferMode mode)
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
}
