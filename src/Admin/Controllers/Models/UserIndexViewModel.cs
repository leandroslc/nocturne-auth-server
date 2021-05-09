using Nocturne.Auth.Core.Modules.Users.Services;
using Nocturne.Auth.Core.Shared.Collections;

namespace Nocturne.Auth.Admin.Controllers.Models
{
    public class UserIndexViewModel
    {
        public ListUsersCommand Query { get; set; }

        public IPagedCollection<ListUsersItem> Users { get; set; }
    }
}
