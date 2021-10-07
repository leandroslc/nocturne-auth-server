// Copyright (c) Leandro Silva Luz do Carmo
// SPDX-License-Identifier: GPL-3.0-or-later

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Nocturne.Auth.Core.Shared.Extensions;

namespace Nocturne.Auth.Core.Shared.Helpers
{
    public static class UriValidator
    {
        public static IEnumerable<ValidationResult> Validate(
            ValidationContext context,
            string uris,
            string memberName)
        {
            var invalidUris = GetInvalidUris(uris);

            var localizer = context.GetLocalizer();

            foreach (var invalidUri in invalidUris)
            {
                yield return new ValidationResult(
                    localizer["{0} is invalid", invalidUri],
                    new[] { memberName });
            }
        }

        private static IEnumerable<string> GetInvalidUris(string uris)
        {
            if (uris == null)
            {
                yield break;
            }

            foreach (var uriValue in uris.GetDelimitedElements())
            {
                if (!UriHelper.TryCreate(uriValue, out var _))
                {
                    yield return uriValue;
                }
            }
        }
    }
}
