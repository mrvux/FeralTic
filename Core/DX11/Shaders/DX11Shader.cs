using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SlimDX.Direct3D11;
using SlimDX.D3DCompiler;
using System.IO;
using SlimDX;
using System.Reflection;

namespace FeralTic.DX11
{
    /// <summary>
    /// Effect Compiler
    /// Compiles effect against a null device, so reflection can be kept even
    /// in case of device change
    /// </summary>
    public class DX11ShaderCompiler
    {
        #region Compile (from string)
        private static ShaderBytecode Compile(string content, bool isfile, Include include,string profile,string entrypoint)
        {
            try
            {
                string errors;

                if (isfile)
                {
                    return ShaderBytecode.CompileFromFile(content,entrypoint, profile, ShaderFlags.OptimizationLevel2, EffectFlags.None, null, include, out errors);
                }
                else
                {
                    return ShaderBytecode.Compile(content,entrypoint, profile, ShaderFlags.OptimizationLevel2, EffectFlags.None, null, include, out errors);
                }

            }
            catch (Exception ex)
            {
                return null;
            }
        }
        #endregion


        public static ShaderBytecode FromResource(Assembly assembly, string path, string profile, string entrypoint)
        {
            var textStreamReader = new StreamReader(assembly.GetManifestResourceStream(path));
            string code = textStreamReader.ReadToEnd();
            textStreamReader.Dispose();
            return Compile(code, false, null,profile,entrypoint);
        }






    }
}
