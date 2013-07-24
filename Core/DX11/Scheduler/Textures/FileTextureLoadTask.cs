using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FeralTic.DX11.Resources;

namespace FeralTic.DX11
{
    public abstract class FileTextureLoadTask<T> : DX11AbstractLoadTask<T> where T : IDX11Resource
    {
        protected string path;

        public FileTextureLoadTask(DX11RenderContext context, string path)
            : base(context)
        {
            this.path = path;
        }

        public override void Dispose()
        {
            if (this.Resource != null) { this.Resource.Dispose(); }
        }
    }

    public class FileTexture2dLoadTask : FileTextureLoadTask<DX11Texture2D>
    {
        public FileTexture2dLoadTask(DX11RenderContext context, string path)
            : base(context, path)
        {
        }

        protected override void DoProcess()
        {
            this.Resource = DX11Texture2D.FromFile(this.Context, path);
        }
    }

    public class FileTexture1dLoadTask : FileTextureLoadTask<DX11Texture1D>
    {
        public FileTexture1dLoadTask(DX11RenderContext context, string path)
            : base(context, path)
        {
        }

        protected override void DoProcess()
        {
            this.Resource = DX11Texture1D.FromFile(this.Context, path);
        }
    }

    public class FileTexture3dLoadTask : FileTextureLoadTask<DX11Texture3D>
    {
        public FileTexture3dLoadTask(DX11RenderContext context, string path)
            : base(context, path)
        {
        }

        protected override void DoProcess()
        {
            this.Resource = DX11Texture3D.FromFile(this.Context, path);
        }
    }
}
