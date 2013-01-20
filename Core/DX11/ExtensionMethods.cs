using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SlimDX.Direct3D11;
using System.Drawing;

namespace FeralTic.DX11
{
    public static class ExtensionMethods
    {
        public static Viewport Normalize(this Viewport nvp, float cw,float ch)
        {
            Viewport vp = new Viewport();
            vp.Width = nvp.Width * cw;
            vp.Height = nvp.Height * ch;
            vp.MinZ = nvp.MinZ;
            vp.MaxZ = nvp.MaxZ;

            float x = nvp.X / 2.0f + 0.5f;
            float y = 1.0f - (nvp.Y / 2.0f + 0.5f);
            vp.X = (x * cw) - (vp.Width / 2.0f);
            vp.Y = (y * ch) - (vp.Height / 2.0f);

            return vp;
        }
    }
}
