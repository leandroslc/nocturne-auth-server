using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nocturne.Auth.Core.Services.Identity;
using OpenIddictConstants = OpenIddict.Abstractions.OpenIddictConstants;

namespace Nocturne.Auth.Configuration.Services
{
    public static class IdentityServices
    {
        public static IServiceCollection AddApplicationIdentity(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services
                .AddIdentity<ApplicationUser, ApplicationRole>(options =>
                {
                    configuration.GetSection("Identity").Bind(options);
                })
                .AddEntityFrameworkStores<ApplicationIdentityDbContext>()
                .AddDefaultTokenProviders();

            services.ConfigureApplicationCookie(options =>
            {
                options.Cookie.Name = "auth";
                options.LoginPath = "/account/signin";
                options.ReturnUrlParameter = "returnUrl";
            });

            // Configures Identity to use the same claim types as OpenIddict
            services.Configure<IdentityOptions>(options =>
            {
                options.ClaimsIdentity.UserNameClaimType = OpenIddictConstants.Claims.Name;
                options.ClaimsIdentity.UserIdClaimType = OpenIddictConstants.Claims.Subject;
                options.ClaimsIdentity.RoleClaimType = OpenIddictConstants.Claims.Role;
            });

            services.AddScoped<IUserClaimsPrincipalFactory<ApplicationUser>, CustomUserClaimsPrincipalFactory>();

            return services;
        }

        public static IServiceCollection AddApplicationIdentityServicesOnly(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services
                .AddIdentityCore<ApplicationUser>(options =>
                {
                    configuration.GetSection("Identity").Bind(options);
                })
                .AddEntityFrameworkStores<ApplicationIdentityDbContext>()
                .AddDefaultTokenProviders();

            return services;
        }
    }
}
