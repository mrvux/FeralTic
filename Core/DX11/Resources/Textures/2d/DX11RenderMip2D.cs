using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SlimDX.Direct3D11;
using SlimDX.DXGI;

namespace FeralTic.DX11.Resources
{
    public class DX11RenderMip2D : DX11Texture2D
    {
        public DX11MipSliceRenderTarget2D[] Slices { get; protected set; }

        private int CountMipLevels(int w,int h)
        {
            int level = 1;
            while (w > 1 && h > 1)
            {
                w /= 2; h /= 2; level++; 
            }
            return level;
        }

        public DX11RenderMip2D(DX11RenderContext context, int w, int h, Format format, bool allowUAV = false)
        {
            this.context = context;
            int levels = this.CountMipLevels(w,h);

            BindFlags flags = BindFlags.RenderTarget | BindFlags.ShaderResource;
            if (allowUAV)
            {
                flags |= BindFlags.UnorderedAccess;
            }

            var texBufferDesc = new Texture2DDescription
            {
                ArraySize = 1,
                BindFlags = flags,
                CpuAccessFlags = CpuAccessFlags.None,
                Format = format,
                Height = h,
                Width = w,
                OptionFlags = ResourceOptionFlags.None,
                SampleDescription = new SampleDescription(1, 0),
                Usage = ResourceUsage.Default,
                MipLevels = levels,
            };

            
            this.Resource = new Texture2D(context.Device, texBufferDesc);
            this.desc = this.Resource.Description;

            this.SRV = new ShaderResourceView(context.Device, this.Resource);

            this.Slices = new DX11MipSliceRenderTarget2D[levels];

            int sw = w;
            int sh = h;

            for (int i = 0; i < levels; i++)
            {
                this.Slices[i] = new DX11MipSliceRenderTarget2D(this.context, this, i, w, h);
                w /= 2; h /= 2;
            }
        }

        public override void Dispose()
        {
            if (this.SRV != null) { this.SRV.Dispose(); }
            if (this.Resource != null) { this.Resource.Dispose(); }
            foreach (DX11MipSliceRenderTarget2D slice in this.Slices)
            {
                slice.Dispose();
            }
        }
    }
}
