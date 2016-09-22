namespace Aqua.Core
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Threading;
    using Aqua.Types;
    using Aqua.Core.Interfaces;
    using Aqua.Core.Utils;
    using Aqua.Core.Network;

    public class Aquarium : IAquarium
    {
        private static List<Fish> Fish = new List<Fish>();
        private readonly ReaderWriterLock Lock = new ReaderWriterLock();
        private readonly LocalPeer localPeer;
        private readonly IAquariumStore store;
        private ulong tickCount;
        
        public AquariumProperties Properties { get; private set; }
        public event Action<Fish> FishArrived;
        public event Action<Fish, Fish, Fish> FishBorn;
        public event Action<Fish> FishRemoved;

        public Aquarium(string id, int numberOfFish) : this(id)
        {
            Lock.AcquireWriterLock(TimeSpan.FromSeconds(10.0d));
            Fish.AddRange(FishGenerator.Create(numberOfFish, this.Id));
            Lock.ReleaseWriterLock();
        }

        public Aquarium(string id, IEnumerable<Fish> fish) : this(id)
        {
            Lock.AcquireWriterLock(TimeSpan.FromSeconds(10.0d));
            Fish.AddRange(fish);
            Lock.ReleaseWriterLock();

            // No fish?  Have some for free.
            if (!Fish.Any())
            {
                Lock.AcquireWriterLock(TimeSpan.FromSeconds(10.0d));
                Fish.AddRange(FishGenerator.Create(2, this.Id));
                Lock.ReleaseWriterLock();
            }
        }

        public Aquarium(string id)
        {
            // if no id defined - generate one.
            if (string.IsNullOrEmpty(id))
            {
                this.Id = Environment.MachineName + "|" + Guid.NewGuid();
            }
            else
            {
                this.Id = id;
            }

            localPeer = new LocalPeer(this.Id);
            localPeer.NewFish += NewFish;
            localPeer.FishSent += FishSent;
            Trace.WriteLine("Starting Aquarium Version " + localPeer.Version);
            localPeer.Start();

            this.Properties = new AquariumProperties(this.localPeer.PeerProperties);
            this.store = IocContainer.Get<IAquariumStore>();
        }

        public int FishCount
        {
            get { return Fish.Count; }
        }

        public int PeerCount
        {
            get { return localPeer.PeerCount; }
        }
        
        public string Id { get; set; }

        public IEnumerable<Fish> ReadFish
        {
            get
            {
                Lock.AcquireReaderLock(TimeSpan.FromSeconds(10.0d));
                foreach (Fish fish in Fish)
                {
                    yield return fish;
                }
                Lock.ReleaseReaderLock();
            }
        }

        public void SendFishAway_Test()
        {
            localPeer.SendFish(Fish.Random());
        }

        public void Tick()
        {
            if (tickCount % 50 == 0 && this.Properties.Algae < byte.MaxValue)
            {
                this.Properties.Algae += 1;
            }

            this.Properties.Occupants = this.FishCount;

            this.tickCount++;
        }

        public void Send(Fish fish)
        {
            Lock.AcquireWriterLock(TimeSpan.FromSeconds(10.0d));
            localPeer.SendFish(fish);
            Lock.ReleaseWriterLock();
        }

        private void NewFish(Fish fish)
        {
            Trace.WriteLine("Received fish called: " + fish.Name);
            Lock.AcquireWriterLock(TimeSpan.FromSeconds(10.0d));
            Fish.Add(fish);
            Lock.ReleaseWriterLock();

            if (FishArrived != null)
            {
                FishArrived(fish);
            }
        }

        private void FishSent(Fish fish)
        {
            Trace.WriteLine("Sent fish called: " + fish.Name);
            Lock.AcquireWriterLock(TimeSpan.FromSeconds(10.0d));
            fish.Remove();
            Fish.Remove(fish);

            Lock.ReleaseWriterLock();

            if (FishRemoved != null)
            {
                FishRemoved(fish);
            }
        }

        public void Breed(Fish parent1, Fish parent2)
        {
            Fish newFish = FishGenerator.Breed(parent1, parent2, this.Id);
            Trace.WriteLine("Fish born called: " + newFish.Name);
            Lock.AcquireWriterLock(TimeSpan.FromSeconds(10.0d));
            Fish.Add(newFish);
            Lock.ReleaseWriterLock();

            if (FishBorn != null)
            {
                FishBorn(newFish, parent1, parent2);
            }
        }

        public void SaveState()
        {
            this.store.Save(this);
        }

        public void Shutdown()
        {
            Trace.WriteLine("Shutting down and saving state.");
            localPeer.Stop();
            this.SaveState();
        }
    }
}