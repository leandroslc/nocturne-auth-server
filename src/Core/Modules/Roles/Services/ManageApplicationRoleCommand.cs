using System.ComponentModel.DataAnnotations;

namespace Nocturne.Auth.Core.Modules.Roles.Services
{
    public abstract class ManageApplicationRoleCommand
    {
        [Required(ErrorMessage = "The name is required")]
        [MaxLength(200, ErrorMessage = "The name must have less than {1} characters")]
        public string Name { get; set; }

        [MaxLength(400, ErrorMessage = "The description must have less than {1} characters")]
        public string Description { get; set; }

        [Required(ErrorMessage = "The application is required")]
        public string ApplicationId { get; set; }
    }
}