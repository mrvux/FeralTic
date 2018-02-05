using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SlimDX.Direct3D11;
using SlimDX.DXGI;

namespace FeralTic.DX11.Resources
{
    public class DX11Texture3D : DX11DeviceResource<Texture3D>
    {
        protected DX11RenderContext context;

        public DX11Texture3D(DX11RenderContext context)
        {
            this.context = context;
        }

        public Format Format
        {
            get;
            protected set;
        }

        public int Width { get; protected set; }
        public int Height { get; protected set; }
        public int Depth { get; protected set; }

       // protected virtual void OnDispose() { }


        public override void Dispose() 
        {
            //his.OnDispose();
        }

        public static DX11Texture3D FromFile(DX11RenderContext context, string path)
        {
            DX11Texture3D res = new DX11Texture3D(context);
            try
            {
                res.Resource = Texture3D.FromFile(context.Device, path);

                res.SRV = new ShaderResourceView(context.Device, res.Resource);

                Texture3DDescription desc = res.Resource.Description;

                res.Width = desc.Width;
                res.Height = desc.Height;
                res.Format = desc.Format;
                res.Depth = desc.Depth;
            }
            catch
            {

            }
            return res;
        }

        public static DX11Texture3D FromFile(DX11RenderContext context, string path,ImageLoadInformation loadinfo)
        {
            DX11Texture3D res = new DX11Texture3D(context);
            try
            {
                res.Resource = Texture3D.FromFile(context.Device, path,loadinfo);

                res.SRV = new ShaderResourceView(context.Device, res.Resource);

                Texture3DDescription desc = res.Resource.Description;

                res.Width = desc.Width;
                res.Height = desc.Height;
                res.Format = desc.Format;
                res.Depth = desc.Depth;
            }
            catch
            {

            }
            return res;
        }


        public static DX11Texture3D FromDescription(DX11RenderContext context, Texture3DDescription desc)
        {
            DX11OwnedTexture3D res = new DX11OwnedTexture3D(context);
            res.context = context;
            res.Resource = new Texture3D(context.Device, desc);
            res.SRV = new ShaderResourceView(context.Device, res.Resource);
            return res;
        }

        public static DX11Texture3D FromResource(DX11RenderContext context, Texture3D tex, ShaderResourceView srv)
        {
            DX11Texture3D res = new DX11Texture3D(context);
            res.Resource = tex;
            res.SRV = srv;


            Texture3DDescription desc = res.Resource.Description;

            res.Width = desc.Width;
            res.Height = desc.Height;
            res.Format = desc.Format;
            res.Depth = desc.Depth;

            return res;
        }
    }
}
