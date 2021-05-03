namespace Nocturne.Auth.Core.Modules.Roles.Services
{
    public sealed class UnassignPermissionFromRoleResult
    {
        public bool IsNotFound { get; private set; }

        public bool IsSuccess { get; private set; }

        public string ErrorDescription { get; private set; }

        public static UnassignPermissionFromRoleResult NotFound(string description)
        {
            return new UnassignPermissionFromRoleResult
            {
                IsNotFound = true,
                ErrorDescription = description,
            };
        }

        public static UnassignPermissionFromRoleResult Success()
        {
            return new UnassignPermissionFromRoleResult
            {
                IsSuccess = true,
            };
        }
    }
}
