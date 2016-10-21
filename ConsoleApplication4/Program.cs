using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using static ConsoleApplication4.PagedSources;

namespace ConsoleApplication4
{
    internal static class Program
    {
        private static IEnumerable<dynamic> GetRecords(DbCommand command)
        {
            throw new NotImplementedException();
        }

        internal static void Main(string[] args)
        {
            AllPubs()
            .ConvertToUrls()
            .Crawl()
            .Execute(Store);
        }

        private static void Store(string payload)
        {
            throw new NotImplementedException();
        }
        
        private static IEnumerable<Uri> ConvertToUrls(this IEnumerable<PublicationIdentifier> pubs)
        {
            return pubs
                .Batch(partial => partial.ToAddress().Length > 500)
                .Select(ToUri);
        }

        private static Uri ToUri(this IEnumerable<PublicationIdentifier> ids)
        {
            return new Uri(ids.ToAddress());
        }

        private static string ToAddress(this IEnumerable<PublicationIdentifier> ids)
        {
            return "http://example.com?" + ToQueryString(ids);
        }

        private static string ToQueryString(this IEnumerable<PublicationIdentifier> ids)
        {
            return string.Join("&", ids.Select(x => "DOI=" + x.Doi));
        }


        private static IEnumerable<PublicationIdentifier> AllPubs(int? pubId = null)
        {
            return PagedSource<PublicationIdentifier, int>(
                null,
                id =>
                {
                    var page = PublicationPage(pubId);
                    int? nextId = page.Count == 20 ? page.Last().PublicationId : (int?)null;
                    return PagedResult.Create(page, nextId);
                });
        }

        private static IList<PublicationIdentifier> PublicationPage(int? pubId)
        {
            var sql = "SELECT TOP 20 * FROM IMP_Publication WHERE publicationId=" + pubId;
            return Query(sql, MapToPublicationIdentifier).ToList();
        }


        private static IEnumerable<T> Query<T>(
            string sql,
            Func<dynamic, T> convert)
        {
            using (DbConnection connection = new SqlConnection())
            {
                connection.Open();
                using (DbTransaction transaction = connection.BeginTransaction())
                {
                    DbCommand cmd = connection.CreateCommand();
                    cmd.CommandText = sql;
                    cmd.Transaction = transaction;

                    return GetRecords(cmd)
                        .Select(convert)
                        .ToList();
                }
            }
        }

        private static PublicationIdentifier MapToPublicationIdentifier(dynamic obj)
        {
            return new PublicationIdentifier(obj.Doi);
        }
    }
}
