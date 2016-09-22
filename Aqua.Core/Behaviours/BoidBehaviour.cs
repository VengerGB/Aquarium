namespace Aqua.Core.Behaviour
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Linq;
    using Aqua.Core.Utils;
    using Aqua.Core.Interfaces;

    public class BoidBehaviour
    {
        protected float Border = 100f;
        protected float Sight = 300f;
        protected float Space = 25f;
        protected static readonly Random random = new Random();
        protected Size Boundary;
        public PointF Direction;
        public PointF Position;
        public Point? Avoid;
        public bool Predator;
        public float Speed { get; set; }
        public long Ticks;

        public int BodySize { get; set; }

        public void Move(IEnumerable<BoidBehaviour> boids, Point? avoid)
        {
            this.Ticks++;
            this.Avoid = avoid;

            if (boids.Any())
            {
                this.Flocking(boids);
            }
            else
            {
                this.Predation(boids);
            }

            CheckBounds();
            CheckSpeed();
            Position.X += Direction.X;
            Position.Y += Direction.Y;
        }

        private void Flocking(IEnumerable<BoidBehaviour> boids)
        {
            if (boids.Count() == 1)
            {
                Direction.X += 1;
                Direction.Y += 1;
            }

            foreach (BoidBehaviour boid in boids)
            {
                float distance = Position.Distance(boid.Position);
                if (boid != this && !boid.Predator)
                {
                    if (distance < Space)
                    {
                        // Create space.
                        Direction.X += (Position.X - boid.Position.X) * 0.01f;
                        Direction.Y += (Position.Y - boid.Position.Y) * 0.01f;
                    }
                    else if (distance < Sight)
                    {
                        // Flock together.
                        Direction.X += (boid.Position.X - Position.X)*0.001f;
                        Direction.Y += (boid.Position.Y - Position.Y)*0.001f;
                    }

                    if (distance < Sight)
                    {
                        // Align movement.
                        Direction.X += boid.Direction.X*0.001f;
                        Direction.Y += boid.Direction.Y*0.001f;
                    }
                }

                if (boid.Predator && distance < Sight)
                {
                    // Avoid hunters.
                    Direction.X += Position.X - boid.Position.X * 0.2f;
                    Direction.Y += Position.Y - boid.Position.Y * 0.2f;
                }

                if (this.Avoid.HasValue && this.Position.Distance(this.Avoid.Value) < Sight)
                {
                    // Avoid hunters.
                    Direction.X += Position.X - this.Avoid.Value.X * 1.0f;
                    Direction.Y += Position.Y - this.Avoid.Value.Y * 1.0f;
                }
            }
        }

        private void Predation(IEnumerable<BoidBehaviour> boids)
        {
            float range = float.MaxValue;
            BoidBehaviour prey = null;
            foreach (BoidBehaviour boid in boids)
            {
                if (!boid.Predator)
                {
                    float distance = Position.Distance(boid.Position);
                    if (distance < Sight && distance < range)
                    {
                        range = distance;
                        prey = boid;
                    }
                }
            }

            if (prey != null)
            {
                // Move towards closest prey.
                Direction.X += prey.Position.X - Position.X;
                Direction.Y += prey.Position.Y - Position.Y;
            }
        }

        private void CheckBounds()
        {
            if (Position.X < Border)
            {
                Direction.X += Border - Position.X;
            }

            if (Position.Y < Border)
            {
                Direction.Y += Border - Position.Y;
            }

            if (Position.X > Boundary.Width - Border)
            {
                Direction.X += Boundary.Width - Border - Position.X;
            }

            if (Position.Y > Boundary.Height - Border)
            {
                Direction.Y += Boundary.Height - Border - Position.Y;
            }
        }

        private void CheckSpeed()
        {
            float s;
            if (!Predator)
            {
                s = this.Speed;
            }
            else
            {
                s = this.Speed/4f;
            }

            float val = new PointF().Distance(Direction);
            if (val > s)
            {
                Direction.X = Direction.X*s/val;
                Direction.Y = Direction.Y*s/val;
            }
        }
    }
}