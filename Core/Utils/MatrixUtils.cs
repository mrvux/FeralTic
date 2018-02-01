using SlimDX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FeralTic
{
    public static class MatrixUtils
    {
        public static Matrix AsTextureTransform(this Matrix m)
        {
            m = Matrix.Translation(-0.5f, -0.5f, 0.0f) * m;
            m *= Matrix.Translation(0.5f, 0.5f, 0.0f);
            return m;
        }
    }
}
