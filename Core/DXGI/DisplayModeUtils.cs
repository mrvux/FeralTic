using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SlimDX.DXGI;

namespace FeralTic.DXGI
{
    public class DisplayModeUtils
    {
        public static ModeDescription GetClosestDisplayMode(Output o, Device device, SlimDX.Rational rational)
        {
            var pt = new System.Drawing.Point(o.Description.DesktopBounds.Left + 1, o.Description.DesktopBounds.Top + 1);

            Screen scr = Screen.FromPoint(pt);

            ModeDescription md = new ModeDescription(scr.Bounds.Width, scr.Bounds.Height, rational, Format.R8G8B8A8_UNorm);
            ModeDescription closest;
            o.GetClosestMatchingMode(device, md, out closest);
            return closest;
        }
    }
}
