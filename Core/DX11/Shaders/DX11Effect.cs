using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SlimDX.Direct3D11;
using SlimDX.D3DCompiler;
using System.IO;
using SlimDX;
using System.Reflection;
using System.Windows.Forms;

namespace FeralTic.DX11
{
    public class FolderIncludeHandler : Include
    {
        public string BaseShaderPath { get; set; }

        private string sysincludepath;

        public FolderIncludeHandler()
        {
            bool fallback = false;
            try
            {
                string path = System.Configuration.ConfigurationManager.AppSettings["SysIncludePath"];
                fallback = Directory.Exists(path) == false;
                this.sysincludepath = path;
            }
            catch
            {
                fallback = true;
            }

            if (fallback)
            {
                this.sysincludepath = Path.GetDirectoryName(Application.ExecutablePath);
            }
        }

        public void Close(Stream stream)
        {
            /*if (stream != null)
            {
                stream.Dispose();
            }*/
        }

        public void Open(IncludeType type, string fileName, Stream parentStream, out Stream stream)
        {
            string path;

            if (type == IncludeType.Local)
            {
                string ppath = this.BaseShaderPath;
                FileStream ps = parentStream as FileStream;

                if( ps != null ){
                    ppath = ppath = Path.GetDirectoryName(ps.Name);
                }

                // Attempt to include relative to current file
                path = Path.Combine(ppath, fileName);
                if (File.Exists(path))
                {
                    stream = new FileStream(path, FileMode.Open, FileAccess.Read);
                    return;
                }
                
                // Attempt to include relative to origin file
                path = Path.Combine(this.BaseShaderPath, fileName);
                if (File.Exists(path))
                {
                    stream = new FileStream(path, FileMode.Open, FileAccess.Read);
                    return;
                }
            }

            // If system type include or if the file was not found for a local include
            path = Path.Combine(this.sysincludepath, fileName);
            if (File.Exists(path))
            {
                stream = new FileStream(path, FileMode.Open, FileAccess.Read);
            }
            else
            {
                stream = null;
            }
        }
    }

    /// <summary>
    /// Effect Compiler
    /// Compiles effect against a null device, so reflection can be kept even
    /// in case of device change
    /// </summary>
    public class DX11Effect : IDisposable
    {
        private static FolderIncludeHandler folderhandler = new FolderIncludeHandler();

        private DX11Effect()
        {
            //To prevent instancing
        }

        #region Load from byte code
        private static DX11Effect LoadByteCode(byte[] bytecode)
        {
            DX11Effect shader = new DX11Effect();
            try
            {
                DataStream ds = new DataStream(bytecode.Length, true, true);
                ds.Write(bytecode, 0, bytecode.Length);
                shader.ByteCode = new ShaderBytecode(ds);

                //fs.Close();
                ds.Dispose();

                shader.IsCompiled = true;
                shader.ErrorMessage = "";

                shader.Preprocess();

            }
            catch (Exception ex)
            {
                shader.IsCompiled = false;
                shader.ErrorMessage = ex.Message;
                shader.DefaultEffect = null;
            }
            return shader;
        }
        #endregion

        #region Compile (from string)
        private static DX11Effect Compile(string content, bool isfile, Include include, ShaderMacro[] defines)
        {
            DX11Effect shader = new DX11Effect();

            string errors;
            try
            {
                ShaderFlags flags = ShaderFlags.OptimizationLevel1;

                if (isfile)
                {
                    shader.ByteCode = ShaderBytecode.CompileFromFile(content, "fx_5_0", flags, EffectFlags.None, defines, include, out errors);
                }
                else
                {
                    shader.ByteCode = ShaderBytecode.Compile(content, "fx_5_0", flags, EffectFlags.None, defines, include, out errors);
                }

                //Compilation worked, but we can still have warning
                shader.IsCompiled = true;
                shader.ErrorMessage = errors;

                shader.Preprocess();

            }
            catch (Exception ex)
            {
                shader.IsCompiled = false;
                shader.ErrorMessage = ex.Message;
                shader.DefaultEffect = null;
            }
            return shader;
        }
        #endregion

        #region Overload utils
        public static DX11Effect FromString(string code)
        {
            return Compile(code, false, null, null);
        }

        public static DX11Effect FromString(string code, Include include)
        {
            return Compile(code, false, include, null);
        }

        public static DX11Effect FromString(string code, Include include, ShaderMacro[] defines)
        {
            return Compile(code, false, include, defines);
        }



        public static DX11Effect FromFile(string path, ShaderMacro[] defines)
        {
            folderhandler.BaseShaderPath = Path.GetDirectoryName(path);
            return Compile(path, true, folderhandler, defines);
        }

        public static DX11Effect FromFile(string path)
        {
            folderhandler.BaseShaderPath = Path.GetDirectoryName(path);
            return Compile(path, true, folderhandler, null);
        }

        public static DX11Effect FromResource(Assembly assembly, string path)
        {
            var textStreamReader = new StreamReader(assembly.GetManifestResourceStream(path));
            string code = textStreamReader.ReadToEnd();
            textStreamReader.Dispose();
            return Compile(code, false, null, null);
        }

        public static DX11Effect FromFile(string path, Include include, ShaderMacro[] defines)
        {
            return Compile(path, true, include, defines);
        }

        public static DX11Effect FromFile(string path, Include include)
        {
            return Compile(path, true, include, null);
        }

        public static DX11Effect FromByteCode(byte[] bytecode)
        {
            return LoadByteCode(bytecode);
        }

        public static DX11Effect FromByteCode(string path)
        {
            FileStream fs = new FileStream(path, FileMode.Open);
            byte[] data = new byte[fs.Length];
            fs.Read(data, 0, (int)fs.Length);
            fs.Close();
            return LoadByteCode(data);
        }
        #endregion

        #region Pre process
        private void Preprocess()
        {
            //Bind to null device 
            this.DefaultEffect = new Effect(NullRenderDevice.Device, this.ByteCode);

            //Set techniques
            int techcnt = this.DefaultEffect.Description.TechniqueCount;

            this.TechniqueNames = new string[techcnt];
            this.TechniqueValids = new bool[techcnt];

            for (int i = 0; i < techcnt; i++)
            {
                EffectTechnique technique = this.DefaultEffect.GetTechniqueByIndex(i);

                this.TechniqueNames[i] = technique.Description.Name;
                this.TechniqueValids[i] = technique.IsValid;
            }
        }
        #endregion

        #region Save Byte code
        public bool SaveByteCode(string path)
        {
            FileStream fs = null;
            bool result = false;
            try
            {
                fs = new FileStream(path, FileMode.Create, FileAccess.Write);
                byte[] b = new byte[this.ByteCode.Data.Length];

                this.ByteCode.Data.Read(b, 0, b.Length);
                fs.Write(b, 0, b.Length);
                result = true;
            }
            catch
            {

            }
            finally
            {
                if (fs != null) { fs.Close(); }
            }

            return result;
        }
        #endregion

        //Compilation results
        public ShaderBytecode ByteCode { get; private set; }
        public Effect DefaultEffect { get; private set; }
        public string ErrorMessage { get; private set; }
        public bool IsCompiled { get; private set; }

        //Techniques
        public bool[] TechniqueValids { get; private set; }
        public string[] TechniqueNames { get; private set; }


        public void Dispose()
        {
            if (this.DefaultEffect != null) { this.DefaultEffect.Dispose(); }
            if (this.ByteCode != null) { this.ByteCode.Dispose(); }
        }
    }
}
