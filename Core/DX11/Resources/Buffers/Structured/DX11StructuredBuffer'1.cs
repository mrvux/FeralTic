using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

using SlimDX;
using SlimDX.Direct3D11;

using Buffer = SlimDX.Direct3D11.Buffer;


namespace FeralTic.DX11.Resources
{
    public unsafe abstract class DX11StructuredBuffer<T> : IDX11StructuredBuffer where T : struct
    {
        public Buffer Buffer { get; protected set; }

        public int Size { get; protected set; }

        public int ElementCount { get; protected set; }

        public int Stride { get; protected set; }

        protected abstract void OnDispose();

        public void Copy(DeviceContext ctx, DX11StructuredBuffer<T> destination)
        {
            if (this.Size == destination.Size)
            {
                ctx.CopyResource(this.Buffer, destination.Buffer);
            }
            else
            {
                throw new Exception("Invalid Matching sizes");
            }
        }

        public virtual void Dispose()
        {
            this.OnDispose();
            if (this.Buffer != null) { this.Buffer.Dispose(); }
        }
    }
}
