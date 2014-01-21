using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SlimDX.Direct3D11;
using SlimDX.DXGI;

using Device = SlimDX.Direct3D11.Device;

namespace FeralTic.DX11.Resources
{
    public class DX11CubeRenderTarget : DX11Texture2D, IDX11RenderTargetView
    {
        public DX11SliceRenderTarget[] SliceRTV { get; protected set; }

        public RenderTargetView RTV { get; protected set; }

        public DX11CubeRenderTarget(DX11RenderContext context, int size, SampleDescription sd, Format format, bool genMipMaps, int mmLevels)
        {
            this.context = context;

            var texBufferDesc = new Texture2DDescription
            {
                ArraySize = 6,
                BindFlags = BindFlags.RenderTarget | BindFlags.ShaderResource,
                CpuAccessFlags = CpuAccessFlags.None,
                Format = format,
                Height = size,
                Width = size,
                OptionFlags = ResourceOptionFlags.TextureCube,
                SampleDescription = sd,
                Usage = ResourceUsage.Default,
            };

            if (genMipMaps && sd.Count == 1)
            {
                texBufferDesc.OptionFlags |= ResourceOptionFlags.GenerateMipMaps;
                texBufferDesc.MipLevels = mmLevels;
            }
            else
            {
                //Make sure we enforce 1 here, as we dont generate
                texBufferDesc.MipLevels = 1;
            }

            this.Resource = new Texture2D(context.Device, texBufferDesc);

            this.desc = texBufferDesc;

            //Create faces SRV/RTV
            this.SliceRTV = new DX11SliceRenderTarget[6];

            ShaderResourceViewDescription svd = new ShaderResourceViewDescription()
            {
                Dimension = ShaderResourceViewDimension.TextureCube,
                Format = format,
                MipLevels = 1,
                MostDetailedMip = 0,
                First2DArrayFace = 0
            };

            RenderTargetViewDescription rtvd = new RenderTargetViewDescription()
            {
                ArraySize= 6,
                Dimension = RenderTargetViewDimension.Texture2DArray,
                FirstArraySlice = 0,
                Format = format,
                MipSlice = 0
            };

            this.RTV = new RenderTargetView(context.Device, this.Resource, rtvd);

            this.SRV = new ShaderResourceView(context.Device, this.Resource, svd);

            for (int i = 0; i < 6; i++)
            {
                this.SliceRTV[i] = new DX11SliceRenderTarget(context, this, i);
            }
        }

        public override void Dispose()
        {
            for (int i = 0; i < 6; i++)
            {
                this.SliceRTV[i].Dispose();
            }
            base.Dispose();
        }
    }
}
