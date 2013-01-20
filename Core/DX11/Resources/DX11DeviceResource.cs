using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SlimDX.Direct3D11;

namespace FeralTic.DX11.Resources
{
    /// <summary>
    /// Per device DirectX 11 resource (does not include geometry)
    /// </summary>
    /// <typeparam name="T">Resource Type</typeparam>
    public abstract class DX11DeviceResource<T> : IDX11Resource, IDisposable where T : Resource
    {
        public DX11DeviceResource()
        {
            this.Resource = null;
            this.SRV = null;
        }

        /// <summary>
        /// Resource owns a shader resource view
        /// </summary>
        public virtual ShaderResourceView SRV { get; protected set; }

        /// <summary>
        /// Real resource
        /// </summary>
        public virtual T Resource { get; protected set; }

        /// <summary>
        /// Here we could implement basic dispose, but resource my not be owned by this class,
        /// wo we leave up to the subclass to decide
        /// </summary>
        public abstract void Dispose();
    }
}
