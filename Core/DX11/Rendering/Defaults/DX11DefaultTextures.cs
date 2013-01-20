using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using FeralTic.DX11.Resources;

namespace FeralTic.DX11
{
    public class DefaultTextures : IDisposable
    {
        private DX11RenderContext context;

        private DX11DynamicTexture2D blacktex;
        private DX11DynamicTexture2D whitetex;

        public DefaultTextures(DX11RenderContext context)
        {
            this.context = context;
        }

        public DX11DynamicTexture2D BlackTexture
        {
            get
            {
                if (this.blacktex == null)
                {
                    blacktex = new DX11DynamicTexture2D(context, 1, 1, SlimDX.DXGI.Format.R8G8B8A8_UNorm);
                    blacktex.WriteData(new byte[] { 0, 0, 0, 255 });
                }
                return blacktex;
            }
        }

        public DX11DynamicTexture2D WhiteTexture
        {
            get
            {
                if (this.whitetex == null)
                {
                    whitetex = new DX11DynamicTexture2D(context, 1, 1, SlimDX.DXGI.Format.R8G8B8A8_UNorm);
                    whitetex.WriteData(new byte[] {255,255,255, 255 });
                }
                return whitetex;
            }
        }

        public void Dispose()
        {
            if (this.blacktex != null) { this.blacktex.Dispose(); this.blacktex = null; }
            if (this.whitetex != null) { this.whitetex.Dispose(); this.whitetex = null; }
        }

    }
}
