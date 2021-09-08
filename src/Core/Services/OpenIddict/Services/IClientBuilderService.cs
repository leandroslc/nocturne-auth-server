namespace Nocturne.Auth.Core.Services.OpenIddict.Services
{
    public interface IClientBuilderService
    {
        string GenerateClientId();

        string GenerateClientSecret();
    }
}
