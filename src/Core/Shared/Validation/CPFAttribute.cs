using System.ComponentModel.DataAnnotations;
using Nocturne.Auth.Core.Shared.Models;

namespace Nocturne.Auth.Core.Shared.Validation
{
    public class CPFAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var cpfValue = value as string;

            if (string.IsNullOrWhiteSpace(cpfValue))
            {
                return ValidationResult.Success;
            }

            if (CPF.IsValid(cpfValue))
            {
                return ValidationResult.Success;
            }

            return new ValidationResult(ErrorMessage);
        }
    }
}
