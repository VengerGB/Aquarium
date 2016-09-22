namespace Aqua.Core.Behaviour
{
    using Aqua.Core.Interfaces;
    using Aqua.Core.Utils;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Linq;

    public class FishBehaviour : IBehaviour
    {
        public Point? Cursor { get; set; }
        protected PointF[] PreviousDirections = new PointF[5];

        public void Move(AquaObject me, IEnumerable<AquaObject> others, Point? cursor, AquariumProperties properties)
        {
            if (others.Any())
            {
                this.Flocking(me, others, cursor);
            }
            else
            {
                this.Predation(me, others, cursor);
            }

            this.CalculateHappiness(me, properties);
            this.CheckBounds(me);
            this.CheckSpeed(me);
            me.Position = me.Position.Add(me.Direction);
            me.Ticks++;
        }

        private void CalculateHappiness(AquaObject me, AquariumProperties properties)
        {
            if (me.Ticks % 100 == 0 && me.Cleanliness <= 100 && me.Cleanliness >= 0)
            {
                if (properties.Algae < 25)
                {
                    --me.Cleanliness;
                }
                
                if (properties.Algae > 100)
                {
                    ++me.Cleanliness;
                }
            }
        }

        private void Flocking(AquaObject me, IEnumerable<AquaObject> others, Point? cursor)
        {
            if (others.Count() == 1)
            {
                me.Direction = me.Direction.Add(new PointF(1.0f, 1.0f));
            }

            foreach (AquaObject other in others)
            {
                float distance = me.Position.Distance(other.Position);
                if (other != me && !other.Predator)
                {
                    if (distance < me.Space)
                    {
                        // Create space.
                        me.Direction = me.Direction.Add(
                                        new PointF(
                                        (me.Position.X - other.Position.X) * 0.01f,
                                        (me.Position.Y - other.Position.Y) * 0.01f));
                    }
                    else if (distance < me.Sight)
                    {
                        // Flock together.
                        me.Direction = me.Direction.Add(
                                        new PointF(
                                        (other.Position.X - me.Position.X) * 0.001f,
                                        (other.Position.Y - me.Position.Y) * 0.001f));
                    }

                    if (distance < me.Sight)
                    {
                        // Align movement.
                        me.Direction = me.Direction.Add(
                                        new PointF(
                                        other.Direction.X * 0.001f,
                                        other.Direction.Y * 0.001f));
                    }
                }

                if (other.Predator && distance < me.Sight)
                {
                    // Avoid hunters.
                    me.Direction = me.Direction.Add(
                                    new PointF(
                                    me.Position.X - other.Position.X * 0.2f,
                                    me.Position.Y - other.Position.Y * 0.2f));
                }

                if (cursor.HasValue && me.Position.Distance(cursor.Value) < me.Sight)
                {
                    // Avoid cursor.
                    me.Direction = me.Direction.Add(
                                    new PointF(
                                    me.Position.X - cursor.Value.X * 1.0f,
                                    me.Position.Y - cursor.Value.Y * 1.0f));
                }
            }
        }

        private void Predation(AquaObject me, IEnumerable<AquaObject> others, Point? cursor)
        {
            float range = float.MaxValue;
            AquaObject prey = null;
            foreach (var other in others)
            {
                if (!other.Predator)
                {
                    float distance = me.Position.Distance(other.Position);
                    if (distance < me.Sight && distance < range)
                    {
                        range = distance;
                        prey = other;
                    }
                }
            }

            if (prey != null)
            {
                // Move towards closest prey.
                me.Direction = me.Direction.Add(
                                new PointF(
                                prey.Position.X - me.Position.X,
                                prey.Position.Y - me.Position.Y));
            }
        }

        private void CheckBounds(AquaObject me)
        {
            if (me.Position.X < me.Border)
            {
                me.Direction = me.Direction.Add(new PointF(me.Border - me.Position.X, 0));
            }

            if (me.Position.Y < me.Border)
            {
                me.Direction = me.Direction.Add(new PointF(0, me.Border - me.Position.Y));
            }

            if (me.Position.X > me.Boundary.Width - me.Border)
            {
                me.Direction = me.Direction.Add(new PointF(me.Boundary.Width - me.Border - me.Position.X, 0));
            }

            if (me.Position.Y > me.Boundary.Height - me.Border)
            {
                me.Direction = me.Direction.Add(new PointF(0, me.Boundary.Height - me.Border - me.Position.Y));
            }
        }

        private void CheckSpeed(AquaObject me)
        {
            float s;
            if (!me.Predator)
            {
                s = me.Speed;
            }
            else
            {
                s = me.Speed/4f;
            }

            float val = new PointF().Distance(me.Direction);
            if (val > s)
            {
                me.Direction = new PointF(me.Direction.X*s/val, me.Direction.Y*s/val);
            }
        }
    }
}
