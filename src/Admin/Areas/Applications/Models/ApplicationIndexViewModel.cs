using Nocturne.Auth.Core.OpenIddict.Applications.Commands;
using Nocturne.Auth.Core.OpenIddict.Applications.Results;
using Nocturne.Auth.Core.Shared.Collections;

namespace Nocturne.Auth.Admin.Areas.Applications.Models
{
    public class ApplicationIndexViewModel
    {
        public ListApplicationsCommand Query { get; set; }

        public IPagedCollection<ListApplicationsResult> Applications { get; set; }
    }
}
