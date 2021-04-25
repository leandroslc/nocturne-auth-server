namespace Nocturne.Auth.Core.Modules.Permissions.Services
{
    public sealed class ViewApplicationPermissionResult
    {
        public bool IsSuccess { get; private set; }

        public bool IsNotFound { get; private set; }

        public ViewApplicationPermissionItem Permission { get; private set; }

        public static ViewApplicationPermissionResult Success(Permission permission)
        {
            return new ViewApplicationPermissionResult
            {
                IsSuccess = true,
                Permission = new ViewApplicationPermissionItem(permission),
            };
        }

        public static ViewApplicationPermissionResult NotFound()
        {
            return new ViewApplicationPermissionResult
            {
                IsNotFound = true,
            };
        }
    }
}
