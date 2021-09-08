// Copyright (c) Leandro Silva Luz do Carmo
// SPDX-License-Identifier: GPL-3.0-or-later

using Microsoft.Extensions.DependencyInjection;

namespace Nocturne.Auth.Authorization
{
    public static class ServiceCollectionExtensions
    {
        public static UserAccessControlBuilder AddAccessControlService(
            this IServiceCollection services)
        {
            return new UserAccessControlBuilder(services);
        }
    }
}
