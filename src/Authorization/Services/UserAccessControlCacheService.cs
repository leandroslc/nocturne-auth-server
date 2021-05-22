using Microsoft.AspNetCore.Http;
using System;
using System.Text.Json;

namespace Nocturne.Auth.Authorization.Services
{
    public class UserAccessControlCacheService
    {
        private const string SessionKey = "_access-control";

        private readonly IHttpContextAccessor httpContextAccessor;

        public UserAccessControlCacheService(
            IHttpContextAccessor httpContextAccessor)
        {
            this.httpContextAccessor = httpContextAccessor;
        }

        public void Set(UserAccessControlResponse value)
        {
            var data = JsonSerializer.SerializeToUtf8Bytes(value);

            httpContextAccessor.HttpContext.Session.Set(SessionKey, data);
        }

        public bool TryGet(out UserAccessControlResponse value)
        {
            var session = httpContextAccessor.HttpContext.Session;

            if (session.TryGetValue(SessionKey, out var data) is false)
            {
                value = null;

                return false;
            }

            var item = JsonSerializer.Deserialize<UserAccessControlResponse>(
                new ReadOnlySpan<byte>(data));

            value = item;

            return true;
        }
    }
}
