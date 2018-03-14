using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FeralTic.DX11
{
    /// <summary>
    /// Enumeration with a list of useful depth stencil presets
    /// </summary>
    public enum DepthStencilStatePreset : int
    {
        /// <summary>
        /// Less depth comparison, no depth write, no stencil
        /// </summary>
        LessRead = 0,
        /// <summary>
        /// No depth, no stencil
        /// </summary>
        NoDepth = 1,
        /// <summary>
        /// Less or equal depth comparison, no depth write, no stencil
        /// </summary>     
        LessEqualRead = 2,
        /// <summary>
        /// Less depth comparison, with depth write, no stencil
        /// </summary>  
        LessReadWrite = 3,
        /// <summary>
        /// Less equal depth comparison, with depth write, no stencil
        /// </summary>  
        LessEqualReadWrite = 4,
        /// <summary>
        /// Always writes to depth (no depth culling), no stancil (use for raymarch for example)
        /// </summary>  
        WriteOnly = 5,
        /// <summary>
        /// Less comparison, with depth write, increments stencil on each pixel drawn
        /// </summary>  
        LessReadStencilIncrement = 6,
        /// <summary>
        /// Less comparison, with depth write, sets stencil back to zero if pixel drawn
        /// </summary>  
        LessReadStencilZero = 7,
        /// <summary>
        /// No depth, only writes is stencil is less than reference
        /// </summary>  
        StencilLess = 8,
        /// <summary>
        /// No depth, only writes is stencil is more than reference
        /// </summary>  
        StencilGreater = 9,
        /// <summary>
        /// No depth, increments stencil when drawn
        /// </summary> 
        StencilIncrement = 10,
        /// <summary>
        /// No depth, inverts stencil when drawn
        /// </summary> 
        StencilInvert = 11,
        /// <summary>
        /// No depth, replaces stencil by reference value when drawn
        /// </summary> 
        StencilReplace = 12
    }
}
