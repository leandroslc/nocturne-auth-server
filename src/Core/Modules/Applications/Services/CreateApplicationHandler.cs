// Copyright (c) Leandro Silva Luz do Carmo
// SPDX-License-Identifier: GPL-3.0-or-later

using Microsoft.Extensions.Localization;
using Nocturne.Auth.Core.Services.OpenIddict.Services;
using OpenIddict.Abstractions;

namespace Nocturne.Auth.Core.Modules.Applications.Services
{
    public class CreateApplicationHandler : ManageApplicationHandler<CreateApplicationCommand>
    {
        public CreateApplicationHandler(
            IOpenIddictApplicationManager applicationManager,
            IOpenIddictScopeManager scopeManager,
            IClientBuilderService clientBuilderService,
            IStringLocalizer<CreateApplicationHandler> localizer)
            : base(applicationManager, scopeManager, clientBuilderService, localizer)
        {
        }

        public async Task<CreateApplicationCommand> CreateCommandAsync()
        {
            var command = new CreateApplicationCommand();

            await AddAvailableScopesAsync(command);

            return command;
        }

        public async Task<CreateApplicationResult> HandleAsync(CreateApplicationCommand command)
        {
            if (await HasDuplicatedApplication(command))
            {
                return CreateApplicationResult.Fail(
                    Localizer["Application {0} already exists", command.DisplayName]);
            }

            var descriptor = await CreateApplicationDescriptorBuilder(command)
                .BuildAsync();

            var application = await ApplicationManager.CreateAsync(descriptor);

            return CreateApplicationResult.Created(
                await ApplicationManager.GetIdAsync(application));
        }
    }
}
