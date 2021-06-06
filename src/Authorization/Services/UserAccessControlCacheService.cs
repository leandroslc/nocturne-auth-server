using System;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;

namespace Nocturne.Auth.Authorization.Services
{
    public class UserAccessControlCacheService
    {
        private readonly IDistributedCache cache;

        public UserAccessControlCacheService(IDistributedCache cache)
        {
            this.cache = cache;
        }

        public async Task SetAsync(
            string userIdentifier,
            string clientId,
            UserAccessControlResponse value)
        {
            var key = GetCacheKey(userIdentifier, clientId);

            var data = JsonSerializer.SerializeToUtf8Bytes(value);

            await cache.SetAsync(key, data);
        }

        public async Task<UserAccessControlResponse> GetAsync(
            string userIdentifier,
            string clientId)
        {
            var key = GetCacheKey(userIdentifier, clientId);

            var data = await cache.GetAsync(key);

            if (data is null)
            {
                return null;
            }

            return JsonSerializer.Deserialize<UserAccessControlResponse>(
                new ReadOnlySpan<byte>(data));
        }

        private static string GetCacheKey(string userIdentifier, string clientId)
        {
            const string CacheKeyFormat = "{0}:{1}:access-control";

            return string.Format(CacheKeyFormat, clientId, userIdentifier);
        }
    }
}
