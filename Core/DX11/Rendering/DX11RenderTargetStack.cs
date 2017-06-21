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

        public int StackCount { get { return this.stack.Count; } }

        public RenderTargetStackElement Current
        {
            get { return this.stack.Peek(); }
        }

        public DX11RenderTargetStack(DX11RenderContext context)
        {
            this.context = context;
            stack = new Stack<RenderTargetStackElement>();
        }

        public void Push(RenderTargetStackElement stackElement)
        {
            stack.Push(stackElement);
            this.Apply();
        }

        public void Push(IDX11DepthStencil dsv, bool rodsv = false, params IDX11RenderTargetView[] rts)
        {
            RenderTargetStackElement elem = new RenderTargetStackElement(dsv, rodsv, rts);
            Push(elem);
        }

        public void Push(params IDX11RenderTargetView[] rts)
        {
            RenderTargetStackElement elem = new RenderTargetStackElement(null, false, rts);
            Push(elem);
        }

        public void Push(Viewport vp, IDX11DepthStencil dsv, bool rodsv = false, params IDX11RenderTargetView[] rts)
        {
            RenderTargetStackElement elem = new RenderTargetStackElement(vp, dsv, rodsv, rts);
            Push(elem);
        }

        public void PushViewport(Viewport viewport)
        {
            Push(this.stack.Peek().WithViewport(viewport));
        }

        public void PushViewPort(Viewport viewport, Rectangle scissor)
        {
            Push(this.stack.Peek().WithViewportAndScissor(viewport, scissor));
        }

        public void Pop()
        {
            stack.Pop();
            this.Apply();
        }

        public void Apply()
        {
            Apply(context.CurrentDeviceContext);
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
