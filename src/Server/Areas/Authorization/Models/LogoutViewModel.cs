using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;

namespace Nocturne.Auth.Server.Areas.Authorization.Models
{
    public class LogoutViewModel
    {
        private readonly HttpRequest request;

        public LogoutViewModel(
            HttpRequest request)
        {
            this.request = request;
        }

        public IEnumerable<KeyValuePair<string, StringValues>> RequestParameters
            => request.HasFormContentType ? request.Form : request.Query;
    }
}
