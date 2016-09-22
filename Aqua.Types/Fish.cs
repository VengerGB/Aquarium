namespace Aqua.Types
{
    using System;

    [Serializable]
    public class Fish : SerializableBase<Fish>
    {
        public Guid Id;
        public string Name { get; set; }
        public DateTime Born { get; set; }
        public string BirthPlace { get; set; }
        public byte[] PayLoad { get; set; }
        
        public int Food { get; set; }
        public int Health { get; set; }

        [field: NonSerialized]
        public event Action Removed;

        public void Remove()
        {
            if (Removed != null)
            {
                Removed();
            }
        }
    }
}