// Copyright (c) Leandro Silva Luz do Carmo
// SPDX-License-Identifier: GPL-3.0-or-later

using System.ComponentModel.DataAnnotations;
using Nocturne.Auth.Core.Shared.Models;

namespace Nocturne.Auth.Core.Shared.Validation
{
    public sealed class CPFAttribute : ValidationAttribute
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
