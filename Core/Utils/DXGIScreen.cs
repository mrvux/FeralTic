using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SlimDX.DXGI;

namespace FeralTic.Utils
{
    public class DXGIScreen
    {
        public DXGIScreen()
        {
            this.Adapter = null;
            this.AdapterId = -1;
            this.Monitor = null;
            this.MonitorId = -1;
        }

        public int AdapterId { get; set; }
        public Adapter1 Adapter { get; set; }
        public int MonitorId { get; set; }
        public Output Monitor { get; set; }
    }
}
