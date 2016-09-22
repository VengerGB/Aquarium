namespace Aqua.Core.Network
{
    using Aqua.Core.Interfaces;
    using Aqua.Core.Utils;
    using Aqua.Types;
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Net.Sockets;
    using System.Reflection;
    using System.Threading;

    public class LocalPeer : RemotePeer
    {
        public delegate void FishCallback(Fish f);

        private const int BROADCAST_NOTIFICATION_PORT_NUMBER = 15001;
        
        private readonly TimeSpan NotificationInterval = TimeSpan.FromSeconds(5.0d);

        private readonly ConcurrentDictionary<string, RemotePeer> Peers = new ConcurrentDictionary<string, RemotePeer>();
        private UdpClient broadcast;
        private readonly TcpListener fishListener = new TcpListener(IPAddress.Any, 0);
        private readonly TcpListener manualPeerListener;
        private readonly IManualPeerList manualPeers;

        private bool Stopping;
        private Timer notifyTimer;

        public LocalPeer(string id)
        {
            fishListener.Start();
            this.Id = id;
            this.Address = MyIP();
            this.Port = ((IPEndPoint)fishListener.LocalEndpoint).Port;
            this.Version = Assembly.GetExecutingAssembly().GetName().Version;
            this.ManualNotificationAddress = IPAddress.None;
            this.ManualNotificationPort = -1;

            try
            {
                // If a manual peer list is defined start - otherwise don't start yet another socket.
                this.manualPeers = IocContainer.Get<IManualPeerList>();
                this.manualPeerListener = new TcpListener(IPAddress.Any, 0);
                manualPeerListener.Start();

                this.manualPeers.HostData = new ManualPeer
                                            {
                                                Address = MyIP(),
                                                Port = ((IPEndPoint)this.manualPeerListener.LocalEndpoint).Port,
                                            };

                this.ManualNotificationAddress = this.manualPeers.HostData.Address;
                this.ManualNotificationPort = this.manualPeers.HostData.Port;
            }
            catch (InvalidOperationException)
            {
            }
        }

        public int PeerCount
        {
            get { return Peers.Count; }
        }

        public event FishCallback NewFish;
        public event FishCallback FishSent;

        public void Start(bool broadcastNotifications = true, bool unicastNotifications = true)
        {
            Stopping = false;

            if (broadcastNotifications)
            {
                this.broadcast = new UdpClient();
                this.broadcast.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
                this.broadcast.Client.Bind(new IPEndPoint(IPAddress.Any, BROADCAST_NOTIFICATION_PORT_NUMBER));
                this.broadcast.BeginReceive(this.ReceiveBroadcastNotification, null);
                Trace.WriteLine(string.Format("Listening for new peers on multi-cast Port {0}.", BROADCAST_NOTIFICATION_PORT_NUMBER));
            }

            this.fishListener.BeginAcceptSocket(this.ReceiveFishData, null);
            Trace.WriteLine(string.Format("Listening for new data on IP:{0} and Port {1}.", Address, Port));

            if (this.manualPeerListener != null && unicastNotifications)
            {
                this.manualPeerListener.BeginAcceptSocket(this.ReceiveUnicastNotification, null);
                Trace.WriteLine(string.Format("Listening for manual peer requests data on IP:{0} and Port {1}.", Address, ((IPEndPoint)this.manualPeerListener.LocalEndpoint).Port));
            }

            this.notifyTimer = new Timer(NotifyTimer, null, TimeSpan.Zero, NotificationInterval);
        }

        public void Stop()
        {
            Stopping = true;
            this.notifyTimer.Change(int.MaxValue, int.MaxValue);

            try
            {
                if (this.broadcast != null)
                {
                    this.broadcast.Close();
                    Trace.WriteLine("Stopped listening for peers.");
                }
            }
            catch
            {
                /* don't care */
            }

            try
            {
                this.fishListener.Stop();
                Trace.WriteLine("Stopped listening for data.");
            }
            catch
            {
                /* don't care */
            }

            try
            {
                this.manualPeerListener.Stop();
                Trace.WriteLine("Stopped sending manual details to peers.");
            }
            catch
            {
                /* don't care */
            }
        }

        private void ReceiveUnicastNotification(IAsyncResult ar)
        {
            if (Stopping)
            {
                return;
            }

            Trace.WriteLine("Received notification from manual peer.");
            Socket socket = this.manualPeerListener.EndAcceptSocket(ar);

            using (var s = new NetworkStream(socket))
            {
                RemotePeer peerNofication = FromNotification(s);
                this.AddOrUpdatePeer(peerNofication);
                this.manualPeers.AddOrUpdate(new ManualPeer { Address = peerNofication.ManualNotificationAddress, Port = peerNofication.ManualNotificationPort });
            }

            this.manualPeerListener.BeginAcceptSocket(this.ReceiveUnicastNotification, null);
        }

        private void ReceiveBroadcastNotification(IAsyncResult ar)
        {
            if (Stopping)
            {
                return;
            }

            Trace.WriteLine("Received notification from broadcast peer.");
            var ip = new IPEndPoint(IPAddress.Any, BROADCAST_NOTIFICATION_PORT_NUMBER);

            byte[] notification = broadcast.EndReceive(ar, ref ip);
            RemotePeer peerNofication = FromNotification(notification);
            AddOrUpdatePeer(peerNofication);
            broadcast.BeginReceive(this.ReceiveBroadcastNotification, null);
        }

        private void ReceiveFishData(IAsyncResult ar)
        {
            if (Stopping)
            {
                return;
            }

            Socket socket = fishListener.EndAcceptSocket(ar);

            using (var s = new NetworkStream(socket))
            {
                Fish fish = Fish.FromStream(s);
                FireNewFish(fish);
            }

            fishListener.BeginAcceptSocket(this.ReceiveFishData, null);
        }

        private void FireNewFish(Fish fish)
        {
            if (NewFish != null)
            {
                NewFish(fish);
            }
        }

        private void FireFishSent(Fish fish)
        {
            if (FishSent != null)
            {
                FishSent(fish);
            }
        }

        public void SendFish(Fish f)
        {
            if (Stopping)
            {
                return;
            }

            if (!Peers.Values.Any())
            {
                Trace.WriteLine("Nowhere to send " + f.Name);
                return;
            }

            // Send to the most recent peer we heard from. 
            RemotePeer peer = Peers.Values.OrderByDescending(v => v.LastHeardFrom)
                                          .FirstOrDefault(p => p.PeerProperties["Occupants"] < 200 && p.PeerProperties["Algae"] < 128);

            if (peer == null)
            {
                Trace.WriteLine("No suitable peers to send {0}", f.Name);
            }

            if (peer != null && f != null)
            {
                try
                {
                    using (var client = new TcpClient())
                    {
                        client.Connect(new IPEndPoint(peer.Address, peer.Port));
                        using (NetworkStream stream = client.GetStream())
                        {
                            f.ToStream(stream);
                        }
                    }
                    FireFishSent(f);
                }
                catch (Exception e)
                {
                    Trace.WriteLine("Exception on send.");
                    Trace.WriteLine(e);
                }
            }
        }        

        private void AddOrUpdatePeer(RemotePeer peerNofication)
        {
            if (peerNofication == null)
            {
                Trace.Write("Couldn't parse notification.");
            }
            else if (peerNofication.Id != Id)
            {
                Peers.AddOrUpdate(peerNofication.Id, key =>
                    {
                        Trace.WriteLine(
                            string.Format(
                                "New data from {0} received: {1} ",
                                peerNofication.Address, 
                                peerNofication.Id));

                        return peerNofication;
                    },
                    (key, oldvalue) =>
                    {
                        Trace.WriteLine(string.Format("Update from {0} received: {1} ", peerNofication.Address, peerNofication.Id));
                        return peerNofication;
                    });

                foreach(var property in peerNofication.PeerProperties)
                {
                    Trace.WriteLine(string.Format("{0}:{1}", property.Key, property.Value));
                }
            }
        }

        public void NotifyTimer(object state)
        {
            RemoveOldPeers();
            Notify();
        }

        private void RemoveOldPeers()
        {
            // Remove old peers which are no longer pinging here...
            List<RemotePeer> oldPeers = Peers.Where(p => (DateTime.UtcNow - p.Value.LastHeardFrom).TotalSeconds > 20)
                .Select(peer => peer.Value)
                .ToList();

            if (oldPeers.Any())
            {
                RemotePeer oldPeer;
                oldPeers.ForEach(peer => Peers.TryRemove(peer.Id, out oldPeer));
            }
        }

        public void Notify()
        {
            if (Stopping)
            {
                return;
            }

            byte[] notificationBytes = this.ToNetwork();

            if (this.broadcast != null)
            {
                broadcast.Send(notificationBytes, notificationBytes.Length, new IPEndPoint(IPAddress.Broadcast, BROADCAST_NOTIFICATION_PORT_NUMBER));
                Console.WriteLine("Broadcast: {0}", Id);
            }

            if (this.manualPeers == null || !this.manualPeers.Peers.Any())
            {
                return;
            }

            using (var memory = new MemoryStream(notificationBytes))
            {
                foreach (var peer in this.manualPeers.Peers)
                {
                    using (var client = new TcpClient())
                    {
                        try
                        {
                            client.Connect(new IPEndPoint(peer.Address, peer.Port));
                            using (NetworkStream stream = client.GetStream())
                            {
                                memory.Position = 0;
                                memory.CopyTo(stream);
                                Console.WriteLine("Sent: {0}", Id);
                            }
                        }
                        catch (SocketException)
                        {
                            Trace.WriteLine(string.Format("Unable to contact {0} on {1}", peer.Address, peer.Port));
                        }
                    }
                }
            }
        }

        public IPAddress MyIP()
        {
            IPHostEntry host;
            host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (IPAddress ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip;
                }
            }
            return IPAddress.Any;
        }
    }
}