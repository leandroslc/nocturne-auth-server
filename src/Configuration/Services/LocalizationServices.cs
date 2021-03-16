using System.Globalization;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nocturne.Auth.Configuration.Models;

namespace Nocturne.Auth.Configuration.Services
{
    public static class LocalizationServices
    {
        private const string LanguagesPath = "Locales";

        public static IServiceCollection AddApplicationLocalization(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddPortableObjectLocalization(options =>
            {
                options.ResourcesPath = LanguagesPath;
            });

            var localizationOptions = GetOptions(configuration);

            SetDefaultCulture(localizationOptions);

            return services;
        }

        public static IMvcBuilder AddApplicationMvcLocalization(
            this IMvcBuilder builder)
        {
            builder.AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix);

            builder.AddDataAnnotationsLocalization();

            return builder;
        }

        private static void SetDefaultCulture(LocalizationOptions options)
        {
            var culture = CultureInfo.GetCultureInfo(options.DefaultCulture);

            CultureInfo.DefaultThreadCurrentCulture = culture;
            CultureInfo.DefaultThreadCurrentUICulture = culture;
        }

        private static LocalizationOptions GetOptions(IConfiguration configuration)
        {
            var options = new LocalizationOptions();

            configuration
                .GetSection(LocalizationOptions.Section)
                .Bind(options);

            return options;
        }
    }
}
