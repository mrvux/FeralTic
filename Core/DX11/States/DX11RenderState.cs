using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SlimDX.Direct3D11;

using SlimDX;

namespace FeralTic.DX11
{
    public class DX11RenderState
    {
        public RasterizerStateDescription Rasterizer;

        public DepthStencilStateDescription DepthStencil;

        public BlendStateDescription Blend;


        public DX11RenderState()
        {
            this.SetDefaultRasterizer();
            this.SetDefaultDepthStencil();
            this.SetDefaultBlend();
        }

        public DX11RenderState Clone()
        {
            DX11RenderState result = new DX11RenderState();
            result.Blend = this.Blend;
            result.DepthStencil = this.DepthStencil;
            result.Rasterizer = this.Rasterizer;
            return result;
        }

        public void SetDefaultRasterizer()
        {
            this.Rasterizer = new RasterizerStateDescription()
            {
                CullMode = CullMode.Back,
                DepthBias = 0,
                DepthBiasClamp = 0.0f,
                FillMode = FillMode.Solid,
                IsAntialiasedLineEnabled = false,
                IsDepthClipEnabled = true,
                IsFrontCounterclockwise = false,
                IsMultisampleEnabled = false,
                IsScissorEnabled = false,
                SlopeScaledDepthBias = 0.0f
            };
        }

        public void SetDefaultBlend()
        {
            this.Blend = new BlendStateDescription()
            {
                AlphaToCoverageEnable = false,
                IndependentBlendEnable = false,
                //RenderTargets 
                //RenderTargets = this.SetDefaultRenderBlend()
            };
            this.Blend.RenderTargets[0] = this.SetDefaultRenderBlend();
            this.Blend.RenderTargets[1] = this.SetDefaultRenderBlend();
            this.Blend.RenderTargets[2] = this.SetDefaultRenderBlend();
            this.Blend.RenderTargets[3] = this.SetDefaultRenderBlend();
            this.Blend.RenderTargets[4] = this.SetDefaultRenderBlend();
            this.Blend.RenderTargets[5] = this.SetDefaultRenderBlend();
            this.Blend.RenderTargets[6] = this.SetDefaultRenderBlend();
            this.Blend.RenderTargets[7] = this.SetDefaultRenderBlend();

        }

        public RenderTargetBlendDescription SetDefaultRenderBlend()
        {
            return new RenderTargetBlendDescription()
            {
                BlendEnable = false,
                BlendOperation = BlendOperation.Add,
                BlendOperationAlpha = BlendOperation.Add,
                DestinationBlend = BlendOption.Zero,
                DestinationBlendAlpha = BlendOption.Zero,
                RenderTargetWriteMask = ColorWriteMaskFlags.All,
                SourceBlend = BlendOption.One,
                SourceBlendAlpha = BlendOption.One
            };
        }

        public void SetDefaultDepthStencil()
        {
            this.DepthStencil = new DepthStencilStateDescription()
            {
                DepthComparison = Comparison.LessEqual,
                DepthWriteMask = DepthWriteMask.All,
                IsDepthEnabled = true,
                IsStencilEnabled = false,
                StencilReadMask = 0xFF,
                StencilWriteMask = 0xFF
            };
        }

        public void Apply(DX11RenderContext context)
        {
            DeviceContext ctx = context.CurrentDeviceContext;

            RasterizerState rs = RasterizerState.FromDescription(context.Device, this.Rasterizer);
            DepthStencilState ds = DepthStencilState.FromDescription(context.Device, this.DepthStencil);
            BlendState bs = BlendState.FromDescription(context.Device, this.Blend);

            ctx.Rasterizer.State = rs;
            ctx.OutputMerger.DepthStencilState = ds;
            ctx.OutputMerger.DepthStencilReference = 0;
            ctx.OutputMerger.BlendState = bs;
            ctx.OutputMerger.BlendFactor = new Color4(0, 0, 0, 0);
            ctx.OutputMerger.BlendSampleMask = int.MaxValue;
        }
    }
}
