namespace Aqua.Core.UI
{
    using Aqua.Types;
    using Aqua.Core.Utils;
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.Linq;

    [Serializable]
    public class UIFishData : SerializableBase<UIFishData>, ICloneable
    {
        public PointF Eye;
        public List<PointF> Body;
        public List<PointF> Tail;

        public UIFishData()
        {
            this.Body = new List<PointF>();
            this.Tail = new List<PointF>();
            this.Eye = new PointF();
        }

        public UIFishData TranslateToPoint(PointF point)
        {
            float scale = 0.25f;
            var cloned = (UIFishData)this.Clone();
            
            cloned.Body.Add(cloned.Eye);
            var transformedBody = TransformPointsToPoint(point, cloned.Body.ToArray(), scale);
            cloned.Eye = transformedBody.Last();
            transformedBody.Remove(cloned.Eye);
            cloned.Body = transformedBody;
            cloned.Tail = TransformPointsToPoint(point, cloned.Tail.ToArray(), scale);

            return cloned;
        }

        public UIFishData TranslateToOrigin()
        {
            return this.TranslateToPoint(new PointF());
        }

        private static List<PointF> TransformPointsToPoint(PointF origin, PointF[] points, float scale)
        {
            var path = new GraphicsPath();
            path.AddLines(points);
            var boundary = path.GetBounds();

            var transform = new Matrix();
            transform.Translate(origin.X + -(boundary.Left + (boundary.Width/2)), origin.Y + -(boundary.Top + (boundary.Height/2)), MatrixOrder.Append);
            transform.Scale(scale, scale, MatrixOrder.Append);

            transform.TransformPoints(points);

            return new List<PointF>(points);
        }

        public object Clone()
        {
            return new UIFishData
                       {
                           Body = this.Body.Select(p => p.Clone()).ToList(),
                           Tail = this.Tail.Select(p => p.Clone()).ToList(),
                           Eye = this.Eye
                       };
        }
    }
}
