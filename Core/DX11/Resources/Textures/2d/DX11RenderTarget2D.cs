using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SlimDX.Direct3D11;
using SlimDX.DXGI;
using Device = SlimDX.Direct3D11.Device;
using SlimDX;

namespace FeralTic.DX11.Resources
{
    public class DX11RenderTarget2D : DX11Texture2D, IDX11RWResource, IDX11RenderTargetView
    {
        private UnorderedAccessView uav;
        private bool allowuav;
        private bool genmm;
        private bool shared;
        private int requestedMipsLevel;

        public bool GenMipMaps
        {
            get { return this.genmm; }
        }

        public int RequestedMipLevels
        {
            get { return this.requestedMipsLevel; }
        }

        public bool Shared
        {
            get { return this.shared; }
        }

        public UnorderedAccessView UAV
        {
            get
            {
                if (this.uav == null && this.allowuav)
                {
                    this.uav = new UnorderedAccessView(this.Resource.Device, this.Resource);
                }
                return this.uav;
            }
        }

        public RenderTargetView RTV { get; protected set; }

        public DX11RenderTarget2D(DX11RenderContext context, Texture2D tex)
        {
            this.context = context;
            this.Resource = tex;

            this.SRV = new ShaderResourceView(context.Device, tex);

            this.RTV = new RenderTargetView(context.Device, tex);
        }


        public DX11RenderTarget2D(DX11RenderContext context, int w, int h, SampleDescription sd, Format format, bool genMipMaps, int mmLevels) :
            this(context,w,h,sd,format,genMipMaps,mmLevels,true,false) {}


        public DX11RenderTarget2D(DX11RenderContext context, int w, int h, SampleDescription sd, Format format, bool genMipMaps, int mmLevels, bool allowUAV, bool allowShare)
        {
            this.context = context;
            this.shared = allowShare;
            var texBufferDesc = new Texture2DDescription
            {
                ArraySize = 1,
                BindFlags = BindFlags.RenderTarget | BindFlags.ShaderResource,
                CpuAccessFlags = CpuAccessFlags.None,
                Format = format,
                Height = h,
                Width = w,
                OptionFlags = ResourceOptionFlags.None,
                SampleDescription = sd,
                Usage = ResourceUsage.Default,
            };

            this.requestedMipsLevel = mmLevels;

            if (sd.Count == 1 && allowUAV && context.IsFeatureLevel11)
            { 
                texBufferDesc.BindFlags |= BindFlags.UnorderedAccess;
                this.allowuav = true;
            }

            if (sd.Count == 1 && genMipMaps == false && allowShare)
            {
                texBufferDesc.OptionFlags = /*ResourceOptionFlags.KeyedMutex |*/ ResourceOptionFlags.Shared;
            }

            if (genMipMaps && sd.Count == 1)
            {
                texBufferDesc.OptionFlags |= ResourceOptionFlags.GenerateMipMaps;
                texBufferDesc.MipLevels = mmLevels;
                this.genmm = true;
            }
            else
            {
                //Make sure we enforce 1 here, as we dont generate
                texBufferDesc.MipLevels = 1;
                this.genmm = false;
            }

            this.Resource = new Texture2D(context.Device, texBufferDesc);

            this.RTV = new RenderTargetView(context.Device, this.Resource);
            this.SRV = new ShaderResourceView(context.Device, this.Resource);
            this.desc = texBufferDesc;
        }

        public DX11RenderTarget2D(DX11RenderContext context, int w, int h, SampleDescription sd, Format format) :
            this(context,w,h,sd,format,false,1)
        {

        }

        public void Clear(Color4 color)
        {
            this.context.CurrentDeviceContext.ClearRenderTargetView(this.RTV, color);
        }

        public override void Dispose()
        {      
            if (this.RTV != null) { this.RTV.Dispose(); }
            if (this.uav != null) { this.uav.Dispose(); }
            if (this.SRV != null) { this.SRV.Dispose(); }

            if (this.Resource != null) { this.Resource.Dispose(); }
        }
    }
}
