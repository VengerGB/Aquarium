namespace Aqua.Core.Utils

{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public static class IEnumerableExtensions
    {
        private static readonly Random random = new Random();

        public static T Random<T>(this IEnumerable<T> source)
        {
            T returnVal = default(T);

            if (source != null && source.Any())
            {
                return source.ElementAt(random.Next(source.Count()));
            }

            return returnVal;
        }

        public static IEnumerable<T> EnumerableOfOne<T>(this T item)
        {
            return new[] {item}.AsEnumerable();
        }

        public static T[] ArrayOfOne<T>(this T item)
        {
            return new[] { item };
        }
    }
}