// Copyright (c) Leandro Silva Luz do Carmo
// SPDX-License-Identifier: GPL-3.0-or-later

using System.Text.RegularExpressions;

namespace Nocturne.Auth.Core.Shared.Extensions
{
    public static class StringExtensions
    {
        public static bool IsEqualInvariant(this string value, string valueToCompare)
        {
            return string.Equals(value, valueToCompare, StringComparison.OrdinalIgnoreCase);
        }

        public static string[] GetDelimitedElements(this string value, params char[] separators)
        {
            var valueSeparators = separators.Length > 0 ? separators : new[] { ' ', ',' };

            return value.Split(valueSeparators, StringSplitOptions.RemoveEmptyEntries);
        }

        public static string RemoveLeadingSpaces(this string value)
        {
            Check.NotNull(value, nameof(value));

            return Regex.Replace(value, @"[\s]+", " ", RegexOptions.IgnorePatternWhitespace).Trim();
        }

        public static string Truncate(this string value, int maxLength)
        {
            const string truncateSufix = "...";

            Check.NotNull(value, nameof(value));

            return value.Length <= maxLength
                ? value
                : value[..(maxLength - truncateSufix.Length)] + truncateSufix;
        }

        public static string ToAlphaNumericOnly(this string value)
        {
            Check.NotNull(value, nameof(value));

            return Regex.Replace(value, "[^\\w]", string.Empty);
        }
    }
}
