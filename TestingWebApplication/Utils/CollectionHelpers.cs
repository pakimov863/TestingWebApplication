namespace TestingWebApplication.Utils
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public static class CollectionHelpers
    {
        public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> collection, Random rnd)
        {
            return collection.OrderBy(e => rnd.Next());
        }
    }
}
