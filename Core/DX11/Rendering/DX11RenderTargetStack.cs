using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

using SlimDX.Direct3D11;

using FeralTic.DX11.Resources;

namespace FeralTic.DX11
{
    public class DX11RenderTargetStack
    {
        private DX11RenderContext context;

        private Stack<RenderTargetStackElement> stack;

        private DX11ViewportStack viewportstack;

        public int StackCount { get { return this.stack.Count; } }
        public int ViewPortStackCount { get { return this.viewportstack.Count; } }

        public DX11RenderTargetStack(DX11RenderContext context)
        {
            this.context = context;
            this.viewportstack = new DX11ViewportStack(context);

            stack = new Stack<RenderTargetStackElement>();
        }

        public void Push(Viewport vp, IDX11DepthStencil dsv, bool rodsv = false, params IDX11RenderTargetView[] rts)
        {
            RenderTargetStackElement elem = new RenderTargetStackElement(vp, dsv, rodsv, rts);
            stack.Push(elem);
            this.Apply();
        }

        public void Push(Viewport vp, Rectangle scissor, IDX11DepthStencil dsv, bool rodsv = false, params IDX11RenderTargetView[] rts)
        {
            RenderTargetStackElement elem = new RenderTargetStackElement(vp,scissor, dsv, rodsv, rts);
            stack.Push(elem);
            this.Apply();
        }

        public void Push(IDX11DepthStencil dsv, bool rodsv = false, params IDX11RenderTargetView[] rts)
        {
            RenderTargetStackElement elem = new RenderTargetStackElement(dsv, rodsv, rts);
            stack.Push(elem);
            this.Apply();
        }

        public void Push(params IDX11RenderTargetView[] rts)
        {
            RenderTargetStackElement elem = new RenderTargetStackElement(null, false, rts);
            stack.Push(elem);
            this.Apply();
        }

        public void PushViewport(Viewport vp)
        {
            //context.CurrentDeviceContext.Rasterizer.SetViewports(new Viewport(0, 0, 50, 50, 0, 1));
            this.viewportstack.Push(vp);
        }

        public void PushViewPort(Viewport vp, Rectangle scissor)
        {
            this.viewportstack.Push(vp, scissor);
        }

        public void PopViewport()
        {
            this.viewportstack.Pop();
        }

        
        public void Pop()
        {
            stack.Pop();
            this.Apply();
        }

        public void Apply()
        {
            if (stack.Count > 0)
            {
                stack.Peek().Apply(context.CurrentDeviceContext);
            }
            else
            {
                RenderTargetView[] zero = new RenderTargetView[] { null,null,null,null,null,null,null,null };
                context.CurrentDeviceContext.OutputMerger.SetTargets(null, zero);
            }
        }

        public void Apply(DeviceContext ctx)
        {
            if (stack.Count > 0)
            {
                stack.Peek().Apply(ctx);
            }
            else
            {
                RenderTargetView[] zero = new RenderTargetView[] { null, null, null, null, null, null, null, null };
                ctx.OutputMerger.SetTargets(null, zero);
            }
        }


    }
}
