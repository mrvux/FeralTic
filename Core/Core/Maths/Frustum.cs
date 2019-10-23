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

        public Plane Left => this.planes[0];

        public Plane Right => this.planes[1];

        public Plane Top => this.planes[2];

        public Plane Bottom => this.planes[3];

        public Plane Near => this.planes[4];

        public Plane Far => this.planes[5];

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

        private static Vector3 Get3PlanesInterPoint(ref Plane p1, ref Plane p2, ref Plane p3)
        {
            //P = -d1 * N2xN3 / N1.N2xN3 - d2 * N3xN1 / N2.N3xN1 - d3 * N1xN2 / N3.N1xN2 
            Vector3 v =
                -p1.D * Vector3.Cross(p2.Normal, p3.Normal) / Vector3.Dot(p1.Normal, Vector3.Cross(p2.Normal, p3.Normal))
                - p2.D * Vector3.Cross(p3.Normal, p1.Normal) / Vector3.Dot(p2.Normal, Vector3.Cross(p3.Normal, p1.Normal))
                - p3.D * Vector3.Cross(p1.Normal, p2.Normal) / Vector3.Dot(p3.Normal, Vector3.Cross(p1.Normal, p2.Normal));

            return v;
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
