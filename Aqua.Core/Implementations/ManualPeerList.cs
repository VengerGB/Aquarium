
namespace Aqua.Core.Network
{
    using Aqua.Core.Interfaces;
    using Aqua.Types;
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Text;
    using System.Threading.Tasks;

    public class ManualPeerList : IManualPeerList
    {
        private readonly ConcurrentDictionary<IPAddress, ManualPeer> manualPeers = new ConcurrentDictionary<IPAddress, ManualPeer>();

        public void AddOrUpdate(ManualPeer peer)
        {
            manualPeers[peer.Address] = peer;
        }

        public void Remove(ManualPeer peer)
        {
            ManualPeer outPeer;
            manualPeers.TryRemove(peer.Address, out outPeer);
        }

        public IEnumerable<ManualPeer> Peers
        {
            get
            {
                return manualPeers.Values.ToList();
            }
        }

        public ManualPeer HostData {get;set;}
    }
}
