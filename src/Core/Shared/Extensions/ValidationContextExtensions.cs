// Copyright (c) Leandro Silva Luz do Carmo
// SPDX-License-Identifier: GPL-3.0-or-later

using System.ComponentModel.DataAnnotations;
using Microsoft.Extensions.Localization;

namespace Nocturne.Auth.Core.Shared.Extensions
{
    /// <summary>
    /// Extension methods for the <see cref="ValidationContext" /> class
    /// </summary>
    public static class ValidationContextExtensions
    {
        private static IStringLocalizer localizer;

        /// <summary>
        /// Gets an <see cref="IStringLocalizer" /> to help localize error messages
        /// </summary>
        /// <param name="context">An instance of <see cref="ValidationContext" /></param>
        public static IStringLocalizer GetLocalizer(this ValidationContext context)
        {
            return localizer ??= (IStringLocalizer)
                context.GetService(typeof(IStringLocalizer<ValidationContext>));
        }
    }
}
