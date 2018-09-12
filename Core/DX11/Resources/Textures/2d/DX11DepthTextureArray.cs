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
    public class DX11DepthTextureArray : DX11Texture2D, IDX11DepthStencil
    {
        public DepthStencilView DSV { get; protected set; }

        public DepthStencilView ReadOnlyDSV { get; protected set; }

        public DX11SliceDepthStencil[] SliceDSV { get; protected set; }

        public DX11Texture2D Stencil { get; protected set; }
        private ShaderResourceView stencilview;

        public int ElemCnt { get { return desc.ArraySize; } }

        private Format original;

        public DX11DepthTextureArray(DX11RenderContext context, int w, int h, int elemcnt, Format format, bool buildslices = true)
        {
            this.context = context;

            this.original = format;

            var texBufferDesc = new Texture2DDescription
            {
                ArraySize = elemcnt,
                BindFlags = BindFlags.DepthStencil | BindFlags.ShaderResource,
                CpuAccessFlags = CpuAccessFlags.None,
                Format = DepthFormatsHelper.GetGenericTextureFormat(format),
                Height = h,
                Width = w,
                OptionFlags = ResourceOptionFlags.None,
                SampleDescription = new SampleDescription(1,0),
                Usage = ResourceUsage.Default,
            };

            this.Resource = new Texture2D(context.Device, texBufferDesc);

            ShaderResourceViewDescription srvd = new ShaderResourceViewDescription()
            {
                ArraySize = elemcnt,
                FirstArraySlice = 0,
                Dimension = ShaderResourceViewDimension.Texture2DArray,
                Format = DepthFormatsHelper.GetSRVFormat(format),
                MipLevels = 1,
                MostDetailedMip = 0
            };

            this.SRV = new ShaderResourceView(context.Device, this.Resource, srvd);

            DepthStencilViewDescription dsvd = new DepthStencilViewDescription()
            {
                ArraySize = elemcnt,
                FirstArraySlice = 0,
                Format = DepthFormatsHelper.GetDepthFormat(format),
                Dimension = DepthStencilViewDimension.Texture2DArray,
                MipSlice = 0
            };

            this.DSV = new DepthStencilView(context.Device, this.Resource, dsvd);

            if (format == Format.D24_UNorm_S8_UInt || format == Format.D32_Float_S8X24_UInt)
            {
                ShaderResourceViewDescription stencild = new ShaderResourceViewDescription()
                {
                    ArraySize = elemcnt,
                    Format = format == Format.D24_UNorm_S8_UInt ? SlimDX.DXGI.Format.X24_Typeless_G8_UInt : Format.X32_Typeless_G8X24_UInt,
                    Dimension = ShaderResourceViewDimension.Texture2DArray,
                    MipLevels = 1,
                    MostDetailedMip = 0
                };

                this.stencilview = new ShaderResourceView(this.context.Device, this.Resource, stencild);

                this.Stencil = DX11Texture2D.FromTextureAndSRV(this.context, this.Resource, this.stencilview);

            }
            else
            {
                //Just pass depth instead
                this.Stencil = DX11Texture2D.FromTextureAndSRV(this.context, this.Resource, this.SRV);
            }

            dsvd.Flags = DepthStencilViewFlags.ReadOnlyDepth;
            if (format == Format.D24_UNorm_S8_UInt) { dsvd.Flags |= DepthStencilViewFlags.ReadOnlyStencil; }

            this.ReadOnlyDSV = new DepthStencilView(context.Device, this.Resource, dsvd);

            this.desc = texBufferDesc;

            this.SliceDSV = new DX11SliceDepthStencil[this.ElemCnt];

            if (buildslices)
            {
                for (int i = 0; i < this.ElemCnt; i++)
                {
                    this.SliceDSV[i] = new DX11SliceDepthStencil(this.context, this, i, DepthFormatsHelper.GetDepthFormat(format));
                }
            }
        }

        public override void Dispose()
        {
            if (this.DSV != null) { this.DSV.Dispose(); }
            if (this.SRV != null) { this.SRV.Dispose(); }
            if (this.Resource != null) { this.Resource.Dispose(); }
            if (this.ReadOnlyDSV != null) { this.ReadOnlyDSV.Dispose(); }
        }
    }
}
