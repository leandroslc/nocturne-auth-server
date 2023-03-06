// Copyright (c) Leandro Silva Luz do Carmo
// SPDX-License-Identifier: GPL-3.0-or-later

using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Nocturne.Auth.Admin.Services
{
    public static class ModelStateDictionaryExtensions
    {
        public static async Task AddErrorsFromValidationAsync(
            this ModelStateDictionary modelState,
            IAsyncEnumerable<ValidationResult> results)
        {
            if (results == null)
            {
                return;
            }

            await foreach (var result in results)
            {
                modelState.AddModelError(
                    result.MemberNames.FirstOrDefault() ?? string.Empty,
                    result.ErrorMessage);
            }
        }
    }
}
