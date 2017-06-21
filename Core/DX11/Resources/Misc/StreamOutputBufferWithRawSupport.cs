using FeralTic.DX11.Utils;
using SlimDX.Direct3D11;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FeralTic.DX11.Resources.Misc
{
    /// <summary>
    /// A stream output buffer, that also provides a raw buffer view
    /// </summary>
    public class StreamOutputBufferWithRawSupport
    {
        private SlimDX.Direct3D11.Buffer innerBuffer;
        private DX11VertexGeometry vertexGeometry;
        private DX11RawBuffer rawBuffer;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="context">Render context</param>
        /// <param name="vertexSize">Vertex size</param>
        /// <param name="vertexCount">Vertex Count</param>
        /// <param name="inputElements">Input elements</param>
        public StreamOutputBufferWithRawSupport(DX11RenderContext context, int vertexSize, int vertexCount, InputElement[] inputElements)
        {
            this.innerBuffer = BufferHelper.CreateStreamOutBuffer(context, vertexSize, vertexCount);

            //Copy a new Vertex buffer with stream out
            this.vertexGeometry = new DX11VertexGeometry(context);
            vertexGeometry.AssignDrawer(new DX11VertexAutoDrawer());
            vertexGeometry.HasBoundingBox = false;
            vertexGeometry.InputLayout = inputElements;
            vertexGeometry.Topology = PrimitiveTopology.TriangleList;
            vertexGeometry.VertexBuffer = innerBuffer;
            vertexGeometry.VertexSize = vertexSize;
            vertexGeometry.VerticesCount = vertexCount;

            if (context.ComputeShaderSupport)
            {
                this.rawBuffer = new DX11RawBuffer(context, innerBuffer);
            }
        }

        /// <summary>
        /// Gets the Low level Direct3D11 underlying resource
        /// </summary>
        public SlimDX.Direct3D11.Buffer D3DBuffer => this.innerBuffer;

        /// <summary>
        /// Gets a drawable vertex geometry
        /// </summary>
        public DX11VertexGeometry VertexGeometry => this.vertexGeometry;

        /// <summary>
        /// Raw buffer view, that can be used as byteaddress buffer
        /// This can be null in case device does not support compute shaders (that shoudl be rare nowadays except phones)
        /// </summary>
        public DX11RawBuffer RawBuffer => this.rawBuffer;

        /// <summary>
        /// Disposes resources (buffer and view)
        /// </summary>
        public void Dispose()
        {
            if (this.rawBuffer != null)
            {
                this.rawBuffer.SRV.Dispose();
                this.rawBuffer = null;
            }

            if (this.innerBuffer != null)
            {
                this.innerBuffer.Dispose();
                this.innerBuffer = null;
            }

        } 
    }
}
