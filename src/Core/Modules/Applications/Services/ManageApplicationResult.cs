using System.Collections.Generic;
using Nocturne.Auth.Core.Shared.Results;

namespace Nocturne.Auth.Core.Modules.Applications.Services
{
    public class ManageApplicationResult
    {
        public IReadOnlyCollection<Problem> Problems { get; private set; }

        public string ApplicationId { get; private set; }

        public bool Success { get; private set; }

        public bool HasProblems { get; private set; }

        protected static TResult Succeded<TResult>(string applicationId)
            where TResult : ManageApplicationResult, new()
        {
            return new TResult
            {
                Success = true,
                ApplicationId = applicationId,
            };
        }

        protected static TResult Fail<TResult>(string description)
            where TResult : ManageApplicationResult, new()
        {
            return new TResult
            {
                HasProblems = true,
                Problems = new[] { new Problem(description) },
            };
        }
    }
}
