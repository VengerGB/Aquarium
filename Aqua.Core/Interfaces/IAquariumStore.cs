namespace Aqua.Core.Interfaces
{
    public interface IAquariumStore
    {
        void Save(IAquarium aquarium);
        IAquarium Load();
    }
}
