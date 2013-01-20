using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FeralTic.DX11
{
    public class DX11RenderStateStack
    {
        private DX11RenderContext context;

        private DX11RenderState defaultstate;

        private Stack<DX11RenderState> stack;

        public int Count
        {
            get { return this.stack.Count; }
        }

        public DX11RenderStateStack(DX11RenderContext context)
        {
            this.context = context;
            this.defaultstate = new DX11RenderState();
            stack = new Stack<DX11RenderState>();
            this.defaultstate.Apply(context);
        }

        public void Push(DX11RenderState state)
        { 
            stack.Push(state);
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
            else
            {
                defaultstate.Apply(context);
            }
        }
    }
}
