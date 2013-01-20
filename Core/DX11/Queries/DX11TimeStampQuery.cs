using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SlimDX.Direct3D11;


namespace FeralTic.DX11.Queries
{
    public class DX11TimeStampQuery : IDX11Query
    {
        private Query tstart;
        private Query tend;
        private Query tsdis;

        public float Elapsed;

        public bool hasrun = false;

        private DX11RenderContext context;

        public DX11TimeStampQuery(DX11RenderContext context)
        {
            this.context = context;
            QueryDescription qd = new QueryDescription();
            qd.Type = QueryType.Timestamp;
            qd.Flags = QueryFlags.None;

            this.tstart = new Query(context.Device, qd);
            this.tend = new Query(context.Device,qd);

            QueryDescription qdd = new QueryDescription();
            qdd.Type = QueryType.TimestampDisjoint;
            qdd.Flags = QueryFlags.None;

            this.tsdis = new Query(context.Device, qdd);
        }

        public void Start()
        {
            context.CurrentDeviceContext.Begin(this.tsdis);
            context.CurrentDeviceContext.End(this.tstart);
            //this.hasend = false;
        }

        public void Stop()
        {
            context.CurrentDeviceContext.End(this.tend);
            context.CurrentDeviceContext.End(this.tsdis);
            this.hasrun = true;
        }

        public void GetData()
        {
            if (this.hasrun == false) { return; }

            DeviceContext ctx = this.context.CurrentDeviceContext;

            while (!ctx.IsDataAvailable(this.tstart)) { }
            while (!ctx.IsDataAvailable(this.tend)) { }
            while (!ctx.IsDataAvailable(this.tsdis)) { }

            Int64 startTime = ctx.GetData<Int64>(this.tstart);
            Int64 endTime = ctx.GetData<Int64>(this.tend);
            TimestampQueryData data = ctx.GetData<TimestampQueryData>(this.tsdis);

            float time = 0.0f;
            if (data.IsDisjointed == false)
            {
                Int64 delta = endTime - startTime;
                float frequency = (float)data.Frequency;
                time = ((float)delta / frequency) * 1000.0f;

                this.Elapsed = time;
            }   
        }
    }
}
