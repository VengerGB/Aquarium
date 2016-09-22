namespace Aqua.Types
{
    using System;
    using System.IO;
    using System.Runtime.Serialization.Formatters.Binary;

    [Serializable]
    public class SerializableBase<T>
    {
        public static T FromStream(Stream s)
        {
            if (s == null)
            {
                throw new ArgumentException("Stream 's' was null.");
            }

            var formatter = new BinaryFormatter();
            return (T) formatter.Deserialize(s);
        }

        public void ToStream(Stream s)
        {
            var formatter = new BinaryFormatter();
            formatter.Serialize(s, this);
        }
    }
}