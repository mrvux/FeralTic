using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SlimDX.Direct3D11;
using SlimDX.DXGI;
using Device = SlimDX.Direct3D11.Device;

using FeralTic.DX11.Utils;



namespace FeralTic.DX11.Resources
{
    /// <summary>
    /// This is a write only depth stencil implementation, does not allow read views and cannot retrieve data
    /// </summary>
    public class DX11WriteOnlyDepthStencil : IDX11DepthStencil, IDisposable
    {
        private DX11RenderContext context;
        private Texture2DDescription textureDescription;
        private Texture2D texture;
        private DepthStencilView depthStencilView;

        public DepthStencilView DSV
        {
            get { return this.depthStencilView; }
        }

        public DepthStencilView ReadOnlyDSV
        {
            get { return this.depthStencilView; }
        }

        public int Width
        {
            get { return this.textureDescription.Width; }
        }


        public int Height
        {
            get { return this.textureDescription.Height; }
        }

        public DX11WriteOnlyDepthStencil(DX11RenderContext context, int w, int h, SampleDescription sd)
            : this(context, w, h, sd, Format.D32_Float)
        {
        }

        public DX11WriteOnlyDepthStencil(DX11RenderContext context, int w, int h, Format format)
            : this(context, w, h, new SampleDescription(1,0), format)
        {
        }

        public DX11WriteOnlyDepthStencil(DX11RenderContext context, int w, int h, SampleDescription sd, Format format)
        {
            this.context = context;
            var depthBufferDesc = new Texture2DDescription
            {
                ArraySize = 1,
                BindFlags = BindFlags.DepthStencil,
                CpuAccessFlags = CpuAccessFlags.None,
                Format = format,
                Height = h,
                Width = w,
                MipLevels = 1,
                OptionFlags = ResourceOptionFlags.None,
                SampleDescription = sd,
                Usage = ResourceUsage.Default
            };

            this.texture = new Texture2D(context.Device, depthBufferDesc);
            this.textureDescription = this.texture.Description;

            this.depthStencilView = new DepthStencilView(context.Device, this.texture);
        }

        public void Clear(bool cleardepth = true, bool clearstencil = true, float depth = 1.0f, byte stencil = 0)
        {
            if (cleardepth || clearstencil)
            {
                DepthStencilClearFlags flags = (DepthStencilClearFlags)0;
                if (cleardepth) { flags = DepthStencilClearFlags.Depth; }
                if (clearstencil) { flags |= DepthStencilClearFlags.Stencil; }

                this.context.CurrentDeviceContext.ClearDepthStencilView(this.DSV, flags, depth, stencil);
            }

            
        }

        public void Dispose()
        {
            if (this.texture != null) { this.texture.Dispose(); }
            if (this.depthStencilView != null) { this.DSV.Dispose(); }
        }
    }
}
