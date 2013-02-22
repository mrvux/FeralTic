using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

using SlimDX.Direct3D11;
using SlimDX.DXGI;
using SlimDX;

using Device = SlimDX.Direct3D11.Device;


namespace FeralTic.DX11.Resources
{
    public unsafe class DX11StagingTexture2D : DX11Texture2D
    {
        [DllImport("msvcrt.dll", SetLastError = false)]
        static extern IntPtr memcpy(IntPtr dest, IntPtr src, int count);

        public DX11StagingTexture2D(DX11RenderContext context, int width, int height, Format format)
        {
            //device.ComPointer.
            this.desc = new Texture2DDescription()
            {
                ArraySize = 1,
                BindFlags = BindFlags.None,
                CpuAccessFlags = CpuAccessFlags.Read,
                Format = format,
                MipLevels = 1,
                OptionFlags = ResourceOptionFlags.None,
                Usage = ResourceUsage.Staging,
                Width = width,
                Height = height,
                SampleDescription= new SampleDescription(1,0)
            };

            this.context = context;
            this.Resource = new Texture2D(context.Device, desc);
        }

        public DataBox LockForRead()
        {
            return this.context.CurrentDeviceContext.MapSubresource(this.Resource, 0, 0, MapMode.Read, SlimDX.Direct3D11.MapFlags.None);
        }

        public void UnLock()
        {
            this.context.CurrentDeviceContext.UnmapSubresource(this.Resource, 0);  
        }

        public void CopyFrom(DX11Texture2D tex)
        {
            this.context.CurrentDeviceContext.CopyResource(tex.Resource, this.Resource);
        }

        private int rowpitch = -1;

        public int GetRowPitch()
        {
            if (rowpitch == -1)
            {
                DeviceContext ctx = this.context.CurrentDeviceContext;
                DataBox db = ctx.MapSubresource(this.Resource, 0, 0, MapMode.Read, SlimDX.Direct3D11.MapFlags.None);
                int val = 0;
                try
                {
                    val = db.RowPitch;
                }
                catch
                {

                }

                ctx.UnmapSubresource(this.Resource, 0);
                rowpitch = val;
            }
            return rowpitch;
        }


        public override void Dispose()
        {
            this.Resource.Dispose();
        }
    }
}
