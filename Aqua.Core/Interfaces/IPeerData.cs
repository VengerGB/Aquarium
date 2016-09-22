namespace Aqua.Core.Interfaces
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public interface IPeerData
    {
        Dictionary<string, string> PeerData
        {
            get;
        }
    }
}
