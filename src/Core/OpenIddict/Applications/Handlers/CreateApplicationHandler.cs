using System.Threading.Tasks;
using Nocturne.Auth.Core.OpenIddict.Applications.Commands;
using OpenIddict.Abstractions;

namespace Nocturne.Auth.Core.OpenIddict.Applications.Handlers
{
    public class CreateApplicationHandler : ManageApplicationHandler<CreateApplicationCommand>
    {
        public CreateApplicationHandler(
            IOpenIddictApplicationManager applicationManager,
            IOpenIddictScopeManager scopeManager)
            : base(applicationManager, scopeManager)
        {
        }

        public async Task<CreateApplicationCommand> CreateCommandAsync()
        {
            var command = new CreateApplicationCommand();

            await AddAvailableScopesAsync(command);

            return command;
        }

        public override async Task<string> HandleAsync(CreateApplicationCommand command)
        {
            var descriptor = await new ApplicationDescriptorBuilder(
                command, ApplicationManager)
                .BuildAsync();

            var application = await ApplicationManager.CreateAsync(descriptor);

            return await ApplicationManager.GetIdAsync(application);
        }
    }
}
