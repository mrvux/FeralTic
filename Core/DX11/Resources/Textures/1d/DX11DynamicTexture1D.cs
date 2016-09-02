using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SlimDX.Direct3D11;
using SlimDX.DXGI;
using SlimDX;
using Device = SlimDX.Direct3D11.Device;


namespace FeralTic.DX11.Resources
{
    public class DX11DynamicTexture1D : DX11Texture1D
    {


        public DX11DynamicTexture1D(DX11RenderContext context, int width, Format format) : base(context)
        {
            Texture1DDescription desc = new Texture1DDescription()
            {
                ArraySize = 1,
                BindFlags = BindFlags.ShaderResource,
                CpuAccessFlags = CpuAccessFlags.Write,
                Format = format,
                MipLevels = 1,
                OptionFlags = ResourceOptionFlags.None,
                Usage = ResourceUsage.Dynamic,
                Width = width,
            };

            this.Resource = new Texture1D(context.Device, desc);

            this.SRV = new ShaderResourceView(context.Device, this.Resource);
        }

        public DataStream LockForWrite(DeviceContext ctx)
        {
            DataBox db = ctx.MapSubresource(this.Resource, 0, 0, MapMode.WriteDiscard, SlimDX.Direct3D11.MapFlags.None);
            return db.Data;        
        }

        public void UnLock(DeviceContext ctx)
        {
            ctx.UnmapSubresource(this.Resource, 0);
        }

        public void WriteData(float[] data)
        {
            DeviceContext ctx = this.Resource.Device.ImmediateContext;
            DataBox db = ctx.MapSubresource(this.Resource, 0, 0, MapMode.WriteDiscard, SlimDX.Direct3D11.MapFlags.None);
            db.Data.WriteRange(data);
            ctx.UnmapSubresource(this.Resource, 0);
        }

        public void WriteData(Color4[] data)
        {
            DeviceContext ctx = this.Resource.Device.ImmediateContext;
            DataBox db = ctx.MapSubresource(this.Resource, 0, 0, MapMode.WriteDiscard, SlimDX.Direct3D11.MapFlags.None);
            db.Data.WriteRange(data);
            ctx.UnmapSubresource(this.Resource, 0);
        }

        public override void Dispose()
        {
            this.SRV.Dispose();
            this.Resource.Dispose();
        }
    }
}
