using SlimDX.Direct3D11;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FeralTic.DX11.States
{
    public enum eDX11ViewportSpace { Clip, Pixel }

    public class DX11ViewportState
    {
        public Viewport Viewport;
        public eDX11ViewportSpace Space;


    }
}
