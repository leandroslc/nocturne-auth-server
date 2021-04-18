using System.Threading.Tasks;
using Microsoft.Extensions.Localization;
using Nocturne.Auth.Core.Shared.Results;
using OpenIddict.Abstractions;

namespace Nocturne.Auth.Core.Modules.Applications.Services
{
    public class CreateApplicationHandler : ManageApplicationHandler<CreateApplicationCommand>
    {
        public CreateApplicationHandler(
            IOpenIddictApplicationManager applicationManager,
            IOpenIddictScopeManager scopeManager,
            IStringLocalizer<CreateApplicationHandler> localizer)
            : base(applicationManager, scopeManager, localizer)
        {
        }

        public async Task<CreateApplicationCommand> CreateCommandAsync()
        {
            var command = new CreateApplicationCommand();

            await AddAvailableScopesAsync(command);

            return command;
        }

        public async Task<(Result, string)> HandleAsync(CreateApplicationCommand command)
        {
            if (await HasDuplicatedApplication(command))
            {
                return (
                    Result.Fail(Localizer["Application {0} already exists", command.DisplayName]),
                    null);
            }

            var descriptor = await new ApplicationDescriptorBuilder(
                command, ApplicationManager)
                .BuildAsync();

            var application = await ApplicationManager.CreateAsync(descriptor);

            return (
                Result.Success,
                await ApplicationManager.GetIdAsync(application));
        }
    }
}
