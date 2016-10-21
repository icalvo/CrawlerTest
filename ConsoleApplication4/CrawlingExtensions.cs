using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;

namespace ConsoleApplication4
{
    public static class CrawlingExtensions
    {

        internal static IEnumerable<string> Crawl(this IEnumerable<Uri> request)
        {
            return
                request
                    .Select(Request)
                    .Where(response => response.IsSuccessStatusCode)
                    .Select(response => response.Content.ReadAsStringAsync().GetAwaiter().GetResult());
        }

        private static HttpResponseMessage Request(Uri arg)
        {
            throw new NotImplementedException();
        }


    }
}