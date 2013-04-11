using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SlimDX.Direct3D11;

namespace FeralTic.DX11.Resources
{
    public interface IDX11GeometryDrawer
    {
        void Draw(DeviceContext ctx);
    }

    /// <summary>
    /// Drawer for DX11 geometry
    /// </summary>
    /// <typeparam name="T">Geometry type</typeparam>
    public interface IDX11GeometryDrawer<T> : IDX11GeometryDrawer where T : IDX11Geometry
    {
        /// <summary>
        /// Assigns the geometry to the drawer
        /// </summary>
        /// <param name="geometry"></param>
        void Assign(T geometry);

        /// <summary>
        /// Prepares geometry input assembler
        /// </summary>
        /// <param name="ctx">Device Context</param>
        /// <param name="layout">Input Layout</param>
        void PrepareInputAssembler(DeviceContext ctx, InputLayout layout);

        /// <summary>
        /// Draws geometry
        /// </summary>
        /// <param name="ctx">Device context</param>
        void Draw(DeviceContext ctx);
    }
}
