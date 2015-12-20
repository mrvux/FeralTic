using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FeralTic.DX11
{
    public class SharpDXIncludeWrapper : SharpDX.D3DCompiler.Include
    {
        private SlimDX.D3DCompiler.Include slimInclude;

        public SharpDXIncludeWrapper(SlimDX.D3DCompiler.Include slimInclude)
        {
            this.slimInclude = slimInclude;
        }

        public IDisposable Shadow
        {
            get; set;
        }

        public void Close(Stream stream)
        {
            this.slimInclude.Close(stream);
        }

        public void Dispose()
        {

        }

        public Stream Open(SharpDX.D3DCompiler.IncludeType type, string fileName, Stream parentStream)
        {
            SlimDX.D3DCompiler.IncludeType slimType = (SlimDX.D3DCompiler.IncludeType)(int)type;

            Stream stream;

            this.slimInclude.Open(slimType, fileName, parentStream, out stream);
            return stream;
        }
    }
}
