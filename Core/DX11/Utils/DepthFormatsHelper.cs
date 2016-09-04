using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SlimDX.DXGI;

namespace FeralTic.DX11.Utils
{
    /// <summary>
    /// Static class to provide format matching for depth.
    /// </summary>
    public static class DepthFormatsHelper
    {
        /// <summary>
        /// Gets format to bind a resource to. Since depth format from DSV and SRV is different
        /// </summary>
        /// <param name="depthformat">Depth format</param>
        /// <returns>Generic Format</returns>
        public static Format GetGenericTextureFormat(Format depthformat)
        {
            switch (depthformat)
            {
                case Format.D32_Float:
                    return Format.R32_Typeless;
                case Format.D24_UNorm_S8_UInt:
                    return Format.R24G8_Typeless;
                case Format.D16_UNorm:
                    return Format.R16_Typeless;
                case Format.D32_Float_S8X24_UInt:
                    return Format.R32G8X24_Typeless;
                default:
                    return Format.R32_Typeless; //Defaults as R32
            }
        }

        /// <summary>
        /// Gets a depth format do use in a DSV. It roughly returns same format,
        /// as input, unless the format is not supported (defaults as D32)
        /// </summary>
        /// <param name="depthformat">Depth format</param>
        /// <returns>Valid depth format Format</returns>
        public static Format GetDepthFormat(Format depthformat)
        {
            switch (depthformat)
            {
                case Format.D32_Float:
                    return Format.D32_Float;
                case Format.D24_UNorm_S8_UInt:
                    return Format.D24_UNorm_S8_UInt;
                case Format.D16_UNorm:
                    return Format.D16_UNorm;
                case Format.D32_Float_S8X24_UInt:
                    return Format.D32_Float_S8X24_UInt;
                default:
                    return Format.D32_Float; //Defaults as R32
            }
        }

        /// <summary>
        /// Gets format to bind a depth SRV to.
        /// </summary>
        /// <param name="depthformat">Depth format</param>
        /// <returns>SRV Format</returns>
        public static Format GetSRVFormat(Format depthformat)
        {
            switch (depthformat)
            {
                case Format.D32_Float:
                    return Format.R32_Float;
                case Format.D24_UNorm_S8_UInt:
                    return Format.R24_UNorm_X8_Typeless;
                case Format.D16_UNorm:
                    return Format.R16_UNorm;
                case Format.D32_Float_S8X24_UInt:
                    return Format.R32_Float_X8X24_Typeless;
                default:
                    return Format.R32_Float;
            }
        }

        /// <summary>
        /// Gets format to bind a stencil SRV to.
        /// </summary>
        /// <param name="depthformat">Depth format</param>
        /// <returns>SRV Format</returns>
        public static Format GetStencilSRVFormat(Format depthformat)
        {
            switch (depthformat)
            {
                case Format.D32_Float:
                    return Format.R32_Float; // Note, that means no stencil
                case Format.D24_UNorm_S8_UInt:
                    return Format.X24_Typeless_G8_UInt;
                case Format.D16_UNorm:
                    return Format.R16_UNorm; // No stencil here either
                case Format.D32_Float_S8X24_UInt:
                    return Format.X32_Typeless_G8X24_UInt;
                default:
                    return Format.R32_Float;
            }
        }
    }
}
