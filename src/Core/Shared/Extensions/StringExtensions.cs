using System;

namespace Nocturne.Auth.Core.Shared.Extensions
{
    public static class StringExtensions
    {
        public static bool IsEqualInvariant(this string value, string valueToCompare)
        {
            return string.Equals(value, valueToCompare, StringComparison.InvariantCultureIgnoreCase);
        }

        public static string[] GetDelimitedElements(this string value, params char[] separators)
        {
            var valueSeparators = separators?.Length > 0 ? separators : new[] { ' ', ',' };

            return value.Split(valueSeparators, StringSplitOptions.RemoveEmptyEntries);
        }
    }
}
