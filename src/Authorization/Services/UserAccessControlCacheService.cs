using System;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;
using Nocturne.Auth.Authorization.Configuration;

namespace Nocturne.Auth.Authorization.Services
{
    public class UserAccessControlCacheService
    {
        private readonly IDistributedCache cache;
        private readonly AuthorizationSettings settings;

        public UserAccessControlCacheService(
            IDistributedCache cache,
            AuthorizationSettings settings)
        {
            this.cache = cache;
            this.settings = settings;
        }

        public async Task SetAsync(
            string userIdentifier,
            string clientId,
            UserAccessControlResponse value)
        {
            var key = GetCacheKey(userIdentifier, clientId);

            var data = JsonSerializer.SerializeToUtf8Bytes(value);

            var options = GetEntryOptions();

            await cache.SetAsync(key, data, options);
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

        private DistributedCacheEntryOptions GetEntryOptions()
        {
            return new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = settings.CacheExpirationTime,
            };
        }

        private static string GetCacheKey(string userIdentifier, string clientId)
        {
            const string CacheKeyFormat = "{0}:{1}:access-control";

            return string.Format(CacheKeyFormat, clientId, userIdentifier);
        }
    }
}
