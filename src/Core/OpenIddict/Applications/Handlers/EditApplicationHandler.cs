using System.Threading.Tasks;
using Nocturne.Auth.Core.OpenIddict.Applications.Commands;
using OpenIddict.Abstractions;

namespace Nocturne.Auth.Core.OpenIddict.Applications.Handlers
{
    public class EditApplicationHandler : ManageApplicationHandler<EditApplicationCommand>
    {
        public EditApplicationHandler(
            IOpenIddictApplicationManager applicationManager)
            : base(applicationManager)
        {
        }

        public override async Task HandleAsync(EditApplicationCommand command)
        {
            var application = await GetApplication(command);

            var descriptor = await GenerateDescriptorAsync(command, application);

            await ApplicationManager.UpdateAsync(application, descriptor);
        }

        public async Task<bool> Exists(EditApplicationCommand command)
        {
            return await GetApplication(command) is not null;
        }

        private async Task<object> GetApplication(EditApplicationCommand command)
        {
            return await ApplicationManager.FindByIdAsync(command.Id);
        }
    }
}
