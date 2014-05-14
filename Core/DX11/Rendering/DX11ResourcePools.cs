using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SlimDX.Direct3D11;
using SlimDX.DXGI;

using FeralTic.DX11.Resources;
using FeralTic.DX11.Utils;


namespace FeralTic.DX11
{
    //sizeof(int) * 2, maxNumElements, eDX11BufferMode.Counter);
    public class DX11StructuredBufferPool : DX11ResourcePool<IDX11RWStructureBuffer>
    {
        public DX11StructuredBufferPool(DX11RenderContext context)
            : base(context)
        {

        }

        public DX11ResourcePoolEntry<IDX11RWStructureBuffer> Lock(int stride, int numelements,eDX11BufferMode mode = eDX11BufferMode.Default, bool oneframe = false)
        {
            foreach (DX11ResourcePoolEntry<IDX11RWStructureBuffer> entry in this.pool)
            {
                IDX11RWStructureBuffer tr = entry.Element;

                if (!entry.IsLocked && tr.ElementCount == numelements && tr.Stride == stride && tr.BufferType == mode)
                {
                    entry.Lock();
                    return entry;
                }
            }

            IDX11RWStructureBuffer res = new DX11RWStructuredBuffer(context.Device, numelements, stride, mode);

            DX11ResourcePoolEntry<IDX11RWStructureBuffer> newentry = new DX11ResourcePoolEntry<IDX11RWStructureBuffer>(res);

            this.pool.Add(newentry);

            return newentry;
        }
    }

    public class DX11RenderTargetPool : DX11ResourcePool<DX11RenderTarget2D>
    {
        public DX11RenderTargetPool(DX11RenderContext context)
            : base(context)
        {

        }

        public DX11ResourcePoolEntry<DX11RenderTarget2D> Lock(int w, int h, Format format, SampleDescription sd, bool genMM = false, int mmLevels = 1, bool oneframe = true)
        {
            foreach (DX11ResourcePoolEntry<DX11RenderTarget2D> entry in this.pool)
            {
                DX11RenderTarget2D tr = entry.Element;

                if (!entry.IsLocked && tr.Width == w && tr.Format == format && tr.Height == h
                    && tr.Resource.Description.SampleDescription.Count == sd.Count
                    && tr.Resource.Description.SampleDescription.Quality == sd.Quality
                    && tr.GenMipMaps == genMM && tr.RequestedMipLevels == mmLevels)
                {
                    entry.Lock();
                    return entry;
                }
            }

            DX11RenderTarget2D res = new DX11RenderTarget2D(this.context, w, h,sd, format, genMM, mmLevels);

            DX11ResourcePoolEntry<DX11RenderTarget2D> newentry = new DX11ResourcePoolEntry<DX11RenderTarget2D>(res);

            this.pool.Add(newentry);

            return newentry;
        }
    }

    public class DX11DepthStencilPool : DX11ResourcePool<DX11DepthStencil>
    {
        public DX11DepthStencilPool(DX11RenderContext context)
            : base(context)
        {

        }

        public DX11ResourcePoolEntry<DX11DepthStencil> Lock(int w, int h, Format format, SampleDescription sd)
        {
            foreach (DX11ResourcePoolEntry<DX11DepthStencil> entry in this.pool)
            {
                DX11DepthStencil tr = entry.Element;

                if (!entry.IsLocked && tr.Width == w && tr.Format == DepthFormatsHelper.GetGenericTextureFormat(format) && tr.Height == h
                    && tr.Resource.Description.SampleDescription.Count == sd.Count
                    && tr.Resource.Description.SampleDescription.Quality == sd.Quality)
                {
                    entry.Lock();
                    return entry;
                }
            }

            DX11DepthStencil res = new DX11DepthStencil(this.context, w, h, sd, format);

            DX11ResourcePoolEntry<DX11DepthStencil> newentry = new DX11ResourcePoolEntry<DX11DepthStencil>(res);

            this.pool.Add(newentry);

            return newentry;
        }
    }

    public class DX11VertexBufferPool : DX11ResourcePool<DX11VertexBuffer>
    {
        public DX11VertexBufferPool(DX11RenderContext context)
            : base(context)
        {

        }

        public DX11ResourcePoolEntry<DX11VertexBuffer> Lock(int verticescount, int vertexsize, bool allowstreamout)
        {
            //We can lock any buffer of the right size
            int totalsize = vertexsize * verticescount;

            foreach (DX11ResourcePoolEntry<DX11VertexBuffer> entry in this.pool)
            {
                DX11VertexBuffer tr = entry.Element;


                if (!entry.IsLocked && totalsize == tr.TotalSize && allowstreamout == tr.AllowStreamOutput)
                {
                    entry.Lock();
                    return entry;
                }
            }

            DX11VertexBuffer res = new DX11VertexBuffer(this.context, verticescount, vertexsize, allowstreamout);

            DX11ResourcePoolEntry<DX11VertexBuffer> newentry = new DX11ResourcePoolEntry<DX11VertexBuffer>(res);

            this.pool.Add(newentry);

            return newentry;
        }
    }

    public class DX11VolumeTexturePool : DX11ResourcePool<DX11RenderTexture3D>
    {
        public DX11VolumeTexturePool(DX11RenderContext context)
            : base(context)
        {

        }

        public DX11ResourcePoolEntry<DX11RenderTexture3D> Lock(int w, int h, int d,Format format)
        {
            foreach (DX11ResourcePoolEntry<DX11RenderTexture3D> entry in this.pool)
            {
                DX11RenderTexture3D tr = entry.Element;

                if (!entry.IsLocked && tr.Width == w && tr.Format == format && tr.Height == h && tr.Depth == d)
                {
                    entry.Lock();
                    return entry;
                }
            }

            DX11RenderTexture3D res = new DX11RenderTexture3D(this.context, w, h, d, format);

            DX11ResourcePoolEntry<DX11RenderTexture3D> newentry = new DX11ResourcePoolEntry<DX11RenderTexture3D>(res);

            this.pool.Add(newentry);

            return newentry;
        }
    }
}
