using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SlimDX.Direct3D11;
using SlimDX.DXGI;
using Device = SlimDX.Direct3D11.Device;


namespace FeralTic.DX11.Resources
{
    public class DX11RenderTextureArray : DX11Texture2D, IDX11RenderTargetView
    {
        public RenderTargetView RTV { get; protected set; }

        public int ElemCnt { get { return desc.ArraySize; } }

        public DX11SliceRenderTarget[] SliceRTV { get; protected set; }

        public DX11RenderTextureArray(DX11RenderContext context, int w, int h, int elemcnt, Format format, bool buildslices = true)
        {
            this.context = context;

            var texBufferDesc = new Texture2DDescription
            {
                ArraySize = elemcnt,
                BindFlags = BindFlags.RenderTarget | BindFlags.ShaderResource,
                CpuAccessFlags = CpuAccessFlags.None,
                Format = format,
                Height = h,
                Width = w,
                OptionFlags = ResourceOptionFlags.None,
                SampleDescription = new SampleDescription(1,0),
                Usage = ResourceUsage.Default,
            };

            this.Resource = new Texture2D(context.Device, texBufferDesc);

            RenderTargetViewDescription rtd = new RenderTargetViewDescription()
            {
                ArraySize = elemcnt,
                FirstArraySlice = 0,
                Dimension = RenderTargetViewDimension.Texture2DArray,
                Format = format
            };

            ShaderResourceViewDescription srvd = new ShaderResourceViewDescription()
            {
                ArraySize = elemcnt,
                FirstArraySlice = 0,
                Dimension = ShaderResourceViewDimension.Texture2DArray,
                Format = format,
                MipLevels = 1,
                MostDetailedMip = 0
            };

            this.SRV = new ShaderResourceView(context.Device, this.Resource, srvd);
            this.RTV = new RenderTargetView(context.Device, this.Resource, rtd);

            this.desc = texBufferDesc;

            this.SliceRTV = new DX11SliceRenderTarget[this.ElemCnt];

            if (buildslices)
            {
                for (int i = 0; i < this.ElemCnt; i++)
                {
                    this.SliceRTV[i] = new DX11SliceRenderTarget(this.context, this, i);
                }
            }
        }

        public ShaderResourceView GetSRVSlice(int slice, int count)
        {
            ShaderResourceViewDescription rtd = new ShaderResourceViewDescription()
            {
                ArraySize = count,
                FirstArraySlice = slice,
                Dimension = ShaderResourceViewDimension.Texture2DArray,
                Format = this.Format
            };

            return new ShaderResourceView(context.Device, this.Resource, rtd);
        }

        public RenderTargetView GetRTVSlice(int slice,int count)
        {
            RenderTargetViewDescription rtd = new RenderTargetViewDescription()
            {
                ArraySize = count,
                FirstArraySlice = slice,
                Dimension = RenderTargetViewDimension.Texture2DArray,
                Format = this.Format
            };

            return new RenderTargetView(context.Device, this.Resource, rtd);
        }

        public override void Dispose()
        {
            foreach (DX11SliceRenderTarget slice in this.SliceRTV)
            {
                 if (slice != null) { slice.Dispose(); }
            }

            this.RTV.Dispose();
            this.SRV.Dispose();
            this.Resource.Dispose();
        }
    }
}
