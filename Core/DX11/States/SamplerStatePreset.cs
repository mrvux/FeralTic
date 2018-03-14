using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FeralTic.DX11
{
    /// <summary>
    /// Presets for various sampler states
    /// </summary>
    public enum SamplerStatePreset : int
    {
        /// <summary>
        /// Linear sampling, wrap on borders
        /// </summary>
        LinearWrap,
        /// <summary>
        /// Linear sampling, clamp on borders
        /// </summary>
        LinearClamp,
        /// <summary>
        /// Linear sampling, solid color (black by default) on borders
        /// </summary>
        LinearBorder,
        /// <summary>
        /// Linear sampling, mirror borders
        /// </summary>
        LinearMirror,
        /// <summary>
        /// Point sampling, wrap on borders
        /// </summary>
        PointWrap,
        /// <summary>
        /// Point sampling, clamp on borders
        /// </summary>
        PointClamp,
        /// <summary>
        /// Point sampling, solid color (black by default) on borders
        /// </summary>
        PointBorder,
        /// <summary>
        /// Point sampling, mirror borders
        /// </summary>
        PointMirror
    }
}
