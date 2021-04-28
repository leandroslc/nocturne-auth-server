using System.ComponentModel.DataAnnotations;

namespace Nocturne.Auth.Core.Modules.Roles.Services
{
    public sealed class DeleteApplicationRoleCommand
    {
        public DeleteApplicationRoleCommand()
        {
        }

        public DeleteApplicationRoleCommand(Role role)
        {
            Id = role.Id;
            Name = role.Name;
        }

        public long? Id { get; set; }

        [Required(ErrorMessage = "The name is required")]
        public string Name { get; set; }

        [Required(ErrorMessage = "The confirmation name is required")]
        [Compare("Name", ErrorMessage = "The confirmation name does not match")]
        public string NameConfirmation { get; set; }
    }
}
