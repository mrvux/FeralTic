using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SlimDX.Direct3D11;
using Device = SlimDX.Direct3D11.Device;
using SlimDX.DXGI;

using FeralTic.Utils;
using FeralTic.DX11.Geometry;
using FeralTic.DX11.Resources;
using FeralTic.DX11.StockEffects;

namespace FeralTic.DX11
{
    public class DX11BasicEffects : IDisposable
    {
        private DX11RenderContext context;

        private PointSamplerPSPass pointsamplerPS;

        

        public DX11BasicEffects(DX11RenderContext context)
        {
            this.context = context;
            this.pointsamplerPS = new PointSamplerPSPass(context);
        }

        public PointSamplerPSPass PointSamplerPixelPass => this.pointsamplerPS;

        public void Dispose()
        {
            if (this.pointsamplerPS != null)
            {
                this.pointsamplerPS.Dispose();
                this.pointsamplerPS = null;
            }
        }
    }
}
