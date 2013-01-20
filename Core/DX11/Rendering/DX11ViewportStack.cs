using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SlimDX.Direct3D11;
using System.Drawing;

namespace FeralTic.DX11
{
    public class DX11ViewPortData
    {
        public DX11ViewPortData(Viewport vp)
        {
            this.Viewport = vp;
            this.HasScissor = false;
        }

        public DX11ViewPortData(Viewport vp, Rectangle scissor)
        {
            this.Viewport = vp;
            this.HasScissor = true;
            this.Scissor = scissor;
        }

        public Viewport Viewport { get; protected set; }
        public Rectangle Scissor { get; protected set; }
        public bool HasScissor { get; protected set;}

        public void Apply(DX11RenderContext context)
        {
            context.CurrentDeviceContext.Rasterizer.SetViewports(this.Viewport);

            if (this.HasScissor)
            {
                context.CurrentDeviceContext.Rasterizer.SetScissorRectangles(this.Scissor);
            }
            else
            {
                context.CurrentDeviceContext.Rasterizer.SetScissorRectangles(null);
            }
        }

    }

    public class DX11ViewportStack
    {
        private DX11RenderContext context;

        private Stack<DX11ViewPortData> stack;

        public int Count
        {
            get { return stack.Count; }
        }

        public DX11ViewportStack(DX11RenderContext context)
        {
            this.context = context;
            stack = new Stack<DX11ViewPortData>();
        }

        public void Push(Viewport vp)
        {
            stack.Push(new DX11ViewPortData(vp));
            this.Apply();
        }

        public void Push(Viewport vp,Rectangle scissor)
        {
            stack.Push(new DX11ViewPortData(vp,scissor));
            this.Apply();
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
                stack.Peek().Apply(context);
            }
        }
    }
}
