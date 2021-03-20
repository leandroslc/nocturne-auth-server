using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Nocturne.Auth.Admin.Services
{
    public static class ModelStateDictionaryExtensions
    {
        public static void AddErrorsFromValidation(
            this ModelStateDictionary modelState,
            IEnumerable<ValidationResult> results)
        {
            if (results == null)
            {
                return;
            }

            foreach (var result in results)
            {
                modelState.AddModelError(
                    result.MemberNames.FirstOrDefault() ?? string.Empty,
                    result.ErrorMessage);
            }
        }
    }
}
