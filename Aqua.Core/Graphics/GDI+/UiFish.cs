namespace Aqua.Core.UI
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.IO;
    using System.Linq;
    using Aqua.Core.Utils;
    using Aqua.Types;
    using Aqua.Core.Behaviour;
    using Aqua.Core.Interfaces;

    public class UiFish : AquaObject
    {
        public RectangleF Bounds { get; set; }
        public School School { get; set; }
        public Fish Fish { get; private set; }
        public bool Active { get; set; }
        public bool Selected { get; set; }
        public bool Leaving { get; set; }
        
        private Color FillColor { get; set; }
        private Color OutlineColor { get; set; }
        
        private UIFishData data = UIFishData.FromStream(EmbeddedResourceReader.ReadEmbeddedData("Graphics.GDI_.Fishes.Fish1.dat"));
        private PointF[] PreviousDirections = new PointF[5]; 
        private IBehaviour behaviour;
        private AquariumProperties aquariumProperties;

        public UiFish(Fish fish, PointF startingPosition, AquariumProperties tank)
        {
            this.Border = 100f;
            this.Sight = 300f;
            this.Space = 25f;

            this.behaviour = IocContainer.Get<IBehaviour>();

            this.Predator = false;
            this.Boundary = new Size(tank.Width, tank.Height);
            this.aquariumProperties = tank;

            this.Fish = fish;
            this.Fish.Removed += () => { Active = false; };
            this.Active = true;
            this.Leaving = false;
            this.Cleanliness = 100;

            using (var br = new BinaryReader(new MemoryStream(this.Fish.PayLoad.Reverse().ToArray())))
            {
                FillColor = Color.FromArgb(br.ReadByte(), br.ReadByte(), br.ReadByte());
                OutlineColor = Color.FromArgb(br.ReadByte(), br.ReadByte(), br.ReadByte());
                BodySize = (br.ReadByte() % 10) + 1;
                Speed = (br.ReadByte() % 8) + 1;
                Predator = (br.ReadByte() % 10) <= 2 ? true : false; 
            }

            this.Position = startingPosition;
        }

        public static UiFish RandomPosition(Fish fish, AquariumProperties tank)
        {
            return new UiFish(
                fish,
                new PointF(
                    random.Next(tank.Width),
                    random.Next(tank.Height)),
                    tank);
        }

        public static UiFish Born(Fish fish, UiFish parent1, UiFish parent2, AquariumProperties tank)
        {
            return new UiFish(
                fish,
                parent1.Position.Midpoint(parent2.Position),
                tank);
        }

        public void Move(IEnumerable<UiFish> fish, Point? avoid)
        {
            this.PreviousDirections[this.Ticks % 5] = this.Direction.Clone();
            this.behaviour.Move(this, fish, avoid, this.aquariumProperties);
        }

        public PointF[] GetFishBody(PointF[] bodyPoints, PointF position, Facing direction)
        {
            var transform = new Matrix();
            Scale(BodySize * 0.5f, bodyPoints, direction == Facing.Left);
            transform.Translate(position.X, position.Y, MatrixOrder.Append);
            transform.TransformPoints(bodyPoints);
            return bodyPoints;
        }

        private void Scale(float scaleFactor, PointF[] body, bool left)
        {
            var transform = new Matrix();
            transform.Scale(left ? -scaleFactor : scaleFactor, scaleFactor);
            transform.TransformPoints(body);
        }

        public PointF[] GetFishTail(PointF[] tailPoints, PointF position, Facing direction)
        {
            var transform = new Matrix();
            Scale(Speed * 0.5f, tailPoints, direction == Facing.Left);
            transform.Translate(position.X, position.Y, MatrixOrder.Append);
            transform.TransformPoints(tailPoints);
            return tailPoints;
        }

        public PointF GetFishEye(PointF eye, PointF position, Facing direction)
        {
            var transform = new Matrix();
            var eyeArray = eye.ArrayOfOne();
            Scale(BodySize * 0.5f, eyeArray, direction == Facing.Left);
            transform.Translate(position.X, position.Y, MatrixOrder.Append);
            transform.TransformPoints(eyeArray);
            return eyeArray.Single();
        }

        public void Draw(Graphics g)
        {
            if (!this.Active)
            {
                return;
            }

            var fishColor = this.FillColor;
            if (this.Selected)
            {
                fishColor = Color.FromArgb((int)Math.Abs(Math.Sin(this.Ticks) * 255), fishColor.R, fishColor.G, fishColor.B);
            }

            var movedFish = new UIFishData();
            movedFish.Body = new List<PointF>(this.GetFishBody(this.data.Body.ToArray(), this.Position, this.FacingDirection));
            movedFish.Tail = new List<PointF>(this.GetFishTail(this.data.Tail.ToArray(), movedFish.Body.Last(), this.FacingDirection));
            movedFish.Eye = this.GetFishEye(this.data.Eye, this.Position, this.FacingDirection);

            var path = new GraphicsPath();
            var body = movedFish.Body.ToArray();
            path.AddLines(body);
            g.FillPath(new SolidBrush(fishColor), path);
            path.AddLines(movedFish.Tail.ToArray());
            g.FillPath(new SolidBrush(fishColor), path);

            g.FillEllipse(this.Predator ? Brushes.Red : Brushes.White, new RectangleF(movedFish.Eye.X - (this.BodySize), movedFish.Eye.Y - (this.BodySize), this.BodySize * 2, this.BodySize * 2));
            g.FillEllipse(Brushes.Black, new RectangleF(movedFish.Eye.X - 1.0f, movedFish.Eye.Y - 1.0f, 3.5f, 3.5f));

            g.DrawString(
                Fish.Name,
                new Font("Segoe UI Light", 10, FontStyle.Regular),
                new SolidBrush(Color.White),
                Position.Offset(10.0f));

            this.Bounds = path.GetBounds();
        }

        private Facing FacingDirection
        {
            get
            {
                var left = this.PreviousDirections.Where(d => d != null).Count(d => d.X < 0);
                return left > 3 ? Facing.Left : Facing.Right;
            }
        }
    }
}