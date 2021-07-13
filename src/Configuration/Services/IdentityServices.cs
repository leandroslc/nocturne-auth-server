using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nocturne.Auth.Core.Services.Identity;
using Nocturne.Auth.Core.Web;

namespace Nocturne.Auth.Configuration.Services
{
    public static class IdentityServices
    {
        public static IServiceCollection AddApplicationIdentity(
            this IServiceCollection services,
            IConfiguration configuration,
            string applicationIdentifier)
        {
            services
                .AddIdentity<ApplicationUser, ApplicationRole>(options =>
                {
                    BindIdentityOptions(options, configuration);
                })
                .AddEntityFrameworkStores<ApplicationIdentityDbContext>()
                .AddDefaultTokenProviders()
                .AddErrorDescriber<CustomIdentityErrorDescriber>();

            services.ConfigureApplicationCookie(options =>
            {
                options.Cookie.Name = CookieNameGenerator.Compute("auth", applicationIdentifier);
                options.LoginPath = "/account/signin";
                options.ReturnUrlParameter = "returnUrl";
            });

            services.AddScoped<IUserClaimsPrincipalFactory<ApplicationUser>, CustomUserClaimsPrincipalFactory>();
            services.AddScoped<SignInManager<ApplicationUser>, CustomSignInManager>();

            return services;
        }

        public static IServiceCollection AddApplicationIdentityServicesOnly(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services
                .AddIdentityCore<ApplicationUser>(options =>
                {
                    BindIdentityOptions(options, configuration);
                })
                .AddEntityFrameworkStores<ApplicationIdentityDbContext>()
                .AddDefaultTokenProviders();

            return services;
        }

        private static void BindIdentityOptions(
            IdentityOptions options,
            IConfiguration configuration)
        {
            configuration.GetSection("Identity").Bind(options);
        }
    }
}
