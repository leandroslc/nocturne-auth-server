using System.Collections.Generic;
using System.Threading.Tasks;

namespace Nocturne.Auth.Core.Shared.Extensions
{
    public static class IAsyncEnumerableExtensions
    {
        public static async Task<List<TResult>> ToListAsync<TResult>(
            this IAsyncEnumerable<TResult> source)
        {
            Check.NotNull(source, nameof(source));

            var list = new List<TResult>();

            await foreach (var item in source)
            {
                list.Add(item);
            }

            return list;
        }
    }
}
