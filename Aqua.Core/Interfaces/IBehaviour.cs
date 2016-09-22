namespace Aqua.Core.Interfaces
{
    using Aqua.Core.Behaviour;
    using System.Collections.Generic;
    using System.Drawing;

    public interface IBehaviour
    {
        void Move(AquaObject me, IEnumerable<AquaObject> others, Point? cursor, AquariumProperties properties);
    }
}
