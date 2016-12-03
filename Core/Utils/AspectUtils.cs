using SlimDX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FeralTic
{
    public enum AspectRatioMode { FitIn, FitOut, FitWidth, FitHeight }

    public static class AspectUtils
    {

        public static Matrix AspectMatrix(Vector2 size, AspectRatioMode mode)
        {
            Vector2 aspect = AspectRatio(size, mode);
            return Matrix.Transformation2D(new Vector2(0.5f, 0.5f), 0.0f, aspect, Vector2.Zero, 0.0f, Vector2.Zero);
        }

        public static Vector2 AspectRatio(Vector2 size, AspectRatioMode mode)
        {

            float w = size.X;
            float h = size.Y;

            float sx, sy;

            switch (mode)
            {
                case AspectRatioMode.FitIn:
                    if (w > h)
                    {
                        sx = 1.0f;
                        sy = w / h;
                    }
                    else
                    {
                        sx = h / w;
                        sy = 1.0f;
                    }
                    break;
                case AspectRatioMode.FitOut:
                    if (w > h)
                    {
                        sx = h / w;
                        sy = 1.0f;
                    }
                    else
                    {
                        sx = 1.0f;
                        sy = w / h;
                    }
                    break;
                case AspectRatioMode.FitWidth:
                    sx = 1.0f;
                    sy = w / h;
                    break;
                default:
                    sy = 1.0f;
                    sx = h / w;
                    break;
            }
            return new Vector2(sx, sy);
        }
    }
}
