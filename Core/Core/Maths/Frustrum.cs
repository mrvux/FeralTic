using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;

using SlimDX;

namespace FeralTic.Core.Maths
{
    public class Frustum
    {
        public Plane[] planes;

        public Frustum()
        {
            planes = new Plane[6];
            this.Initialize(Matrix.Identity, Matrix.Identity);
        }

        public Frustum(Matrix viewMatrix, Matrix projectionMatrix)
        {
            planes = new Plane[6];
            this.Initialize(viewMatrix, projectionMatrix);
        }

        public void Initialize(Matrix viewMatrix, Matrix projectionMatrix)
        {
            Matrix viewProjection = Matrix.Multiply(viewMatrix, projectionMatrix);

            //left plane
            planes[0] = new Plane(-viewProjection.M14 + viewProjection.M11,
                                -viewProjection.M24 + viewProjection.M21,
                                -viewProjection.M34 + viewProjection.M31,
                                -viewProjection.M44 + viewProjection.M41);

            //right plane
            planes[1] = new Plane(-viewProjection.M14 - viewProjection.M11,
                                -viewProjection.M24 - viewProjection.M21,
                                -viewProjection.M34 - viewProjection.M31,
                                -viewProjection.M44 - viewProjection.M41);

            //top plane
            planes[2] = new Plane(-viewProjection.M14 - viewProjection.M12,
                                -viewProjection.M24 - viewProjection.M22,
                                -viewProjection.M34 - viewProjection.M32,
                                -viewProjection.M44 - viewProjection.M42);

            //bottom plane
            planes[3] = new Plane(-viewProjection.M14 + viewProjection.M12,
                                -viewProjection.M24 + viewProjection.M22,
                                -viewProjection.M34 + viewProjection.M32,
                                -viewProjection.M44 + viewProjection.M42);

            //near plane
            planes[4] = new Plane(-viewProjection.M13,
                                -viewProjection.M23,
                                -viewProjection.M33,
                                -viewProjection.M43);

            //far plane
            planes[5] = new Plane(-viewProjection.M14 + viewProjection.M13,
                                -viewProjection.M24 + viewProjection.M23,
                                -viewProjection.M34 + viewProjection.M33,
                                -viewProjection.M44 + viewProjection.M43);

            for (int i = 0; i < 6; i++) planes[i].Normalize();
        }

        public bool Contains(BoundingBox boundingBox, Matrix worldMatrix)
        {
            boundingBox.Maximum = Vector3.TransformCoordinate(boundingBox.Maximum, worldMatrix);
            boundingBox.Minimum = Vector3.TransformCoordinate(boundingBox.Minimum, worldMatrix);

            BoundingBox box;
            box.Minimum.X = boundingBox.Minimum.X < boundingBox.Maximum.X ? boundingBox.Minimum.X : boundingBox.Maximum.X;
            box.Minimum.Y = boundingBox.Minimum.Y < boundingBox.Maximum.Y ? boundingBox.Minimum.Y : boundingBox.Maximum.Y;
            box.Minimum.Z = boundingBox.Minimum.Z < boundingBox.Maximum.Z ? boundingBox.Minimum.Z : boundingBox.Maximum.Z;

            box.Maximum.X = boundingBox.Minimum.X > boundingBox.Maximum.X ? boundingBox.Minimum.X : boundingBox.Maximum.X;
            box.Maximum.Y = boundingBox.Minimum.Y > boundingBox.Maximum.Y ? boundingBox.Minimum.Y : boundingBox.Maximum.Y;
            box.Maximum.Z = boundingBox.Minimum.Z > boundingBox.Maximum.Z ? boundingBox.Minimum.Z : boundingBox.Maximum.Z;



            foreach (Plane plane in planes)
            {
                switch (Plane.Intersects(plane, box))
                {
                    case PlaneIntersectionType.Front:
                        return false;
                    case PlaneIntersectionType.Intersecting:
                        break;
                }
            }
            return true;
        }
    }
}
