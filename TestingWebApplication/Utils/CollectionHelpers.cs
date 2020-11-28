namespace TestingWebApplication.Utils
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Утилитарный класс для вспомогательных методов для коллекций.
    /// </summary>
    public static class CollectionHelpers
    {
        /// <summary>
        /// Выполняет перемешивание элементов коллекции.
        /// </summary>
        /// <typeparam name="T">Тип элементов коллекции.</typeparam>
        /// <param name="collection">Начальная коллекция.</param>
        /// <param name="rnd">Рандомайзер для перемешивания.</param>
        /// <returns>Перемешанная коллекция.</returns>
        public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> collection, Random rnd)
        {
            return collection.OrderBy(e => rnd.Next());
        }
    }
}
