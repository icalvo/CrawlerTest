using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace ConsoleApplication4
{
    internal static class EnumerableExtensions
    {
        [SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures",
             Justification = "Functionality is about creating an IEnumerable of IEnumerable")]
        [SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope",
             Justification = "Creating a new IEnumerator, should be disposed of outside")]
        public static IEnumerable<IEnumerable<T>> Batch<T>(
            this IEnumerable<T> source,
            Func<IReadOnlyCollection<T>, bool> cutCondition)
        {
            return new BatchEnumerable<T>(source, cutCondition);
        }

        internal static void Execute<T>(this IEnumerable<T> items, Action<T> action)
        {
            foreach (var item in items)
            {
                action(item);
            }
        }
    }
}