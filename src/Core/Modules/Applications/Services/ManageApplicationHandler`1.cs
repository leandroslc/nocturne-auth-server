// Copyright (c) Leandro Silva Luz do Carmo
// SPDX-License-Identifier: GPL-3.0-or-later

using System.Threading.Tasks;
using Microsoft.Extensions.Localization;
using Nocturne.Auth.Core.Services.OpenIddict.Managers;
using Nocturne.Auth.Core.Services.OpenIddict.Services;
using Nocturne.Auth.Core.Shared.Extensions;
using OpenIddict.Abstractions;

namespace Nocturne.Auth.Core.Modules.Applications.Services
{
    public abstract class ManageApplicationHandler<TCommand>
        where TCommand : ManageApplicationCommand
    {
        private readonly IClientBuilderService clientBuilderService;

        protected ManageApplicationHandler(
            IOpenIddictApplicationManager applicationManager,
            IOpenIddictScopeManager scopeManager,
            IClientBuilderService clientBuilderService,
            IStringLocalizer<ManageApplicationHandler<TCommand>> localizer)
        {
            this.clientBuilderService = clientBuilderService;

            ApplicationManager = applicationManager;
            ScopeManager = scopeManager;
            Localizer = localizer;
        }

        protected IOpenIddictApplicationManager ApplicationManager { get; }

        protected IOpenIddictScopeManager ScopeManager { get; }

        protected IStringLocalizer Localizer { get; }

        public async Task AddAvailableScopesAsync(TCommand command)
        {
            var scopes = ScopeManager.ListAsync();

            await foreach (var scope in scopes)
            {
                var name = await ScopeManager.GetNameAsync(scope);

                command.AvailableScopes.Add(name);
            }
        }

        protected async ValueTask<bool> HasDuplicatedApplication(
            TCommand command,
            object currentApplication = null)
        {
            var application = await ApplicationManager.FindByNameAsync(command.DisplayName);

            var existingApplicationId = application is not null
                ? await ApplicationManager.GetIdAsync(application)
                : null;

            if (existingApplicationId is null)
            {
                return false;
            }

            if (currentApplication is null)
            {
                 return true;
            }

            var currentApplicationId = await ApplicationManager.GetIdAsync(currentApplication);

            return currentApplicationId!.IsEqualInvariant(existingApplicationId) is false;
        }

        protected ApplicationDescriptorBuilder CreateApplicationDescriptorBuilder(
            ManageApplicationCommand command)
        {
            return new ApplicationDescriptorBuilder(
                command,
                ApplicationManager,
                clientBuilderService);
        }
    }
}
