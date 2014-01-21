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
    public class DX11CubeDepthStencil : DX11Texture2D, IDX11DepthStencil
    {
        public DX11SliceDepthStencil[] SliceDSV { get; protected set; }

        public DepthStencilView DSV { get; protected set; }

        public DepthStencilView ReadOnlyDSV { get; protected set; }

        public DX11CubeDepthStencil(DX11RenderContext context, int size, SampleDescription sd, Format format)
        {
            this.context = context;

            var texBufferDesc = new Texture2DDescription
            {
                ArraySize = 6,
                BindFlags = BindFlags.DepthStencil | BindFlags.ShaderResource,
                CpuAccessFlags = CpuAccessFlags.None,
                Format = DepthFormatsHelper.GetGenericTextureFormat(format),
                Height = size,
                Width = size,
                OptionFlags = ResourceOptionFlags.TextureCube,
                SampleDescription = sd,
                Usage = ResourceUsage.Default,
                MipLevels = 1
            };

            this.Resource = new Texture2D(context.Device, texBufferDesc);

            this.desc = texBufferDesc;

            //Create faces SRV/RTV
            this.SliceDSV = new DX11SliceDepthStencil[6];

            ShaderResourceViewDescription svd = new ShaderResourceViewDescription()
            {
                Dimension = ShaderResourceViewDimension.TextureCube,
                Format = DepthFormatsHelper.GetSRVFormat(format),
                MipLevels = 1,
                MostDetailedMip = 0,
                First2DArrayFace = 0
            };

            DepthStencilViewDescription dsvd = new DepthStencilViewDescription()
            {
                ArraySize= 6,
                Dimension = DepthStencilViewDimension.Texture2DArray,
                FirstArraySlice = 0,
                Format = DepthFormatsHelper.GetDepthFormat(format),
                MipSlice = 0
            };

            this.DSV = new DepthStencilView(context.Device, this.Resource, dsvd);

            if (context.IsFeatureLevel11)
            {
                dsvd.Flags = DepthStencilViewFlags.ReadOnlyDepth;
                if (format == Format.D24_UNorm_S8_UInt) { dsvd.Flags |= DepthStencilViewFlags.ReadOnlyStencil; }

                this.ReadOnlyDSV = new DepthStencilView(context.Device, this.Resource, dsvd);
            }

            this.SRV = new ShaderResourceView(context.Device, this.Resource, svd);

            for (int i = 0; i < 6; i++)
            {
                this.SliceDSV[i] = new DX11SliceDepthStencil(context, this, i, DepthFormatsHelper.GetDepthFormat(format));
            }
        }

        public override void Dispose()
        {
            for (int i = 0; i < 6; i++)
            {
                this.SliceDSV[i].Dispose();
            }
            base.Dispose();
        }
    }
}
