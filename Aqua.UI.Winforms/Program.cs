namespace Aqua.UI.Winforms
{
    using Aqua.Core;
    using Aqua.Core.Behaviour;
    using Aqua.Core.Interfaces;
    using Aqua.Core.Network;
    using Aqua.Core.Utils;
    using System;
    using System.Windows.Forms;

    internal static class Program
    {
        /// <summary>
        ///     The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main()
        {
            IocContainer.AddMapping<IManualPeerList, ManualPeerList>(true);
            IocContainer.AddMapping<IAquarium, Aquarium>();
            IocContainer.AddMapping<IAquariumStore, LocalAquariumStore>();
            IocContainer.AddMapping<IBehaviour, FishBehaviour>();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new AquariumForm());
        }
    }
}