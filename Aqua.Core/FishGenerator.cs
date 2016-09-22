namespace Aqua.Core
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Aqua.Types;
    using Aqua.Core.Utils;

    public static class FishGenerator
    {
        public static List<string> Names = new List<string>();
        public static Random randomizer = new Random();
        public static int MutateRate = 1;

        static FishGenerator()
        {
            string rawNames = EmbeddedResourceReader.ReadTextResource("Names.txt");
            foreach (string name in rawNames.Split('\n'))
            {
                if (string.IsNullOrWhiteSpace(name))
                {
                    continue;
                }

                string[] parts = name.Split(new[] {' '}, StringSplitOptions.RemoveEmptyEntries);
                Names.Add(char.ToUpper(parts[0][0]) + parts[0].Substring(1).ToLower());
            }
        }

        public static IEnumerable<Fish> Create(int numberOfFish, string sourceId)
        {
            var fish = new List<Fish>();

            for (int i = 0; i < numberOfFish; i++)
            {
                fish.Add(new Fish
                       {
                           Id = Guid.NewGuid(),
                           Name = Names.Random(),
                           BirthPlace = sourceId,
                           Born = DateTime.UtcNow,
                           PayLoad = Encoding.ASCII.GetBytes(sourceId),
                           Health = 100,
                           Food = 1000,
                       });
            }

            return fish;
        }

        public static Fish Breed(Fish fish1, Fish fish2, string sourceId)
        {
            return new Fish
                   {
                       Id = Guid.NewGuid(),
                       Name = Names.Random(),
                       Born = DateTime.UtcNow,
                       BirthPlace = sourceId,
                       Health = 100,
                       Food = 1000,
                       PayLoad = fish1.PayLoad.Zip(fish2.PayLoad, (first, second) =>
                                                                  {
                                                                      if (MutateRate == randomizer.Next(10))
                                                                      {
                                                                          return (byte) randomizer.Next(byte.MaxValue);
                                                                      }

                                                                      return randomizer.Next(1) == 1 ? first : second;
                                                                  })
                           .ToArray(),
                   };
        }
    }
}