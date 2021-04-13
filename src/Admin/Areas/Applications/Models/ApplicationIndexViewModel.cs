using Nocturne.Auth.Core.Collections;
using Nocturne.Auth.Core.OpenIddict.Applications.Commands;
using Nocturne.Auth.Core.OpenIddict.Applications.Results;

namespace Nocturne.Auth.Admin.Areas.Applications.Models
{
    public class ApplicationIndexViewModel
    {
        public ListApplicationsCommand Query { get; set; }

        public IPagedCollection<ListApplicationsResult> Applications { get; set; }
    }
}
