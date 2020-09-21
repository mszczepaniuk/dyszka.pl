using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ApplicationCore.Extensions
{
    public static class EnumerableExtensions
    {
        private static Random rnd = new Random();

        public static T GetRandom<T>(this IEnumerable<T> enumerable)
        {
            return enumerable.ElementAt(rnd.Next(enumerable.Count()));
        }
    }
}
