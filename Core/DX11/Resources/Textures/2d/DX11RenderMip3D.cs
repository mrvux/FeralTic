using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SlimDX.Direct3D11;
using SlimDX.DXGI;

namespace FeralTic.DX11.Resources
{
    public class DX11RenderMip3D : DX11Texture3D
    {
        public DX11MipSliceRenderTarget[] Slices { get; protected set; }

        private int CountMipLevels(int w,int h,int d)
        {
            int level = 1;
            while (w > 1 && h > 1 && d > 1)
            {
                w /= 2; h /= 2; d /= 2; level++;
            }
            return level;
        }

        public DX11RenderMip3D(DX11RenderContext context, int w, int h,int d, Format format) : base(context)
        {
            int levels = this.CountMipLevels(w,h,d);
            var texBufferDesc = new Texture3DDescription
            {
                BindFlags = BindFlags.RenderTarget | BindFlags.ShaderResource,
                CpuAccessFlags = CpuAccessFlags.None,
                Format = format,
                Height = h,
                Width = w,
                Depth = d,
                OptionFlags = ResourceOptionFlags.None,
                Usage = ResourceUsage.Default,
                MipLevels = levels,
            };

            this.Resource = new Texture3D(context.Device, texBufferDesc);
            this.Width = w;
            this.Height = h;
            this.Depth = d;

            this.SRV = new ShaderResourceView(context.Device, this.Resource);

            this.Slices = new DX11MipSliceRenderTarget[levels];

            int sw = w;
            int sh = h;
            int sd = d;

            for (int i = 0; i < levels; i++)
            {
                this.Slices[i] = new DX11MipSliceRenderTarget(this.context, this, i, w, h,d);
                w /= 2; h /= 2; d /= 2;
            }
        }

        public override void Dispose()
        {
            if (this.SRV != null) { this.SRV.Dispose(); }
            if (this.Resource != null) { this.Resource.Dispose(); }
            foreach (DX11MipSliceRenderTarget slice in this.Slices)
            {
                slice.Dispose();
            }
        }
    }
}
