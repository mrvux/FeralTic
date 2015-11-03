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
    public unsafe class DX11DynamicTexture2D : DX11Texture2D
    {
        [DllImport("msvcrt.dll", SetLastError = false)]
        static extern IntPtr memcpy(IntPtr dest, IntPtr src, int count);

        public DX11DynamicTexture2D(DX11RenderContext context, int width, int height, Format format)
        {
            //device.ComPointer.
            this.desc = new Texture2DDescription()
            {
                ArraySize = 1,
                BindFlags = BindFlags.ShaderResource,
                CpuAccessFlags = CpuAccessFlags.Write,
                Format = format,
                MipLevels = 1,
                OptionFlags = ResourceOptionFlags.None,
                Usage = ResourceUsage.Dynamic,
                Width = width,
                Height = height,
                SampleDescription= new SampleDescription(1,0)
            };

            this.context = context;
            this.Resource = new Texture2D(context.Device, desc);
            this.SRV = new ShaderResourceView(context.Device, this.Resource);
        }



        public void WriteData(IntPtr ptr, int len)
        {
            DeviceContext ctx = this.context.CurrentDeviceContext;
            DataBox db = ctx.MapSubresource(this.Resource, 0, 0, MapMode.WriteDiscard, SlimDX.Direct3D11.MapFlags.None);
            //db.Data.WriteRange(ptr, (long)len);
            memcpy(db.Data.DataPointer, ptr, len);
            ctx.UnmapSubresource(this.Resource, 0);  
        }


        public void WriteDataPitch(IntPtr ptr, int len, int rowsize = 4)
        {
            DeviceContext ctx = this.context.CurrentDeviceContext;
            DataBox db = ctx.MapSubresource(this.Resource, 0, 0, MapMode.WriteDiscard, SlimDX.Direct3D11.MapFlags.None);

            int w= desc.Width;
            int pos = 0;
            int stride = w*rowsize;

            byte* data = (byte*)ptr.ToPointer();
            for (int i = 0; i < desc.Height; i++)
            {
                db.Data.WriteRange((IntPtr)data, desc.Width * rowsize);
                
                pos += db.RowPitch;
                db.Data.Position = pos;
                data += stride;
            }
            ctx.UnmapSubresource(this.Resource, 0);
        }

        /*public void WriteDataPitch(byte[] b, int len, int rowsize = 4)
        {
            DeviceContext ctx = this.context.CurrentDeviceContext;
            DataBox db = ctx.MapSubresource(this.Resource, 0, 0, MapMode.WriteDiscard, SlimDX.Direct3D11.MapFlags.None);

            int w = desc.Width;
            int pos = 0;
            int stride = w * rowsize;

            byte* data = (byte*)ptr.ToPointer();
            for (int i = 0; i < desc.Height; i++)
            {
                db.Data.WriteRange((IntPtr)data, desc.Width * rowsize);

                pos += db.RowPitch;
                db.Data.Position = pos;
                data += stride;
            }
            ctx.UnmapSubresource(this.Resource, 0);
        }*/



        public void WriteData(float[] data, int chans)
        {
            DeviceContext ctx = this.context.CurrentDeviceContext;
            DataBox db = ctx.MapSubresource(this.Resource,0,0, MapMode.WriteDiscard,SlimDX.Direct3D11.MapFlags.None);

            int pos = 0;
            int idx = 0;
            for (int i = 0; i < desc.Height; i++)
            {
                for (int j = 0; j < desc.Width; j++)
                {
                    for (int k = 0; k < chans; k++)
                    {
                        db.Data.Write(data[idx]);
                        idx++;
                    }
                }
                pos += db.RowPitch;
                db.Data.Position = pos;
            }
            //db.Data.WriteRange(data);
            ctx.UnmapSubresource(this.Resource, 0);        
        }

        private int rowpitch = -1;

        public int GetRowPitch()
        {
            if (rowpitch == -1)
            {
                DeviceContext ctx = this.context.CurrentDeviceContext;
                DataBox db = ctx.MapSubresource(this.Resource, 0, 0, MapMode.WriteDiscard, SlimDX.Direct3D11.MapFlags.None);
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


        public void WriteDataStride(byte[] data)
        {
            DeviceContext ctx = this.context.CurrentDeviceContext;
            DataBox db = ctx.MapSubresource(this.Resource, 0, 0, MapMode.WriteDiscard, SlimDX.Direct3D11.MapFlags.None);

            try
            {
                int pos = 0;
                int idx = 0;
                for (int i = 0; i < desc.Height; i++)
                {
                    for (int j = 0; j < desc.Width; j++)
                    {
                        db.Data.Write(data[idx]);
                        idx++;
                    }
                    pos += db.RowPitch;
                    db.Data.Position = pos;
                }
            }
            catch { }
            ctx.UnmapSubresource(this.Resource, 0);  
        }

        public void WriteDataStride(IntPtr data, long size)
        {
            DeviceContext ctx = this.context.CurrentDeviceContext;
            DataBox db = ctx.MapSubresource(this.Resource, 0, 0, MapMode.WriteDiscard, SlimDX.Direct3D11.MapFlags.None);

            byte* b = (byte*)data.ToPointer();
            try
            {
                int pos = 0;
                int idx = 0;
                for (int i = 0; i < desc.Height; i++)
                {
                    for (int j = 0; j < desc.Width; j++)
                    {
                        db.Data.Write(b[idx]);
                        idx++;
                    }
                    pos += db.RowPitch;
                    db.Data.Position = pos;
                }
            }
            catch { }
            ctx.UnmapSubresource(this.Resource, 0);  
        }

        public void WriteDataStride(short[] data)
        {
            DeviceContext ctx = this.context.CurrentDeviceContext;
            DataBox db = ctx.MapSubresource(this.Resource, 0, 0, MapMode.WriteDiscard, SlimDX.Direct3D11.MapFlags.None);

            try
            {
                int pos = 0;
                int idx = 0;
                for (int i = 0; i < desc.Height; i++)
                {
                    for (int j = 0; j < desc.Width; j++)
                    {
                        db.Data.Write(data[idx]);
                        idx++;
                    }
                    pos += db.RowPitch;
                    db.Data.Position = pos;
                }
            }
            catch { }
            ctx.UnmapSubresource(this.Resource, 0);
        }

        public IntPtr MapForWrite()
        {
            DeviceContext ctx = this.context.CurrentDeviceContext;
            DataBox db = ctx.MapSubresource(this.Resource, 0, 0, MapMode.WriteDiscard, SlimDX.Direct3D11.MapFlags.None);
            return db.Data.DataPointer;
        }

        public void UnMap()
        {
            DeviceContext ctx = this.context.CurrentDeviceContext;
            ctx.UnmapSubresource(this.Resource, 0);
        }

        public void WriteData(byte[] data)
        {
            DeviceContext ctx = this.context.CurrentDeviceContext;
            DataBox db = ctx.MapSubresource(this.Resource, 0, 0, MapMode.WriteDiscard, SlimDX.Direct3D11.MapFlags.None);
            db.Data.WriteRange(data);
            ctx.UnmapSubresource(this.Resource, 0);
        }

        public void WriteData<T>(T[] data) where T : struct
        {
            DeviceContext ctx = this.context.CurrentDeviceContext;
            DataBox db = ctx.MapSubresource(this.Resource, 0, 0, MapMode.WriteDiscard, SlimDX.Direct3D11.MapFlags.None);
            db.Data.WriteRange<T>(data);
            ctx.UnmapSubresource(this.Resource, 0);
        }

        public void WriteData(IntPtr data, long size)
        {
            DeviceContext ctx = this.context.CurrentDeviceContext;
            DataBox db = ctx.MapSubresource(this.Resource, 0, 0, MapMode.WriteDiscard, SlimDX.Direct3D11.MapFlags.None);
            db.Data.WriteRange(data, size);
            ctx.UnmapSubresource(this.Resource, 0);
        }

        public override void Dispose()
        {
            this.SRV.Dispose();
            this.Resource.Dispose();
        }
    }
}
