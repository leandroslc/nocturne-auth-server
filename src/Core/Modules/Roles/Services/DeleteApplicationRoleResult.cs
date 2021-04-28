namespace Nocturne.Auth.Core.Modules.Roles.Services
{
    public sealed class DeleteApplicationRoleResult
    {
        public bool IsSuccess { get; private set; }

        public bool IsNotFound { get; private set; }

        public static DeleteApplicationRoleResult Success()
        {
            return new DeleteApplicationRoleResult
            {
                IsSuccess = true,
            };
        }

        public static DeleteApplicationRoleResult NotFound()
        {
            return new DeleteApplicationRoleResult
            {
                IsNotFound = true,
            };
        }
    }
}
