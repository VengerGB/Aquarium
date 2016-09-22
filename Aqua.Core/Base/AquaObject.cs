
namespace Aqua.Core
{
    using System;
    using System.Drawing;

    public abstract class AquaObject
    {
        public float Border { get; set; }
        public float Sight { get; set; }
        public float Space { get; set; }

        public PointF Direction { get; set; }
        public PointF Position { get; set; }
        public Size Boundary { get; set; }

        public bool Predator { get; set; }
        public float Speed { get; set; }
        public int BodySize { get; set; }
        public int Cleanliness { get; set; }

        public long Ticks;
        protected static readonly Random random = new Random();
    }
}
