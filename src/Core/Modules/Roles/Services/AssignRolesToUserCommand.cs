using System.Collections.Generic;

namespace Nocturne.Auth.Core.Modules.Roles.Services
{
    public sealed class AssignRolesToUserCommand
    {
        public AssignRolesToUserCommand()
        {
        }

        public AssignRolesToUserCommand(
            string currentApplicationId,
            IReadOnlyCollection<RoleApplication> availableApplications)
        {
            CurrentApplicationId = currentApplicationId;
            AvailableApplications = availableApplications;
        }

        public long? UserId { get; set; }

        public IReadOnlyCollection<AssignRolesToUserRole> Roles { get; set; }

        public string CurrentApplicationId { get; private set; }

        public IReadOnlyCollection<RoleApplication> AvailableApplications { get; private set; }
    }
}
