namespace Nocturne.Auth.Core.Modules.Applications.Services
{
    public sealed class CreateApplicationResult : ManageApplicationResult
    {
        public static CreateApplicationResult Created(string applicationId)
            => Succeded<CreateApplicationResult>(applicationId);

        public static CreateApplicationResult Fail(string description)
            => Fail<CreateApplicationResult>(description);
    }
}
