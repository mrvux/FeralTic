using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

using SlimDX.Direct3D11;

using FeralTic.DX11.Resources;

namespace FeralTic.DX11
{
    public class RenderTargetStackElement
    {
        private IDX11RenderTargetView[] rendertargets;
        private RenderTargetView[] rtvs;

        private IDX11DepthStencil depth;

        private Viewport vp;

        private Rectangle? scissor;
        private bool rodsv = false;

        public IDX11RenderTargetView[] RenderTargets
        {
            get { return this.rendertargets; }
        }

        public IDX11DepthStencil DepthStencil
        {
            get { return this.depth; }
        }

        public bool ReadonlyDepth
        {
            get { return this.rodsv; }
        }

        public RenderTargetStackElement(Viewport vp, Rectangle? scissorRectangle, IDX11DepthStencil dsv, bool rodsv = false, params IDX11RenderTargetView[] rts)
        {
            this.depth = dsv;
            this.rendertargets = rts;
            this.rodsv = rodsv;

            this.vp = vp;
            this.scissor = scissorRectangle;

            rtvs = new RenderTargetView[rts.Length];
            for (int i = 0; i < rts.Length; i++) { rtvs[i] = rts[i].RTV; }
        }

        public RenderTargetStackElement(Viewport vp, IDX11DepthStencil dsv, bool rodsv = false, params IDX11RenderTargetView[] rts) : 
            this (vp, null, dsv, rodsv, rts)
        {
        }

        public RenderTargetStackElement(IDX11DepthStencil dsv, bool rodsv = false, params IDX11RenderTargetView[] rts)
        {
            this.depth = dsv;
            this.rendertargets = rts;
            this.rodsv = rodsv;

            this.vp.X = 0;
            this.vp.Y = 0;
            this.vp.MinZ = 0.0f;
            this.vp.MaxZ = 1.0f;

            this.scissor = null;

            if (this.depth != null)
            {
                this.vp.Width = this.depth.Width;
                this.vp.Height = this.depth.Height;
            }
            else
            {
                this.vp.Width = rts[0].Width;
                this.vp.Height = rts[0].Height;
            }

            rtvs = new RenderTargetView[rts.Length];
            for (int i = 0; i < rts.Length; i++) { rtvs[i] = rts[i].RTV; }
        }

        public void Apply(DeviceContext ctx)
        {
            if (depth != null)
            {
                if (rodsv)
                {
                    ctx.OutputMerger.SetTargets(depth.ReadOnlyDSV, this.rtvs);
                }
                else
                {
                    ctx.OutputMerger.SetTargets(depth.DSV, this.rtvs);
                }
            }
            else
            {
                ctx.OutputMerger.SetTargets(this.rtvs);
            }
            ctx.Rasterizer.SetViewports(vp);
            if (this.scissor.HasValue) { ctx.Rasterizer.SetScissorRectangles(scissor.Value); }
            else { ctx.Rasterizer.SetScissorRectangles(null); }
        }

        public RenderTargetStackElement WithViewport(Viewport viewport)
        {
            return new RenderTargetStackElement(viewport, this.scissor, this.depth, this.rodsv, this.rendertargets);
        }

        public RenderTargetStackElement WithViewportAndScissor(Viewport viewport,Rectangle scissor)
        {
            return new RenderTargetStackElement(viewport, scissor, this.depth, this.rodsv, this.rendertargets);
        }

    }
}
