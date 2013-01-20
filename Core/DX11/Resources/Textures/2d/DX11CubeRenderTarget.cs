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
        public RenderTargetView[] FaceRTVs { get; protected set; }

        public ShaderResourceView[] FaceSRVs { get; protected set; }

        public RenderTargetView RTV { get; protected set; }

        //Sets face index for rendering
        public int FaceIndex
        {
            set
            {
                this.RTV = this.FaceRTVs[value % 6];
            }
        }

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
            this.FaceRTVs = new RenderTargetView[6];
            this.FaceSRVs = new ShaderResourceView[6];

            ShaderResourceViewDescription svd = new ShaderResourceViewDescription()
            {
                Dimension = ShaderResourceViewDimension.TextureCube,
                Format = format,
                MipLevels = 1,
                MostDetailedMip = 0,
                First2DArrayFace = 0
            };

            this.SRV = new ShaderResourceView(context.Device, this.Resource, svd);

            this.CreateSliceViews(context.Device, format);
        }

        private void CreateSliceViews(Device device, Format format)
        {
            RenderTargetViewDescription rtd = new RenderTargetViewDescription()
            {
                ArraySize = 1,
                Dimension = RenderTargetViewDimension.Texture2DArray,
                Format = format
            };

            ShaderResourceViewDescription srvd = new ShaderResourceViewDescription()
            {
                ArraySize = 1,
                Dimension = ShaderResourceViewDimension.Texture2DArray,
                Format = format,
                MipLevels = 1,
                MostDetailedMip = 0
            };

            for (int i = 0; i < 6; i++)
            {
                rtd.FirstArraySlice = i;
                srvd.FirstArraySlice = i;

                this.FaceRTVs[i] = new RenderTargetView(device, this.Resource, rtd);
                this.FaceSRVs[i] = new ShaderResourceView(device, this.Resource, srvd);
            }
        }

        public override void Dispose()
        {
            for (int i = 0; i < 6; i++)
            {
                this.FaceRTVs[i].Dispose();
                this.FaceSRVs[i].Dispose();
            }
            base.Dispose();
        }
    }
}
