using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.WebUtilities;

namespace Nocturne.Auth.Core.Shared.Extensions
{
    public static class HttpRequestExtensions
    {
        public static string CreateUrlWithNewQuery(
            this HttpRequest request,
            params (string, string)[] parameters)
        {
            Check.NotNull(request, nameof(request));

            var values = request.Query.ToDictionary(q => q.Key, q => q.Value.ToString());

            if (parameters != null)
            {
                foreach (var parameter in parameters)
                {
                    values.AddOrReplace(parameter.Item1, parameter.Item2);
                }
            }

            return QueryHelpers.AddQueryString(request.Path, values);
        }
    }
}
