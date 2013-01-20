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

        public int ElemCnt { get { return desc.ArraySize; } }


        private Format original;

        public DX11DepthTextureArray(DX11RenderContext context, int w, int h, int elemcnt, Format format)
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
                Format = DepthFormatsHelper.GetDepthFormat(format),// Format.D32_Float,
                Dimension = DepthStencilViewDimension.Texture2DArray,
                MipSlice = 0
            };

            this.DSV = new DepthStencilView(context.Device, this.Resource, dsvd);

            dsvd.Flags = DepthStencilViewFlags.ReadOnlyDepth;
            if (format == Format.D24_UNorm_S8_UInt) { dsvd.Flags |= DepthStencilViewFlags.ReadOnlyStencil; }

            this.ReadOnlyDSV = new DepthStencilView(context.Device, this.Resource, dsvd);

            this.desc = texBufferDesc;
        }

        public DepthStencilView GetView(int slice, int count)
        {
            DepthStencilViewDescription dsvd = new DepthStencilViewDescription()
            {
                ArraySize = count,
                FirstArraySlice = slice,
                Format = DepthFormatsHelper.GetDepthFormat(this.original),// Format.D32_Float,
                Dimension = DepthStencilViewDimension.Texture2DArray,
                MipSlice = 0
            };

            return new DepthStencilView(context.Device, this.Resource, dsvd);
        }

        public override void Dispose()
        {
            this.DSV.Dispose();
            this.SRV.Dispose();
            this.Resource.Dispose();
        }
    }
}
