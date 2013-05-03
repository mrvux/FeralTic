using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SlimDX;
using SlimDX.Direct3D11;
using Buffer = SlimDX.Direct3D11.Buffer;

namespace FeralTic.DX11.Utils
{
    public static class BufferHelper
    {
        public static Buffer CreateStreamOutBuffer(DX11RenderContext context, int vertexsize, int maxelements, bool allowvbo = true, bool allowibo = false)
        {
            BindFlags flags = BindFlags.StreamOutput;

            //Flag as raw if possible
            //flags |= context.ComputeShaderSupport ? BindFlags.UnorderedAccess : BindFlags.None;
            flags |= context.ComputeShaderSupport ? BindFlags.ShaderResource : BindFlags.None;

            flags |= allowvbo ? BindFlags.VertexBuffer : BindFlags.None;
            flags |= allowibo ? BindFlags.IndexBuffer : BindFlags.None;


            //Allow access as raw if possible
            Buffer buffer = new SlimDX.Direct3D11.Buffer(context.Device, new BufferDescription()
            {
                BindFlags = flags,
                CpuAccessFlags = CpuAccessFlags.None,
                OptionFlags = context.ComputeShaderSupport ? ResourceOptionFlags.RawBuffer : ResourceOptionFlags.None,
                SizeInBytes = vertexsize * maxelements,
                Usage = ResourceUsage.Default
            });
            return buffer;
        }

        public static Buffer CreateVertexBuffer(DX11RenderContext context, DataStream ds, bool alloraw = false, bool dispose = false)
        { 
            ds.Position = 0;

            var vertices = new SlimDX.Direct3D11.Buffer(context.Device, ds, new BufferDescription()
            {
                BindFlags = BindFlags.VertexBuffer | BindFlags.ShaderResource,
                CpuAccessFlags = CpuAccessFlags.None,
                OptionFlags = ResourceOptionFlags.None,
                SizeInBytes = (int)ds.Length,
                Usage = ResourceUsage.Default
            });

            if (dispose)
            {
                ds.Dispose();
            }

            return vertices;
        }

        public static Buffer CreateDynamicVertexBuffer(DX11RenderContext context, DataStream ds, bool dispose = false)
        {
            ds.Position = 0;
            var vertices = new SlimDX.Direct3D11.Buffer(context.Device, ds, new BufferDescription()
            {
                BindFlags = BindFlags.VertexBuffer,
                CpuAccessFlags = CpuAccessFlags.Write,
                OptionFlags = ResourceOptionFlags.None,
                SizeInBytes = (int)ds.Length,
                Usage = ResourceUsage.Dynamic
            });

            if (dispose)
            {
                ds.Dispose();
            }

            return vertices;
        }
    }
}
