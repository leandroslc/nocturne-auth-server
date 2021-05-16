namespace Nocturne.Auth.Core.Modules.Users.Services
{
    public sealed class GetUserAccessResult
    {
        public bool IsSuccess { get; private set; }

        public bool IsNotFound { get; private set; }

        public string ErrorDescription { get; private set; }

        public GetUserAccessReturn Value { get; private set; }

        public static GetUserAccessResult Success(GetUserAccessReturn value)
        {
            return new GetUserAccessResult
            {
                IsSuccess = true,
                Value = value,
            };
        }

        public static GetUserAccessResult NotFound(string description)
        {
            return new GetUserAccessResult
            {
                IsNotFound = true,
                ErrorDescription = description,
            };
        }
    }
}
