// Copyright (c) Leandro Silva Luz do Carmo
// SPDX-License-Identifier: GPL-3.0-or-later

using System.Globalization;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nocturne.Auth.Configuration.Options;

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
            var supportedCultures = GetSupportedCultures().ToList();

            services.AddRequestLocalization(options =>
            {
                options.SupportedCultures = supportedCultures;
                options.SupportedUICultures = supportedCultures;
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

        private static IEnumerable<CultureInfo> GetSupportedCultures()
        {
            var localizationFiles = Directory.GetFiles(LanguagesPath);

            foreach (var file in localizationFiles)
            {
                yield return new CultureInfo(Path.GetFileNameWithoutExtension(file));
            }

            yield return new CultureInfo("en");
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
