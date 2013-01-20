using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FeralTic.DX11.Resources
{
    public interface IInstancedDrawer
    {
        int InstanceCount { get; set; }
    }
}
