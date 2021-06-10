using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Mime;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Nocturne.Auth.Authorization.Configuration;

namespace Nocturne.Auth.Authorization.Services
{
    public class UserAccessControlService
    {
        private readonly AuthorizationSettings settings;
        private readonly IHttpClientFactory clientFactory;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly UserAccessControlCacheService cache;
        private readonly JsonSerializerOptions jsonSerializerOptions;

        public UserAccessControlService(
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

        public async Task<UserAccessControlResponse> GetUserAccessControlAsync(
            UserAccessControlCommand command)
        {
            command.Verify();

            var cachedResponse = await cache.GetAsync(command.UserIdentifier, settings.ClientId);

            if (cachedResponse != null)
            {
                return cachedResponse;
            }

            var access = await RequestUserAccessControlAsync();

            await cache.SetAsync(command.UserIdentifier, settings.ClientId, access);

            return access;
        }

        private async Task<UserAccessControlResponse> RequestUserAccessControlAsync()
        {
            var accessToken = await GetAccessTokenAsync();
            var client = CreateClient(accessToken);

            var response = await client.GetAsync(settings.AccessControlEndpoint);

            if (response.IsSuccessStatusCode is false)
            {
                return UserAccessControlResponse.Empty;
            }

            var content = await response.Content.ReadAsStreamAsync();

            return await JsonSerializer
                .DeserializeAsync<UserAccessControlResponse>(content, jsonSerializerOptions);
        }

        private HttpClient CreateClient(string accessToken)
        {
            var client = clientFactory.CreateClient(Constants.HttpClientName);

            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue(MediaTypeNames.Application.Json));

            client.DefaultRequestHeaders.Authorization
                = new AuthenticationHeaderValue("Bearer", accessToken);

            return client;
        }

        private async Task<string> GetAccessTokenAsync()
        {
            return await settings.GetAccessTokenAsync(httpContextAccessor.HttpContext);
        }
    }
}
