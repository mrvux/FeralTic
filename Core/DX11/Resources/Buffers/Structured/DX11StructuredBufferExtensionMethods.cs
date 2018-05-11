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
    public static class DX11StructuredBufferExtensionMethods
    {
        public static DX11DynamicStructuredBuffer GetOrResize(this DX11DynamicStructuredBuffer buffer, Device device, int elementCount, int stride)
        {
            if (buffer != null)
            {
                if (buffer.ElementCount != elementCount || buffer.Stride != stride)
                {
                    buffer.Dispose();
                    buffer = null;
                }
            }

            if (buffer == null)
            {
                buffer = new DX11DynamicStructuredBuffer(device, elementCount, stride);
            }

            return buffer;
        }

        public static DX11RWStructuredBuffer GetOrResize(this DX11RWStructuredBuffer buffer, Device device, int elementCount, int stride)
        {
            if (buffer != null)
            {
                if (buffer.ElementCount != elementCount || buffer.Stride != stride)
                {
                    buffer.Dispose();
                    buffer = null;
                }
            }

            if (buffer == null)
            {
                buffer = new DX11RWStructuredBuffer(device, elementCount, stride);
            }
            return buffer;
        }

        public static DX11StagingStructuredBuffer GetOrResize(this DX11StagingStructuredBuffer buffer, Device device, int elementCount, int stride)
        {
            if (buffer != null)
            {
                if (buffer.ElementCount != elementCount || buffer.Stride != stride)
                {
                    buffer.Dispose();
                    buffer = null;
                }
            }

            if (buffer == null)
            {
                buffer = new DX11StagingStructuredBuffer(device, elementCount, stride);
            }
            return buffer;
        }
    }
}
