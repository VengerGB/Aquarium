namespace Aqua.Core.Interfaces
{
    using Aqua.Types;
    using System.Collections.Generic;

    public interface IManualPeerList
    {
        void AddOrUpdate(ManualPeer peer);
        void Remove(ManualPeer peer);
        IEnumerable<ManualPeer> Peers {get;}
        ManualPeer HostData { get; set; }
    }
}
