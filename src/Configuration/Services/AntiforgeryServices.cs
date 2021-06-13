using Microsoft.Extensions.DependencyInjection;
using Nocturne.Auth.Core.Web;

namespace Nocturne.Auth.Configuration.Services
{
    public static class AntiforgeryServices
    {
        public static IServiceCollection AddApplicationAntiforgery(
            this IServiceCollection services,
            string applicationIdentifier)
        {
            return services.AddAntiforgery(options =>
            {
                options.Cookie.Name = CookieNameGenerator.Compute("antiforgery", applicationIdentifier);
            });
        }
    }
}
