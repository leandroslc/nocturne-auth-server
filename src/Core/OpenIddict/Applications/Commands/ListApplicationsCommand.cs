using Nocturne.Auth.Core.OpenIddict.Applications.Results;
using Nocturne.Auth.Core.Shared.Collections;

namespace Nocturne.Auth.Core.OpenIddict.Applications.Commands
{
    public class ListApplicationsCommand : PagedCommand<ListApplicationsResult>
    {
        public string Name { get; set; }
    }
}
