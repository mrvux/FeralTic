using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SlimDX.Direct3D11;
using SlimDX;

namespace FeralTic.DX11.Resources
{
    /// <summary>
    /// Base interface for any geometry
    /// </summary>
    public interface IDX11Geometry : IDX11Resource, IDisposable
    {
        /// <summary>
        /// Default topology for our geometry
        /// </summary>
        PrimitiveTopology Topology { get; set; }

        InputElement[] InputLayout { get; set; }

        /// <summary>
        /// Creates an input layout 
        /// </summary>
        /// <param name="pass">Effect pass to validate layout on</param>
        /// <param name="layout">Returns validate layout, or null if not valid</param>
        /// <returns>true if layout valid, false otherwise</returns>
        bool ValidateLayout(EffectPass pass,out InputLayout layout);

        void Draw();

        void Bind(InputLayout layout);

        BoundingBox BoundingBox { get; set; }

        bool HasBoundingBox { get; set; }

        IDX11Geometry ShallowCopy();
    }
}
