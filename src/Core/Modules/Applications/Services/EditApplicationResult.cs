namespace Nocturne.Auth.Core.Modules.Applications.Services
{
    public sealed class EditApplicationResult : ManageApplicationResult
    {
        public bool IsNotFound { get; private set; }

        public static EditApplicationResult Updated(string applicationId)
            => Succeded<EditApplicationResult>(applicationId);

        public static EditApplicationResult Fail(string description)
            => Fail<EditApplicationResult>(description);

        public static EditApplicationResult NotFound()
        {
            return new EditApplicationResult
            {
                IsNotFound = true,
            };
        }
    }
}
