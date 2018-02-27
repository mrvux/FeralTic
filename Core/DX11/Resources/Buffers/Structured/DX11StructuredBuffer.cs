using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SlimDX.Direct3D11;
using Buffer = SlimDX.Direct3D11.Buffer;
using System.Runtime.InteropServices;
using SlimDX;

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

    public unsafe abstract class DX11StructuredBuffer : IDX11StructuredBuffer
    {
        public Buffer Buffer { get; protected set; }

        public int Size { get; protected set; }

        public int Stride { get; protected set; }

        public int ElementCount { get; protected set; }

        protected virtual void OnDispose() { }

        public void Copy(DeviceContext ctx, DX11StructuredBuffer destination)
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

        public void Dispose()
        {
            if (this.Buffer != null) { this.Buffer.Dispose(); }
            this.OnDispose();
        }
    }
}
