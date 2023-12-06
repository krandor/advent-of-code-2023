namespace Fertilizer.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public static class ListExtensions
    {
        public static IEnumerable<TSource> Map<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> selectorFunction, Func<TSource, IEnumerable<TSource>> getChildrenFunction)
        {
            // Add what we have to the stack
            var flattenedList = source.Where(selectorFunction);

            // Go through the input enumerable looking for children,
            // and add those if we have them
            foreach (var element in source)
            {
                flattenedList = flattenedList.Concat(getChildrenFunction(element).Map(selectorFunction, getChildrenFunction));
            }

            return flattenedList;
        }
    }
}