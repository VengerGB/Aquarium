
namespace Aqua.Core.Utils
{
    using System;
    using System.Drawing;

    public static class PointExtensions
    {
        public static PointF Offset(this PointF point, float offset)
        {
            return new PointF(point.X + offset, point.Y + offset);
        }

        public static float Distance(this PointF point1, PointF point2)
        {
            double hypotenuse = Math.Pow(point1.X - point2.X, 2) + Math.Pow(point1.Y - point2.Y, 2);
            return (float)Math.Sqrt(hypotenuse);
        }

        public static PointF Midpoint(this PointF point1, PointF point2)
        {
            return new PointF(point1.X + ((point2.X -point1.X) / 2), point1.Y + ((point2.Y - point1.Y) / 2));
        }

        public static PointF Clone(this PointF point)
        {
            return new PointF(point.X, point.Y);
        }

        public static PointF Subtract(this PointF point1, PointF point2)
        {
            return new PointF(point1.X - point2.X, point1.Y - point2.Y);
        }

        public static PointF Add(this PointF point1, PointF point2)
        {
            return new PointF(point1.X + point2.X, point1.Y + point2.Y);
        }
    }
}
