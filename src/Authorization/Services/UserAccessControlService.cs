using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Mime;
using System.Text.Json;
using System.Threading.Tasks;
using Nocturne.Auth.Authorization.Configuration;

namespace Nocturne.Auth.Authorization.Services
{
    public class UserAccessControlService
    {
        private readonly AuthorizationSettings settings;
        private readonly IHttpClientFactory clientFactory;
        private readonly UserAccessControlCacheService cache;
        private readonly JsonSerializerOptions jsonSerializerOptions;

        public UserAccessControlService(
            AuthorizationSettings settings,
            IHttpClientFactory clientFactory,
            UserAccessControlCacheService cache)
        {
            this.settings = settings;
            this.clientFactory = clientFactory;
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

            var access = await RequestUserAccessControlAsync(command);

            await cache.SetAsync(command.UserIdentifier, settings.ClientId, access);

            return access;
        }

        private async Task<UserAccessControlResponse> RequestUserAccessControlAsync(
            UserAccessControlCommand command)
        {
            var client = CreateClient(command.AccessToken);

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
    }
}
