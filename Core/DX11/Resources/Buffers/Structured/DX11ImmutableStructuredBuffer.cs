﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using SlimDX;
using SlimDX.Direct3D11;

using Buffer = SlimDX.Direct3D11.Buffer;

namespace FeralTic.DX11.Resources
{
    public class DX11ImmutableStructuredBuffer : DX11StructuredBuffer, IDX11ReadableStructureBuffer
    {
        public ShaderResourceView SRV { get; protected set; }

        public DX11ImmutableStructuredBuffer(Device dev, int cnt, int stride, DataStream initial)
        {
            this.Stride = stride;
            this.Size = cnt * stride;
            this.ElementCount = cnt;

            BufferDescription bd = new BufferDescription()
            {
                BindFlags = BindFlags.ShaderResource,
                CpuAccessFlags = CpuAccessFlags.None,
                OptionFlags = ResourceOptionFlags.StructuredBuffer,
                SizeInBytes = this.Size,
                StructureByteStride = this.Stride,
                Usage = ResourceUsage.Immutable
            };

            this.Buffer = new Buffer(dev, initial, bd);
            this.SRV = new ShaderResourceView(dev, this.Buffer);
        }

        protected override void OnDispose()
        {
            if (this.SRV != null) { this.SRV.Dispose(); }
            base.OnDispose();
        }
    }
}
