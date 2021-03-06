// Copyright (c) Leandro Silva Luz do Carmo
// SPDX-License-Identifier: GPL-3.0-or-later

using Microsoft.AspNetCore.Mvc.DataAnnotations;
using Microsoft.Extensions.DependencyInjection;
using Nocturne.Auth.Core.Web.Validation;

namespace Nocturne.Auth.Configuration.Services
{
    public static class MvcServices
    {
        public static IServiceCollection AddApplicationCustomMvcValidationAttributes(
            this IServiceCollection services)
        {
            services.AddSingleton<IValidationAttributeAdapterProvider, CustomValidationAttributeAdapterProvider>();

            return services;
        }
    }
}
