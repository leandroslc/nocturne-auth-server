using System;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.Extensions.DependencyInjection;

namespace Nocturne.Auth.Configuration.Services
{
    public static class LocalizationServices
    {

        private const string LanguagesPath = "Locales";

        public static IServiceCollection AddApplicationLocalization(
            this IServiceCollection services)
        {
            services.AddPortableObjectLocalization(options =>
            {
                options.ResourcesPath = LanguagesPath;
            });

            return services;
        }

        public static IMvcBuilder AddApplicationMvcLocalization(
            this IMvcBuilder builder)
        {
            builder.AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix);

            builder.AddDataAnnotationsLocalization();

            return builder;
        }
    }
}
