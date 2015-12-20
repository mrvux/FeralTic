using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace FeralTic.DX11
{
    public static class ShaderMacroExtensionMethods
    {
        public static SharpDX.Direct3D.ShaderMacro AsSharpDXMacro(this SlimDX.D3DCompiler.ShaderMacro macro)
        {
            return new SharpDX.Direct3D.ShaderMacro(macro.Name, macro.Value);
        }

        public static SharpDX.Direct3D.ShaderMacro[] AsSharpDXMacro(this SlimDX.D3DCompiler.ShaderMacro[] macro)
        {
            return macro.Select(m => m.AsSharpDXMacro()).ToArray();
        }
    }
}
