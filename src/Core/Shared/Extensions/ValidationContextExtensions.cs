// Copyright (c) Leandro Silva Luz do Carmo
// SPDX-License-Identifier: GPL-3.0-or-later

using System.ComponentModel.DataAnnotations;
using Microsoft.Extensions.Localization;

namespace Nocturne.Auth.Core.Shared.Extensions
{
    public static class ValidationContextExtensions
    {
        private static IStringLocalizer localizer;

        public static IStringLocalizer GetLocalizer(this ValidationContext context)
        {
            return localizer ??= (IStringLocalizer)context
                .GetService(typeof(IStringLocalizer<ValidationContext>));
        }
    }
}
