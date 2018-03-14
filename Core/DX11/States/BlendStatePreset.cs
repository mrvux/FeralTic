using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FeralTic.DX11
{
    /// <summary>
    /// Presets for various blend states
    /// </summary>
    public enum BlendStatePreset : int
    {
        /// <summary>
        /// No blending
        /// </summary>
        Disabled = 0,
        /// <summary>
        /// Fully additive
        /// </summary>
        Add = 1,
        /// <summary>
        /// Standard alpha blend
        /// </summary>
        Blend = 2,
        /// <summary>
        /// Mulptiply colors
        /// </summary>
        Multiply = 3,
        /// <summary>
        /// Additive, but uses alpha on source to control amount
        /// </summary>
        AlphaAdd = 4,
        /// <summary>
        /// Text default, or premultiplied alpha blend
        /// </summary>
        TextDefault = 5,
        /// <summary>
        /// Keep existing values, can be used to avoid drawing entirely, while keeping depth stencil writes
        /// </summary>
        Keep = 6,
        /// <summary>
        /// Uses constant blend factor to control alpha blending
        /// </summary>
        ConstantFactor = 7,
        /// <summary>
        /// Alpha blend, but uses value in render target to control opacity (as mask)
        /// </summary>
        BlendDestination = 8,
        /// <summary>
        /// Replaces alpha value only, disable color write
        /// </summary>
        ReplaceAlpha = 9,
        /// <summary>
        /// Multiply and write alpha only, disable color write
        /// </summary>
        MultiplyAlpha = 10
    }
}
