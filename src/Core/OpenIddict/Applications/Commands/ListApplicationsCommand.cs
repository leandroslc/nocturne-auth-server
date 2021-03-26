using Nocturne.Auth.Core.Collections;
using Nocturne.Auth.Core.OpenIddict.Applications.Results;

namespace Nocturne.Auth.Core.OpenIddict.Applications.Commands
{
    public class ListApplicationsCommand : PagedCommand<ListApplicationsResult>
    {
        public string Name { get; set; }
    }
}
