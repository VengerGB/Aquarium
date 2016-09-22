namespace Aqua.Types
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Net;
    using System.Text;

    public class RemotePeer
    {
        private const string NetworkFormatSeparator = "|";
        
        public string Id { get; set; }

        public IPAddress Address { get; set; }

        public int Port { get; set; }

        public Version Version { get; set; }

        public DateTime LastHeardFrom { get; set; }

        public IPAddress ManualNotificationAddress { get; set; }

        public int ManualNotificationPort { get; set; }

        public Dictionary<string, int> PeerProperties = new Dictionary<string, int>();

        public static RemotePeer FromNotification(Stream s)
        {
            using (var memoryStream = new MemoryStream())
            {
                s.CopyTo(memoryStream);
                return FromNotification(memoryStream.ToArray());
            }
        }

        public static RemotePeer FromNotification(byte[] networkData)
        {
            string peerInfo = Encoding.ASCII.GetString(networkData);
            string[] peerData = peerInfo.Split(new[] {NetworkFormatSeparator}, StringSplitOptions.None);

            if (peerData.Length >= 7)
            {
                var peer =  new RemotePeer
                               {
                                   Id = peerData[0] + "|" + peerData[1],
                                   Version = Version.Parse(peerData[2]),
                                   Address = IPAddress.Parse(peerData[3]),
                                   Port = int.Parse(peerData[4]),
                                   LastHeardFrom = DateTime.UtcNow,
                                   ManualNotificationAddress = IPAddress.Parse(peerData[5]),
                                   ManualNotificationPort = int.Parse(peerData[6]),
                               };

                if (peerData.Length == 8)
                {
                    var properties = peerData[7].Split(new[] { '#' }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (var property in properties)
                    {
                        var nameValue = property.Split(new[] { '@' }, StringSplitOptions.RemoveEmptyEntries);
                        peer.PeerProperties[nameValue[0]] = int.Parse(nameValue[1]);
                    }
                }

                return peer;
            }

            return null;
        }

        public byte[] ToNetwork()
        {
            var properties = string.Empty;

            foreach (var key in this.PeerProperties.Keys)
            {
                properties += key + "@" + this.PeerProperties[key] + "#";
            }

            return Encoding.ASCII.GetBytes(
                string.Join(
                    NetworkFormatSeparator,
                    Id,
                    Version,
                    Address,
                    Port,
                    ManualNotificationAddress,
                    ManualNotificationPort,
                    properties));
        }
    }
}