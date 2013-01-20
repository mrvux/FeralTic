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
        public static Buffer CreateStreamOutBuffer(Device device, int vertexsize, int maxelements)
        {
            //Create a stream out buffer (no init data, but also allow to bind as SO
            Buffer buffer = new SlimDX.Direct3D11.Buffer(device, new BufferDescription()
            {
                BindFlags = BindFlags.VertexBuffer | BindFlags.StreamOutput | BindFlags.ShaderResource,
                CpuAccessFlags = CpuAccessFlags.None,
                OptionFlags = ResourceOptionFlags.RawBuffer,
                SizeInBytes = vertexsize * maxelements,
                Usage = ResourceUsage.Default
            });
            return buffer;
        }

        public static Buffer CreateVertexBuffer(Device device, DataStream ds,bool alloraw =false, bool dispose = false)
        { 
            ds.Position = 0;

            var vertices = new SlimDX.Direct3D11.Buffer(device, ds, new BufferDescription()
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

        public static Buffer CreateDynamicVertexBuffer(Device device, DataStream ds, bool dispose = false)
        {
            ds.Position = 0;
            var vertices = new SlimDX.Direct3D11.Buffer(device, ds, new BufferDescription()
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
