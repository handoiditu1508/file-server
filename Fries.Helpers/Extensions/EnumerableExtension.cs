﻿namespace Fries.Helpers.Extensions
{
    public static class EnumerableExtension
    {
        /// <summary>
        /// Shuffle and return a new list from old one.
        /// </summary>
        /// <typeparam name="T">Type of list.</typeparam>
        /// <param name="enumeration">List to shuffle.</param>
        /// <returns>Shuffled list.</returns>
        public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> enumeration)
        {
            var result = enumeration.ToList();

            var random = new Random();

            var currentIndex = result.Count;

            // While there remain elements to shuffle.
            while (currentIndex != 0)
            {
                // Pick a remaining element.
                var randomIndex = random.Next(currentIndex);
                currentIndex--;

                // And swap it with the current element.
                (result[randomIndex], result[currentIndex]) = (result[currentIndex], result[randomIndex]);
            }

            return result;
        }

        /// <summary>
        /// Performs the specified action on each element of the IEnumerable.
        /// </summary>
        /// <typeparam name="T">The type of objects to enumerate.</typeparam>
        /// <param name="enumeration">List of T.</param>
        /// <param name="action">The Action delegate to perform on each element of the IEnumerable.</param>
        public static void ForEach<T>(this IEnumerable<T> enumeration, Action<T> action)
        {
            foreach (var item in enumeration)
            {
                action(item);
            }
        }

        public static bool IsNullOrEmpty<T>(this IEnumerable<T> enumeration)
        {
            if (enumeration == null || !enumeration.Any())
                return true;
            return false;
        }
    }
}
