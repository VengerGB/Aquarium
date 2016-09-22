namespace Aqua.Core.UI
{
    using System.Collections.Generic;
    using System.Drawing;
    using System.Linq;
    using Aqua.Core.Behaviour;
    using Aqua.Core.Interfaces;
    using Aqua.Core.Utils;

    public class UiAquarium
    {
        private readonly List<UiFish> allFish = new List<UiFish>();
        private readonly List<UiFish> newFish = new List<UiFish>();
        private readonly IAquarium peerAquarium;
        private readonly List<School> schools = new List<School>();
        private Point? mousePosition;
        
        public UiAquarium(Size tankSize)
        {
            var store = IocContainer.Get<IAquariumStore>();
            this.peerAquarium = store.Load();

            this.peerAquarium.Properties.Width = tankSize.Width;
            this.peerAquarium.Properties.Height = tankSize.Height;

            this.peerAquarium.FishArrived += f => newFish.Add(UiFish.RandomPosition(f, this.peerAquarium.Properties));
            this.peerAquarium.FishBorn += (f, p1, p2) =>
                                     {
                                         UiFish parent1 = allFish.First(uiFish => uiFish.Fish == p1);
                                         UiFish parent2 = allFish.First(uiFish => uiFish.Fish == p2);
                                         newFish.Add(UiFish.Born(f, parent1, parent2, this.peerAquarium.Properties));
                                     };

            // Read fish from storage.
            this.peerAquarium.ReadFish.ToList().ForEach(f => SortOrAddSchool(UiFish.RandomPosition(f, this.peerAquarium.Properties)));
            this.schools.Add(new School { Name = "Killers" });
        }

        public int FishCount
        {
            get { return allFish.Count; }
        }

        public int PeerCount
        {
            get { return peerAquarium.PeerCount; }
        }

        public int SchoolCount
        {
            get { return this.schools.Count; }
        }

        public int Dirtiness
        {
            get { return this.peerAquarium.Properties.Algae; }
        }

        public int Leaving
        {
            get { return allFish.Count(f => f.Leaving); }
        }

        public IEnumerable<UiFish> SelectedFish
        {
            get { return allFish.Where(f => f.Selected); }
        }

        public void Process()
        {
            newFish.ForEach(f => SortOrAddSchool(f));
            newFish.Clear();

            allFish.ForEach((f) => { f.Leaving = f.Cleanliness == 0 || allFish.Count(otherf => otherf.Active) - allFish.Count(otherf => otherf.Leaving) > 30; });

            schools.ForEach(s => s.Tidy());
            allFish.RemoveAll(f => !f.Active && f.Leaving);
            schools.RemoveAll(s => s.Empty);
            schools.ForEach(s => s.Move(this.mousePosition));
            this.peerAquarium.Tick();

            allFish.Where(f => f.Leaving).Take(1).ToList().ForEach(f => peerAquarium.Send(f.Fish));
        }

        public void Draw(Graphics g)
        {
            foreach (School school in schools)
            {
                school.Draw(g);
            }
        }

        public void Save()
        {
            peerAquarium.SaveState();
        }

        public void Quit()
        {
            peerAquarium.Shutdown();
        }

        public void SelectFishAt(Point location, bool multiSelect)
        {
            UiFish foundFish = allFish.FirstOrDefault(f => f.Bounds.Contains(location));
            if (foundFish != null)
            {
                bool foundFishSelected = foundFish.Selected;

                if (!multiSelect)
                {
                    this.UnselectAll();
                }

                foundFish.Selected = !foundFishSelected;
            }
            else
            {
                this.UnselectAll();
            }
        }

        public void SelectFishInside(Rectangle rect, bool multiSelect)
        {
            var foundFish = allFish.Where(f => rect.Contains(Rectangle.Round(f.Bounds))).ToList();
            if (foundFish.Any())
            {
                if (!multiSelect)
                {
                    this.UnselectAll();
                }

                foundFish.ForEach(f => f.Selected = !f.Selected);
            }
            else
            {
                this.UnselectAll();
            }
        }

        public void BreedSelected()
        {
            IEnumerable<UiFish> foundFish = allFish.Where(f => f.Selected);
            if (foundFish.Count() >= 2)
            {
                peerAquarium.Breed(foundFish.First().Fish, foundFish.Last().Fish);
            }
        }

        public void SendSelected()
        {
            List<UiFish> foundFish = allFish.Where(f => f.Selected).ToList();
            foundFish.ForEach(f => peerAquarium.Send(f.Fish));
        }

        public void UnselectAll()
        {
            List<UiFish> foundFish = allFish.Where(f => f.Selected).ToList();
            foundFish.ForEach(f => f.Selected = false);
        }

        public void AvoidPosition(Point? p)
        {
            this.mousePosition = p;
        }

        private void SortOrAddSchool(UiFish newFish)
        {
            School school = null;

            if (newFish.Predator)
            {
                school = schools.FirstOrDefault(s => s.Name == "Killers");
            }
            else
            {
                school = schools.FirstOrDefault(s => s.Name == newFish.Fish.BirthPlace);
            }

            if (school == null)
            {
                school = new School {Name = newFish.Fish.BirthPlace};
                schools.Add(school);
            }

            school.Add(newFish);
            allFish.Add(newFish);
        }

        public void SelectAll()
        {
            this.allFish.ForEach(f => f.Selected = true);
        }

        public void Deactivate()
        {
            this.allFish.Where(f => f.Selected).ToList().ForEach(f => f.Active = false);
        }

        public void Clean()
        {
            if (this.peerAquarium.Properties.Algae >= 10)
            {
                this.peerAquarium.Properties.Algae -= 10;
            }
            else
            {
                this.peerAquarium.Properties.Algae = 0;
            }
        }
    }
}