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
    public enum DX11BufferUploadType { Dynamic = 0, Default = 1, Immutable = 3 }

    public enum eDX11BufferMode { Default = 0, Append = 2, Counter = 4 }

    public interface IDX11StructuredBuffer : IDX11Buffer
    {
        int ElementCount { get; }
        int Stride { get; }
    }

    public interface IDX11ReadableStructureBuffer : IDX11ReadableResource, IDX11StructuredBuffer { }

    public interface IDX11RWStructureBuffer : IDX11RWResource, IDX11StructuredBuffer, IDX11ReadableStructureBuffer 
    {
        eDX11BufferMode BufferType { get; }
        void Clear(DeviceContext ctx, int value);
    }

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
