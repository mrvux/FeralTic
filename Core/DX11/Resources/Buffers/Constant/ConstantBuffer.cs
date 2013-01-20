using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

using SlimDX;
using SlimDX.Direct3D11;

namespace FeralTic.DX11.Resources
{
    /// <summary>
    /// Generic implementation for constant buffer
    /// </summary>
    /// <typeparam name="T">Buffer structure type</typeparam>
    public class DX11ConstantBuffer<T> where T :struct
    {
        private DX11RenderContext context;

        public DX11ConstantBuffer(DX11RenderContext context)
            : this(context, false)
        {
        }

        public DX11ConstantBuffer(DX11RenderContext context, bool align)
        {
            this.context = context;
            int size;
            if (align)
            {
                size = ((Marshal.SizeOf(typeof(T)) + 15) / 16) * 16;
            }
            else
            {
                size = Marshal.SizeOf(typeof(T));
            }

            BufferDescription bd = new BufferDescription()
            {
                BindFlags = BindFlags.ConstantBuffer,
                CpuAccessFlags = CpuAccessFlags.Write,
                OptionFlags = ResourceOptionFlags.None,
                SizeInBytes = size,
                Usage = ResourceUsage.Dynamic
            };

            this.Buffer = new SlimDX.Direct3D11.Buffer(context.Device, bd);
        }

        public T Data
        {
            set
            {
                DataBox db = context.CurrentDeviceContext.MapSubresource(this.Buffer, MapMode.WriteDiscard, MapFlags.None);
                db.Data.Write<T>(value);
                context.CurrentDeviceContext.UnmapSubresource(this.Buffer, 0);
            }
        }

        public SlimDX.Direct3D11.Buffer Buffer { get; protected set; }

        public void Dispose()
        {
            if (this.Buffer != null) { this.Buffer.Dispose(); }
        }

    }
}
