namespace Aqua.Core.Interfaces
{
    using System.Collections.Generic;
    using Aqua.Types;
    using System;

    public interface IAquarium
    {
        void Tick();

        void Send(Fish fish);
        void Breed(Fish parent1, Fish parent2);
        
        event Action<Fish> FishArrived;
        event Action<Fish, Fish, Fish> FishBorn;
        
        int FishCount { get; }
        int PeerCount { get; }
        
        IEnumerable<Fish> ReadFish { get; }

        AquariumProperties Properties { get; }

        void SaveState();
        void Shutdown();

        string Id { get; }
    }
}
