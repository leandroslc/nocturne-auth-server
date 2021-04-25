namespace Nocturne.Auth.Core.Modules.Permissions.Services
{
    public sealed class DeleteApplicationPermissionResult
    {
        public bool IsSuccess { get; private set; }

        public bool IsNotFound { get; private set; }

        public static DeleteApplicationPermissionResult Success()
        {
            return new DeleteApplicationPermissionResult
            {
                IsSuccess = true,
            };
        }

        public static DeleteApplicationPermissionResult NotFound()
        {
            return new DeleteApplicationPermissionResult
            {
                IsNotFound = true,
            };
        }
    }
}
