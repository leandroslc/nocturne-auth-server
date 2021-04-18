using Nocturne.Auth.Core.Modules.Applications.Services;
using Nocturne.Auth.Core.Shared.Collections;

namespace Nocturne.Auth.Admin.Controllers.Models
{
    public class ApplicationIndexViewModel
    {
        public ListApplicationsCommand Query { get; set; }

        public IPagedCollection<ListApplicationsResult> Applications { get; set; }
    }
}
