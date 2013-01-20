using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace FeralTic.DX11.Queries
{
    public delegate void DX11QueryableDelegate(DX11RenderContext context);

    public interface IDX11Query
    {
        void Start();
        void Stop();
        void GetData();
    }

    public interface IDX11Queryable
    {
        event DX11QueryableDelegate BeginQuery;
        event DX11QueryableDelegate EndQuery;
    }
}
