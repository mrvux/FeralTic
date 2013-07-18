using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SlimDX.Direct3D11;
using SlimDX.DXGI;

using Device = SlimDX.Direct3D11.Device;

namespace FeralTic.DX11.Utils
{
    public class FormatHelper
    {
        private Dictionary<SlimDX.DXGI.Format, int> formatsize = new Dictionary<SlimDX.DXGI.Format, int>();
        private Dictionary<string, SlimDX.DXGI.Format> semantic = new Dictionary<string, SlimDX.DXGI.Format>();
        private static FormatHelper instance;

        public static FormatHelper Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new FormatHelper();
                    instance.InitializeSize();
                }
                return instance;
            }
        }


        #region Is Supported
        /// <summary>
        /// Checks if a format is supported for a specific usage
        /// </summary>
        /// <param name="dev">Device to check</param>
        /// <param name="usage">Desired format usage</param>
        /// <param name="format">Desired format</param>
        /// <returns>true if format supported, false otherwise</returns>
        public bool IsSupported(Device dev, FormatSupport usage, Format format)
        {
            FormatSupport support = dev.CheckFormatSupport(format);
            return (support | usage) == support;
        }
        #endregion

        #region Supported Formats
        /// <summary>
        /// Lists supported DXGI formats for a given usage
        /// </summary>
        /// <param name="dev">Device to check format support for</param>
        /// <param name="usage">Requested Usage</param>
        /// <returns>List of Supported formats</returns>
        public List<string> SupportedFormats(Device dev, FormatSupport usage)
        {
            List<string> result = new List<string>();
            foreach (string s in Enum.GetNames(typeof(Format)))
            {
                if (IsSupported(dev, usage, (Format)Enum.Parse(typeof(Format), s)))
                {
                    result.Add(s);
                }
            }
            return result;
        }
        #endregion


        public int GetSize(SlimDX.DXGI.Format format)
        {
            return this.formatsize[format];
        }

        public SlimDX.DXGI.Format GetFormat(string semantic)
        {
            if (this.semantic.ContainsKey(semantic))
            {
                return this.semantic[semantic];
            }
            return SlimDX.DXGI.Format.Unknown;
        }

        private void InitializeSemantic()
        {
            this.semantic.Add("float", SlimDX.DXGI.Format.R32_Float);
            this.semantic.Add("float2", SlimDX.DXGI.Format.R32G32_Float);
            this.semantic.Add("float3", SlimDX.DXGI.Format.R32G32B32_Float);
            this.semantic.Add("float4", SlimDX.DXGI.Format.R32G32B32A32_Float);

            this.semantic.Add("int", SlimDX.DXGI.Format.R32G32_SInt);
            this.semantic.Add("int2", SlimDX.DXGI.Format.R32G32_SInt);
            this.semantic.Add("int3", SlimDX.DXGI.Format.R32G32B32_SInt);
            this.semantic.Add("int4", SlimDX.DXGI.Format.R32G32B32A32_SInt);

            this.semantic.Add("uint", SlimDX.DXGI.Format.R32_UInt);
            this.semantic.Add("uint2", SlimDX.DXGI.Format.R32G32_UInt);
            this.semantic.Add("uint3", SlimDX.DXGI.Format.R32G32B32_UInt);
            this.semantic.Add("uint4", SlimDX.DXGI.Format.R32G32B32A32_UInt);
        }

        private void InitializeSize()
        {
            this.formatsize.Add(SlimDX.DXGI.Format.A8_UNorm, 1);
            this.formatsize.Add(SlimDX.DXGI.Format.B5G5R5A1_UNorm, 2);
            this.formatsize.Add(SlimDX.DXGI.Format.B5G6R5_UNorm, 2);
            this.formatsize.Add(SlimDX.DXGI.Format.B8G8R8A8_Typeless, 4);
            this.formatsize.Add(SlimDX.DXGI.Format.B8G8R8A8_UNorm, 4);
            this.formatsize.Add(SlimDX.DXGI.Format.B8G8R8A8_UNorm_SRGB, 4);
            this.formatsize.Add(SlimDX.DXGI.Format.B8G8R8X8_Typeless, 4);
            this.formatsize.Add(SlimDX.DXGI.Format.B8G8R8X8_UNorm, 4);
            this.formatsize.Add(SlimDX.DXGI.Format.B8G8R8X8_UNorm_SRGB, 4);

            this.formatsize.Add(SlimDX.DXGI.Format.BC1_Typeless, 0); //Fix
            this.formatsize.Add(SlimDX.DXGI.Format.BC1_UNorm, 0);
            this.formatsize.Add(SlimDX.DXGI.Format.BC1_UNorm_SRGB, 0);

            this.formatsize.Add(SlimDX.DXGI.Format.BC2_Typeless, 0); //Fix
            this.formatsize.Add(SlimDX.DXGI.Format.BC2_UNorm, 0);
            this.formatsize.Add(SlimDX.DXGI.Format.BC2_UNorm_SRGB, 0);

            this.formatsize.Add(SlimDX.DXGI.Format.BC3_Typeless, 0); //Fix
            this.formatsize.Add(SlimDX.DXGI.Format.BC3_UNorm, 0);
            this.formatsize.Add(SlimDX.DXGI.Format.BC3_UNorm_SRGB, 0);

            this.formatsize.Add(SlimDX.DXGI.Format.BC4_SNorm, 0); //Fix
            this.formatsize.Add(SlimDX.DXGI.Format.BC4_Typeless, 0);
            this.formatsize.Add(SlimDX.DXGI.Format.BC4_UNorm, 0);

            this.formatsize.Add(SlimDX.DXGI.Format.BC5_SNorm, 0); //Fix
            this.formatsize.Add(SlimDX.DXGI.Format.BC5_Typeless, 0);
            this.formatsize.Add(SlimDX.DXGI.Format.BC5_UNorm, 0);

            this.formatsize.Add(SlimDX.DXGI.Format.BC6_SFloat16, 0); //Fix
            this.formatsize.Add(SlimDX.DXGI.Format.BC6_Typeless, 0);
            this.formatsize.Add(SlimDX.DXGI.Format.BC6_UFloat16, 0);

            this.formatsize.Add(SlimDX.DXGI.Format.BC7_Typeless, 0); //Fix
            this.formatsize.Add(SlimDX.DXGI.Format.BC7_UNorm, 0);
            this.formatsize.Add(SlimDX.DXGI.Format.BC7_UNorm_SRGB, 0);

            this.formatsize.Add(SlimDX.DXGI.Format.D16_UNorm, 2); //Fix
            this.formatsize.Add(SlimDX.DXGI.Format.D24_UNorm_S8_UInt, 4);
            this.formatsize.Add(SlimDX.DXGI.Format.D32_Float, 4);
            this.formatsize.Add(SlimDX.DXGI.Format.D32_Float_S8X24_UInt, 8);

            this.formatsize.Add(SlimDX.DXGI.Format.G8R8_G8B8_UNorm, 4);

            this.formatsize.Add(SlimDX.DXGI.Format.R1_UNorm, 0);

            this.formatsize.Add(SlimDX.DXGI.Format.R10G10B10_XR_Bias_A2_UNorm, 4);
            this.formatsize.Add(SlimDX.DXGI.Format.R10G10B10A2_Typeless, 4);
            this.formatsize.Add(SlimDX.DXGI.Format.R10G10B10A2_UInt, 4);
            this.formatsize.Add(SlimDX.DXGI.Format.R10G10B10A2_UNorm, 4);

            this.formatsize.Add(SlimDX.DXGI.Format.R11G11B10_Float, 4);

            this.formatsize.Add(SlimDX.DXGI.Format.R16_Float, 2);
            this.formatsize.Add(SlimDX.DXGI.Format.R16_SInt, 2);
            this.formatsize.Add(SlimDX.DXGI.Format.R16_SNorm, 2);
            this.formatsize.Add(SlimDX.DXGI.Format.R16_Typeless, 2);
            this.formatsize.Add(SlimDX.DXGI.Format.R16_UInt, 2);
            this.formatsize.Add(SlimDX.DXGI.Format.R16_UNorm, 2);

            this.formatsize.Add(SlimDX.DXGI.Format.R16G16_Float, 4);
            this.formatsize.Add(SlimDX.DXGI.Format.R16G16_SInt, 4);
            this.formatsize.Add(SlimDX.DXGI.Format.R16G16_SNorm, 4);
            this.formatsize.Add(SlimDX.DXGI.Format.R16G16_Typeless, 4);
            this.formatsize.Add(SlimDX.DXGI.Format.R16G16_UInt, 4);
            this.formatsize.Add(SlimDX.DXGI.Format.R16G16_UNorm, 4);

            this.formatsize.Add(SlimDX.DXGI.Format.R16G16B16A16_Float, 8);
            this.formatsize.Add(SlimDX.DXGI.Format.R16G16B16A16_SInt, 8);
            this.formatsize.Add(SlimDX.DXGI.Format.R16G16B16A16_SNorm, 8);
            this.formatsize.Add(SlimDX.DXGI.Format.R16G16B16A16_Typeless, 8);
            this.formatsize.Add(SlimDX.DXGI.Format.R16G16B16A16_UInt, 8);
            this.formatsize.Add(SlimDX.DXGI.Format.R16G16B16A16_UNorm, 8);

            this.formatsize.Add(SlimDX.DXGI.Format.R24_UNorm_X8_Typeless, 4);
            this.formatsize.Add(SlimDX.DXGI.Format.R24G8_Typeless, 4);

            this.formatsize.Add(SlimDX.DXGI.Format.R32_Float, 4);
            this.formatsize.Add(SlimDX.DXGI.Format.R32_Float_X8X24_Typeless, 8);
            this.formatsize.Add(SlimDX.DXGI.Format.R32_SInt, 4);
            this.formatsize.Add(SlimDX.DXGI.Format.R32_Typeless, 4);
            this.formatsize.Add(SlimDX.DXGI.Format.R32_UInt, 4);

            this.formatsize.Add(SlimDX.DXGI.Format.R32G32_Float, 8);
            this.formatsize.Add(SlimDX.DXGI.Format.R32G32_SInt, 8);
            this.formatsize.Add(SlimDX.DXGI.Format.R32G32_Typeless, 8);
            this.formatsize.Add(SlimDX.DXGI.Format.R32G32_UInt, 8);

            this.formatsize.Add(SlimDX.DXGI.Format.R32G32B32_Float, 12);
            this.formatsize.Add(SlimDX.DXGI.Format.R32G32B32_SInt, 12);
            this.formatsize.Add(SlimDX.DXGI.Format.R32G32B32_Typeless, 12);
            this.formatsize.Add(SlimDX.DXGI.Format.R32G32B32_UInt, 12);

            this.formatsize.Add(SlimDX.DXGI.Format.R32G32B32A32_Float, 16);
            this.formatsize.Add(SlimDX.DXGI.Format.R32G32B32A32_SInt, 16);
            this.formatsize.Add(SlimDX.DXGI.Format.R32G32B32A32_Typeless, 16);
            this.formatsize.Add(SlimDX.DXGI.Format.R32G32B32A32_UInt, 16);

            this.formatsize.Add(SlimDX.DXGI.Format.R32G8X24_Typeless, 8);

            this.formatsize.Add(SlimDX.DXGI.Format.R8_SInt, 1);
            this.formatsize.Add(SlimDX.DXGI.Format.R8_SNorm, 1);
            this.formatsize.Add(SlimDX.DXGI.Format.R8_Typeless, 1);
            this.formatsize.Add(SlimDX.DXGI.Format.R8_UInt, 1);
            this.formatsize.Add(SlimDX.DXGI.Format.R8_UNorm, 1);

            this.formatsize.Add(SlimDX.DXGI.Format.R8G8_B8G8_UNorm, 4);

            this.formatsize.Add(SlimDX.DXGI.Format.R8G8_SInt, 2);
            this.formatsize.Add(SlimDX.DXGI.Format.R8G8_SNorm, 2);
            this.formatsize.Add(SlimDX.DXGI.Format.R8G8_Typeless, 2);
            this.formatsize.Add(SlimDX.DXGI.Format.R8G8_UInt, 2);
            this.formatsize.Add(SlimDX.DXGI.Format.R8G8_UNorm, 2);

            this.formatsize.Add(SlimDX.DXGI.Format.R8G8B8A8_SInt, 4);
            this.formatsize.Add(SlimDX.DXGI.Format.R8G8B8A8_SNorm, 4);
            this.formatsize.Add(SlimDX.DXGI.Format.R8G8B8A8_Typeless, 4);
            this.formatsize.Add(SlimDX.DXGI.Format.R8G8B8A8_UInt, 4);
            this.formatsize.Add(SlimDX.DXGI.Format.R8G8B8A8_UNorm, 4);
            this.formatsize.Add(SlimDX.DXGI.Format.R8G8B8A8_UNorm_SRGB, 4);

            this.formatsize.Add(SlimDX.DXGI.Format.R9G9B9E5_SharedExp, 4);
            this.formatsize.Add(SlimDX.DXGI.Format.Unknown, 0);
            this.formatsize.Add(SlimDX.DXGI.Format.X24_Typeless_G8_UInt, 4);
            this.formatsize.Add(SlimDX.DXGI.Format.X32_Typeless_G8X24_UInt, 8);
        }

    }
}
