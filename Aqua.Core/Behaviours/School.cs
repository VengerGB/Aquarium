namespace Aqua.Core.Behaviour
{
    using System.Linq;
    using Aqua.Core.UI;
    using System.Collections.Generic;
    using System.Drawing;

    public class School
    {
        private readonly List<UiFish> fish = new List<UiFish>();
        public string Name { get; set; }

        public bool Empty
        {
            get { return !fish.Any(); }
        }

        public void Move(Point? avoid)
        {
            fish.ForEach(f => f.Move(fish, avoid));
        }

        public void Draw(Graphics g)
        {
            for (int i = fish.Count - 1; i >= 0; --i)
            {
                fish.ElementAt(i).Draw(g);
            }
        }

        public void Add(UiFish fish)
        {
            this.fish.Add(fish);
            fish.School = this;
        }

        public void Tidy()
        {
            fish.RemoveAll(f => !f.Active && f.Leaving);
        }
    }
}