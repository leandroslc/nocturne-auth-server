using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Mime;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Nocturne.Auth.Authorization.Configuration;

namespace Nocturne.Auth.Authorization.Services
{
    public class AccessControlService
    {
        private readonly AuthorizationSettings settings;
        private readonly IHttpClientFactory clientFactory;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly UserAccessControlCacheService cache;
        private readonly JsonSerializerOptions jsonSerializerOptions;

        public AccessControlService(
            AuthorizationSettings settings,
            IHttpClientFactory clientFactory,
            IHttpContextAccessor httpContextAccessor,
            UserAccessControlCacheService cache)
        {
            this.settings = settings;
            this.clientFactory = clientFactory;
            this.httpContextAccessor = httpContextAccessor;
            this.cache = cache;

            jsonSerializerOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };
        }

        public async Task<UserAccessControlResponse> GetUserAccessControlAsync()
        {
            var cachedResponse = await cache.GetAsync();

            if (cachedResponse != null)
            {
                return cachedResponse;
            }

            var access = await GetUserAccessControlInternalAsync();

            await cache.SetAsync(access);

            return access;
        }

        private async Task<UserAccessControlResponse> GetUserAccessControlInternalAsync()
        {
            var client = CreateClient();

            await AddAuthenticationAsync(client);

            var response = await client.GetAsync(settings.AccessControlEndpoint);

            if (response.IsSuccessStatusCode is false)
            {
                return UserAccessControlResponse.Empty;
            }

            var content = await response.Content.ReadAsStreamAsync();

            return await JsonSerializer
                .DeserializeAsync<UserAccessControlResponse>(content, jsonSerializerOptions);
        }

        private HttpClient CreateClient()
        {
            var client = clientFactory.CreateClient();

            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue(MediaTypeNames.Application.Json));

            return client;
        }

        private async Task AddAuthenticationAsync(HttpClient client)
        {
            var accessToken = await httpContextAccessor
                .HttpContext.GetTokenAsync("access_token");

            client.DefaultRequestHeaders.Authorization
                = new AuthenticationHeaderValue("Bearer", accessToken);
        }
    }
}
