using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.IO;

using SlimDX.Direct3D11;
using SlimDX.DXGI;
using Device = SlimDX.Direct3D11.Device;

namespace FeralTic.DX11.Resources
{
    public class DX11Texture2D : DX11DeviceResource<Texture2D>, IDX11ReadableResource
    {
        public Texture2DDescription Description { get { return this.desc; } }
        protected bool isowner;
        protected Texture2DDescription desc;
        protected DX11RenderContext context;

        public static DX11Texture2D FromDescription(DX11RenderContext context, Texture2DDescription desc)
        {
            DX11Texture2D res = new DX11Texture2D();
            res.context = context;
            res.Resource = new Texture2D(context.Device, desc);
            res.isowner = true;
            res.desc = desc;
            res.SRV = new ShaderResourceView(context.Device, res.Resource);

            return res;
        }

        public static DX11Texture2D FromTextureAndSRV(DX11RenderContext context, Texture2D tex,ShaderResourceView srv)
        {
            Texture2DDescription desc = tex.Description;

            DX11Texture2D res = new DX11Texture2D();
            res.context = context;
            res.Resource = tex;
            res.SRV = srv;
            res.desc = desc;
            res.isowner = false;
            return res;
        }

        public static DX11Texture2D FromResource(DX11RenderContext context, Assembly assembly, string path)
        {
            try
            {
                Stream s = assembly.GetManifestResourceStream(path);
                Texture2D tex = Texture2D.FromStream(context.Device, s,(int)s.Length);

                if (tex.Description.ArraySize == 1)
                {
                    DX11Texture2D res = new DX11Texture2D();
                    res.context = context;
                    res.Resource = tex;
                    res.SRV = new ShaderResourceView(context.Device, res.Resource);
                    res.desc = res.Resource.Description;
                    res.isowner = true;
                    return res;
                }
                else
                {
                    if (tex.Description.OptionFlags.HasFlag(ResourceOptionFlags.TextureCube))
                    {
                        return new DX11TextureCube(context, tex);
                    }
                    else
                    {
                        return new DX11TextureArray2D(context, tex);
                    }
                }
            }
            catch
            {
                throw;
            }
        }

        public static DX11Texture2D FromMemory(DX11RenderContext context, byte[] data)
        {
            try
            {
                Texture2D tex = Texture2D.FromMemory(context.Device, data);

                if (tex.Description.ArraySize == 1)
                {
                    DX11Texture2D res = new DX11Texture2D();
                    res.context = context;
                    res.Resource = tex;
                    res.SRV = new ShaderResourceView(context.Device, res.Resource);
                    res.desc = res.Resource.Description;
                    res.isowner = true;
                    return res;
                }
                else
                {
                    if (tex.Description.OptionFlags.HasFlag(ResourceOptionFlags.TextureCube))
                    {
                        return new DX11TextureCube(context, tex);
                    }
                    else
                    {
                        return new DX11TextureArray2D(context, tex);
                    }
                }
            }
            catch
            {
                throw;
            }
        }

        public static DX11Texture2D FromFile(DX11RenderContext context, string path)
        {
            try
            {
                //ImageLoadInformation inf = new ImageLoadInformation();
                //inf.
                //res.Resource = Texture2D.FromFile(context.Device, path);

                Texture2D tex = Texture2D.FromFile(context.Device, path);

                if (tex.Description.ArraySize == 1)
                {
                    DX11Texture2D res = new DX11Texture2D();
                    res.context = context;
                    res.Resource = tex;
                    res.SRV = new ShaderResourceView(context.Device, res.Resource);
                    res.desc = res.Resource.Description;
                    res.isowner = true;
                    return res;
                }
                else
                {
                    if (tex.Description.OptionFlags.HasFlag(ResourceOptionFlags.TextureCube))
                    {
                        return new DX11TextureCube(context, tex);
                    }
                    else
                    {
                        return new DX11TextureArray2D(context, tex);
                    }
                }
            }
            catch
            {
                throw;
            }
        }

        public Format Format { get { return this.desc.Format; } }
        public int Width { get { return this.desc.Width; } }
        public int Height { get { return this.desc.Height; } }



        public override void Dispose()
        {
            if (isowner)
            {
                this.SRV.Dispose();
                this.Resource.Dispose();
            }
        }
    }
}
