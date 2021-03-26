using System.Collections.Generic;
using Nocturne.Auth.Core.OpenIddict.Applications.Commands;
using Nocturne.Auth.Core.OpenIddict.Applications.Results;

namespace Nocturne.Auth.Admin.Areas.Applications.Models
{
    public class ApplicationIndexViewModel
    {
        public ListApplicationsCommand Query { get; set; }

        public IReadOnlyCollection<ListApplicationsResult> Applications { get; set; }
    }
}
