using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;

using SlimDX.Direct3D11;

namespace FeralTic.DX11
{
    /// <summary>
    /// Simple Null device (Used for shader parsing mainly)
    /// </summary>
    public static class NullRenderDevice
    {
        private static Device device;

        public static Device Device
        {
            get
            {
                if (device == null)
                {
                    device = new Device(DriverType.Null);
                }
                return device;
            }
        }
    }
}
