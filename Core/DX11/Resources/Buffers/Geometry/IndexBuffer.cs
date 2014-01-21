using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SlimDX;
using SlimDX.Direct3D11;

using Buffer = SlimDX.Direct3D11.Buffer;

namespace FeralTic.DX11.Resources
{
    public class DX11IndexBuffer : IDisposable, IDX11RWResource
    {
        private DX11RenderContext context;

        public Buffer Buffer { get; protected set; }

        public UnorderedAccessView UAV { get { return uav; } }

        public ShaderResourceView SRV { get { return srv; } }

        public int IndicesCount { get; protected set; }

        private UnorderedAccessView uav = null;
        private ShaderResourceView srv = null;

        private SlimDX.DXGI.Format format;

        public DX11IndexBuffer(DX11RenderContext context, IntPtr ptr, int indicescount)
        {
            this.context = context;
            format = SlimDX.DXGI.Format.R32_UInt;
            this.Buffer = SlimDX.Direct3D11.Buffer.FromPointer(ptr);
            this.IndicesCount = indicescount;
        }

        public DX11IndexBuffer(DX11RenderContext context, DataStream initial,bool dynamic, bool dispose)
        {
            this.context = context;
            format = SlimDX.DXGI.Format.R32_UInt;

            BindFlags flags = BindFlags.IndexBuffer;

            if (context.ComputeShaderSupport) { flags |= BindFlags.ShaderResource; }

            BufferDescription bd = new BufferDescription()
            {
                BindFlags = flags,
                CpuAccessFlags = dynamic ? CpuAccessFlags.Write : CpuAccessFlags.None,
                OptionFlags = context.IsFeatureLevel11 ? ResourceOptionFlags.RawBuffer : ResourceOptionFlags.None,
                SizeInBytes = (int)initial.Length,
                Usage = dynamic ? ResourceUsage.Dynamic : ResourceUsage.Default,
            };

            initial.Position = 0;
            this.IndicesCount = (int)initial.Length / 4;
            this.Buffer = new SlimDX.Direct3D11.Buffer(context.Device, initial, bd);

            this.CreateSRV();

            if (dispose) { initial.Dispose(); }
        }

        public DX11IndexBuffer(DX11RenderContext context, int elementcount, bool uav = false, bool streamout = false)
        {
            this.context = context;
            this.IndicesCount = elementcount;
            format =SlimDX.DXGI.Format.R32_UInt;


            BufferDescription bd = new BufferDescription()
            {
                BindFlags = BindFlags.IndexBuffer,
                CpuAccessFlags = CpuAccessFlags.None,
                OptionFlags =ResourceOptionFlags.None,
                SizeInBytes = elementcount * sizeof(int),
                Usage = ResourceUsage.Default,
            };

            if (streamout)
            {

                bd.BindFlags |= BindFlags.StreamOutput;
            }

            if (uav && context.IsFeatureLevel11)
            {
                bd.BindFlags |= BindFlags.UnorderedAccess;
                bd.BindFlags |= BindFlags.ShaderResource;
                bd.OptionFlags |= ResourceOptionFlags.RawBuffer;
            }

            this.Buffer = new SlimDX.Direct3D11.Buffer(context.Device, bd);

            if (uav)
            {
                UnorderedAccessViewDescription uavd = new UnorderedAccessViewDescription()
                {
                    Format = SlimDX.DXGI.Format.R32_Typeless,
                    Dimension = UnorderedAccessViewDimension.Buffer,
                    Flags = UnorderedAccessViewBufferFlags.RawData,
                    ElementCount = elementcount
                };

                this.uav = new UnorderedAccessView(context.Device, this.Buffer, uavd);
                this.CreateSRV();
            }

            
        }

        private void CreateSRV()
        {
            if (this.context.IsFeatureLevel11)
            {
                ShaderResourceViewDescription srvd = new ShaderResourceViewDescription()
                {
                    Format = SlimDX.DXGI.Format.R32_Typeless,
                    Dimension = ShaderResourceViewDimension.ExtendedBuffer,
                    Flags = ShaderResourceViewExtendedBufferFlags.RawData,
                    ElementCount = this.IndicesCount,
                };


                this.srv = new ShaderResourceView(context.Device, this.Buffer, srvd);
            }
        }

        public void Bind(DeviceContext ctx)
        {
            ctx.InputAssembler.SetIndexBuffer(this.Buffer, this.format, 0);
        }

        public void Bind()
        {
            Bind(this.context.CurrentDeviceContext);
        }

        public void Dispose()
        {
            if (this.uav != null) { this.uav.Dispose(); }
            if (this.srv != null) { this.srv.Dispose(); }
            if (this.Buffer != null) { this.Buffer.Dispose(); }  
        }
    }
}
