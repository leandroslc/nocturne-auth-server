using Nocturne.Auth.Core.Shared.Collections;

namespace Nocturne.Auth.Core.Modules.Applications.Services
{
    public class ListApplicationsCommand : PagedCommand<ListApplicationsResult>
    {
        public string Name { get; set; }
    }
}
