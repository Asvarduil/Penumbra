using System;

namespace Asvarduil.Penumbra.Services
{
    public static class StringExtensions
    {
        /// <summary>
        /// Attempts to parse the source string to a given type of enumeration.
        /// </summary>
        /// <typeparam name="T">Enumeration to attempt to cast to</typeparam>
        /// <param name="source">Source string</param>
        /// <returns>Enumeration if successful.</returns>
        public static T ToEnum<T>(this string source)
        {
            T result = (T)Enum.Parse(typeof(T), source);
            return result;
        }
    }
}
