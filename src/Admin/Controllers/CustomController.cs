using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Nocturne.Auth.Core.Shared.Results;

namespace Nocturne.Auth.Admin.Controllers
{
    public class CustomController : Controller
    {
        protected void AddErrors(IEnumerable<Problem> problems)
        {
            foreach (var problem in problems)
            {
                AddModelError(problem);
            }
        }

        protected void AddError(string error)
        {
            var problem = new Problem(error);

            AddModelError(problem);
        }

        private void AddModelError(Problem problem)
        {
            ModelState.AddModelError(problem.Name ?? string.Empty, problem.Description);
        }
    }
}
