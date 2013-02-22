using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SlimDX.DXGI;

using FeralTic.DX11.Resources;

namespace FeralTic.DX11
{
    public class DX11ResourcePoolManager : IDisposable
    {
        private DX11RenderContext ctx;

        private DX11RenderTargetPool targetpool;
        private DX11StructuredBufferPool sbufferpool;
        private DX11VolumeTexturePool volumepool;
        private DX11DepthStencilPool depthpool;

        public DX11ResourcePoolManager(DX11RenderContext ctx)
        {
            this.ctx = ctx;
            this.targetpool = new DX11RenderTargetPool(this.ctx);
            this.sbufferpool = new DX11StructuredBufferPool(this.ctx);
            this.volumepool = new DX11VolumeTexturePool(this.ctx);
            this.depthpool = new DX11DepthStencilPool(this.ctx);
        }

        public void BeginFrame()
        {
            //this.targetpool.ResetFrame();
        }

        public DX11ResourcePoolEntry<DX11RenderTarget2D> LockRenderTarget(int w, int h, Format format, bool genMM = false, int mmLevels = 1, bool singleframe = true)
        {
            return this.targetpool.Lock(w, h, format, new SampleDescription(1, 0), genMM, mmLevels, singleframe);
        }

        public DX11ResourcePoolEntry<DX11RenderTarget2D> LockRenderTarget(int w, int h, Format format, SampleDescription sd, bool genMM = false, int mmLevels = 1)
        {
            return this.targetpool.Lock(w, h, format, sd, genMM, mmLevels);
        }

        public DX11ResourcePoolEntry<IDX11RWStructureBuffer> LockStructuredBuffer(int stride, int numelements, eDX11BufferMode mode = eDX11BufferMode.Default, bool oneframe = false)
        {
            return this.sbufferpool.Lock(stride, numelements, mode, oneframe);
        }

        public DX11ResourcePoolEntry<DX11RenderTexture3D> LockVolume(int w, int h, int d, Format format)
        {
            return this.volumepool.Lock(w, h, d, format);
        }

        public DX11ResourcePoolEntry<DX11DepthStencil> LockDepth(int w, int h, SampleDescription sd, Format format)
        {
            return this.depthpool.Lock(w, h, format,sd);
        }

        public void Unlock(DX11RenderTarget2D target)
        {
            this.targetpool.UnLock(target);
        }

        public void Unlock(IDX11RWStructureBuffer target)
        {
            this.sbufferpool.UnLock(target);
        }

        public void Unlock(DX11RenderTexture3D target)
        {
            this.volumepool.UnLock(target);
        }

        public void Unlock(DX11DepthStencil target)
        {
            this.depthpool.UnLock(target);
        }

        /*public void UnlockStructuredBuffers()
        {
            this.sbufferpool.UnlockAll();
        }*/

        public void ClearUnlocked()
        {
            this.sbufferpool.ClearUnlocked();
            this.targetpool.ClearUnlocked();
            this.volumepool.ClearUnlocked();
        }

        public void ClearBuffers()
        {
            this.sbufferpool.Dispose();
        }

        public int RenderTargetCount
        {
            get { return this.targetpool.Count; }
        }

        public int BufferCount
        {
            get { return this.sbufferpool.Count; }
        }

        public void Dispose()
        {
            this.targetpool.Dispose();
            this.sbufferpool.Dispose();
            this.volumepool.Dispose();
        }
    }
}
