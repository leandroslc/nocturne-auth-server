using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Text.Json;
using System.Threading.Tasks;

namespace Nocturne.Auth.Authorization.Services
{
    public class UserAccessControlCacheService
    {
        private const string SessionKey = "_access-control";

        private readonly IDistributedCache cache;

        public UserAccessControlCacheService(IDistributedCache cache)
        {
            this.cache = cache;
        }

        public async Task SetAsync(UserAccessControlResponse value)
        {
            var data = JsonSerializer.SerializeToUtf8Bytes(value);

            await cache.SetAsync(SessionKey, data);
        }

        public async Task<UserAccessControlResponse> GetAsync()
        {
            var data = await cache.GetAsync(SessionKey);

            if (data is null)
            {
                return null;
            }

            return JsonSerializer.Deserialize<UserAccessControlResponse>(
                new ReadOnlySpan<byte>(data));
        }
    }
}
