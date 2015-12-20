using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FeralTic.Utils
{
    /// <summary>
    /// Extra magic numbers that are not available in SlimDX
    /// </summary>
    public static class MagicNumberUtils
    {
        public const int FeatureLevel11_1 = 45312;

        public static double[] ToDoubleArray(this SlimDX.Color4 color)
        {
            return new double[] { color.Red, color.Green, color.Blue, color.Alpha };
        }

        public static double[] WhiteDefault()
        {
            return new double[] { 1.0,1.0,1.0,1.0 };
        }
    }
}
