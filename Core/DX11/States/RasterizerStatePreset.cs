using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FeralTic.DX11
{
    /// <summary>
    /// Presets for various rasterizer states
    /// </summary>
    public enum RasterizerStatePreset : int
    {
        /// <summary>
        /// Back face culling
        /// </summary>
        BackCullSimple = 0,
        /// <summary>
        /// Back face culling , wireframe
        /// </summary>
        BackCullWireframe = 1,
        /// <summary>
        /// Front face culling
        /// </summary>
        FrontCullSimple = 2,
        /// <summary>
        /// Front face culling, wireframe
        /// </summary>
        FrontCullWireframe = 3,
        /// <summary>
        /// No face culling
        /// </summary>
        NoCullSimple = 4,
        /// <summary>
        /// No face culling, wireframe
        /// </summary>
        NoCullWireframe = 5,
        /// <summary>
        /// Line antialias, with alpha and blend
        /// </summary>
        LineAlpha = 6,
        /// <summary>
        /// Line quadrilateral
        /// </summary>
        LineQuadrilateral = 7

    }
}
