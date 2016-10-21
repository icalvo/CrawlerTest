using System;
using System.Collections.Generic;

namespace ConsoleApplication4
{
    public static class PagedSources
    {
        public class PagedResult<T, TId> where TId : struct
        {
            public IList<T> Items { get; }
            public TId? NextId { get; }

            public PagedResult(IList<T> items, TId? nextId)
            {
                Items = items;
                NextId = nextId;
            }
        }

        public static class PagedResult
        {
            public static PagedResult<T, TId> Create<T, TId>(IList<T> items, TId? nextId)
                where TId : struct
            {
                return new PagedResult<T, TId>(items, nextId);
            }
        }

        public static IEnumerable<T> PagedSource<T, TId>(
            TId? initial,
            Func<TId, PagedResult<T, TId>> readFunc
        )
            where TId : struct
        {
            var pubId = initial;
            do
            {
                var result = readFunc(pubId.Value);
                var page = result.Items;
                pubId = result.NextId;
                foreach (var id in page)
                {
                    yield return id;
                }
            }
            while (pubId.HasValue);
        }
    }
}