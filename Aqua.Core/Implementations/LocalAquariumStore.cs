namespace Aqua.Core
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Text;
    using Aqua.Types;
    using Aqua.Core.Interfaces;
    using Aqua.Core.Network;

    public class LocalAquariumStore : IAquariumStore
    {
        // Warning the schema value cannot change in length.
        public const string FishStoreSchema = "FSS|0000.0000.0000.0001|"; 
        public const string FishStoreFileName = "SimAquarium.Store.dat";
        public const string ApplicationName = "Aqua";

        public static bool Exists
        {
            get { return File.Exists(DataPath); }
        }

        private static string DataPath
        {
            get
            {
                return Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                    ApplicationName,
                    FishStoreFileName);
            }
        }

        public void Save(IAquarium aquarium)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(DataPath));

            var fishes = aquarium.ReadFish.ToList();

            using (FileStream fs = File.Create(DataPath))
            {
                byte[] schema = UnicodeEncoding.Unicode.GetBytes(FishStoreSchema);
                byte[] id = UnicodeEncoding.Unicode.GetBytes(aquarium.Id);
                byte[] idLength = BitConverter.GetBytes(id.Length);
                fs.Write(schema, 0, schema.Length);
                fs.Write(idLength, 0, idLength.Length);
                fs.Write(id, 0, id.Length);
                
                byte[] size = BitConverter.GetBytes(fishes.Count);
                fs.Write(size, 0, size.Length);
                foreach (Fish fish in fishes)
                {
                    fish.ToStream(fs);
                }
            }
        }

        public IAquarium Load()
        {
            if (!LocalAquariumStore.Exists)
            {
                Trace.WriteLine("No save file found.");
                return new Aquarium(string.Empty, 2);
            }

            var loadedAquarium = LoadAquarium();
            if (loadedAquarium == null)
            {
                // If we were unable to load the current schema we need to start again.
                return new Aquarium(string.Empty, 2);
            }

            return loadedAquarium;
        }

        private static IAquarium LoadAquarium()
        {
            IAquarium returnAquarium = null;
            
            using (FileStream fs = File.OpenRead(DataPath))
            {
                Version version;
                if (TryGetSchemaData(fs, out version))
                {
                    var loadedFishes = new List<Fish>();

                    // Aquarium Details.
                    var idLengthBytes = new byte[4];
                    fs.Read(idLengthBytes, 0, 4);
                    int idLength = BitConverter.ToInt32(idLengthBytes, 0);
                    var idBytes = new byte[idLength];
                    fs.Read(idBytes, 0, idLength);
                    var id = UnicodeEncoding.Unicode.GetString(idBytes);

                    var size = new byte[4];
                    fs.Read(size, 0, 4);
                    int numberOfFish = BitConverter.ToInt32(size, 0);
                    for (int i = 0; i < numberOfFish; i++)
                    {
                        Fish f = Fish.FromStream(fs);
                        if (f.PayLoad == null)
                        {
                            f.PayLoad = Encoding.ASCII.GetBytes(f.BirthPlace);
                        }

                        loadedFishes.Add(f);
                    }

                    returnAquarium = new Aquarium(id, loadedFishes);
                }
            }

            return returnAquarium;
        }

        private static bool TryGetSchemaData(FileStream fs, out Version version)
        {
            bool succeeded = false;
            version = new Version();
            var localSchemaData = UnicodeEncoding.Unicode.GetBytes(FishStoreSchema);
            var storedSchemaData = new byte[localSchemaData.Length];
            var bytesRead = fs.Read(storedSchemaData, 0, storedSchemaData.Length);
            string storedSchemaString = UnicodeEncoding.Unicode.GetString(storedSchemaData);

            if (bytesRead == localSchemaData.Length)
            {
                var schemaComponents = storedSchemaString.Split(new []{"|"}, StringSplitOptions.RemoveEmptyEntries);
                if (schemaComponents.Length == 2)
                {
                    if (schemaComponents[0] == "FSS")
                    {
                        var versionComponents = schemaComponents[1].Split(new[] { "." }, StringSplitOptions.RemoveEmptyEntries);
                        if (versionComponents.Length == 4)
                        {
                            version = new Version(int.Parse(versionComponents[0]), int.Parse(versionComponents[1]), int.Parse(versionComponents[2]), int.Parse(versionComponents[3]));
                            succeeded = true;
                        }
                    }
                }
            }

            return succeeded;
        }
    }
}
