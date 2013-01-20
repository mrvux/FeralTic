using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SlimDX.Direct3D11;

namespace FeralTic.DX11.Resources
{
    public class DX11Texture1D : DX11DeviceResource<Texture1D>
    {
        protected DX11RenderContext context;

        internal DX11Texture1D(DX11RenderContext context)
        {
            this.context = context;
        }


        public static DX11Texture1D FromFile(DX11RenderContext context, string path)
        {
            DX11Texture1D res = new DX11Texture1D(context);

            try
            {
                res.Resource = Texture1D.FromFile(context.Device, path);

                res.SRV = new ShaderResourceView(context.Device, res.Resource);
            }
            catch
            {
            }
            return res;
        }

        public void SaveToFile(string path)
        {
            Texture1D.SaveTextureToFile(context.CurrentDeviceContext, this.Resource, ImageFileFormat.Dds, path);
        }

        public override void Dispose()
        {

        }
    }
}
