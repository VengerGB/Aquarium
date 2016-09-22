namespace NetworkingTest
{
    using System;
    using System.Diagnostics;
    using Aqua.Core;
    using Aqua.Core.Interfaces;
    using Aqua.Core.Network;
    using Aqua.Core.Utils;
    using Aqua.Core.Behaviour;

    internal class Program
    {
        private static void Main(string[] args)
        {
            var clp = new CommandLineParser(args);

            IocContainer.AddMapping<IAquarium, Aquarium>();
            IocContainer.AddMapping<IAquariumStore, LocalAquariumStore>();
            IocContainer.AddMapping<IBehaviour, FishBehaviour>();

            Trace.Listeners.Add(new ConsoleTraceListener());

            var a = new Aquarium(string.Empty, 50);
            a.Properties.Algae = 0;
            a.Properties.Occupants = 0;

            while (Console.ReadKey().KeyChar != 'x')
            {
                Console.WriteLine("Sending fish.");
                a.SendFishAway_Test();
            }

            a.Shutdown();
        }
    }
}