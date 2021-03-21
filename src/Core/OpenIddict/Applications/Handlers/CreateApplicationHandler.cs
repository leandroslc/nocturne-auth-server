using System.Threading.Tasks;
using Nocturne.Auth.Core.OpenIddict.Applications.Commands;
using OpenIddict.Abstractions;

namespace Nocturne.Auth.Core.OpenIddict.Applications.Handlers
{
    public class CreateApplicationHandler : ManageApplicationHandler<CreateApplicationCommand>
    {
        public CreateApplicationHandler(
            IOpenIddictApplicationManager applicationManager)
            : base(applicationManager)
        {
        }

        public override async Task HandleAsync(CreateApplicationCommand command)
        {
            var descriptor = await GenerateDescriptorAsync(command);

            await ApplicationManager.CreateAsync(descriptor);
        }
    }
}
