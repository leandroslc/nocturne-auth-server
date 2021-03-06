// Copyright (c) Leandro Silva Luz do Carmo
// SPDX-License-Identifier: GPL-3.0-or-later

using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Nocturne.Auth.Authorization.Requirements;

namespace Nocturne.Auth.Authorization
{
    public sealed class UserAccessControlBuilder
    {
        private readonly IServiceCollection services;

        public UserAccessControlBuilder(IServiceCollection services)
        {
            this.services = services;
        }

        public UserAccessControlBuilder AddPermissionAuthorizationHandler()
        {
            services.AddScoped<IAuthorizationHandler, PermissionAuthorizationHandler>();
            services.AddScoped<IAuthorizationHandler, AnyPermissionAuthorizationHandler>();

            return this;
        }

        public UserAccessControlBuilder AddRoleAuthorizationHandler()
        {
            services.AddScoped<IAuthorizationHandler, AnyRoleAuthorizationHandler>();

            return this;
        }
    }
}
