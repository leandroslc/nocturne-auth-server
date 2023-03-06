// Copyright (c) Leandro Silva Luz do Carmo
// SPDX-License-Identifier: GPL-3.0-or-later

using System.Globalization;
using System.Text.RegularExpressions;
using Nocturne.Auth.Core.Shared.Extensions;

namespace Nocturne.Auth.Core.Shared.Models
{
    public class CPF : IFormattable
    {
        private const int CPFLength = 11;
        private const string WellKnownInvalidNumber = "12345678909";

        public CPF(string value)
        {
            Value = value?.ToAlphaNumericOnly() ?? string.Empty;
        }

        private CPF()
        {
        }

        public string Value { get; private set; }

        public bool Valid => IsValid(Value);

        public static CPF ToCPF(object value)
        {
            if (value is null)
            {
                return new CPF(string.Empty);
            }

            if (value is string stringValue)
            {
                return ToCPF(stringValue);
            }

            throw new InvalidCastException(
                $"Cannot convert object of type {value.GetType()} to {nameof(CPF)}");
        }

        public static CPF ToCPF(string value)
        {
            return new CPF(value);
        }

        public static bool IsValid(string value)
        {
            var cpf = value.ToAlphaNumericOnly();

            if (cpf == WellKnownInvalidNumber)
            {
                return false;
            }

            var digits = ToDigitsArray(cpf);

            if (digits.Length != CPFLength)
            {
                return false;
            }

            if (AreAllDigitsEqual(digits))
            {
                return false;
            }

            var firstVerificationDigit = GetVerificationDigit(digits, 9);
            var secondVerificationDigit = GetVerificationDigit(digits, 10);

            return firstVerificationDigit == digits[9]
                && secondVerificationDigit == digits[10];
        }

        public override string ToString()
        {
            return ToString(null, CultureInfo.InvariantCulture);
        }

        public string ToString(string format, IFormatProvider formatProvider)
        {
            if (string.Equals("n", format, StringComparison.OrdinalIgnoreCase))
            {
                return Value;
            }

            return Regex.Replace(Value, @"(\d{3})(\d{3})(\d{3})(\d{2})", "$1.$2.$3-$4");
        }

        private static int GetVerificationDigit(int[] digits, int numberOfDigits)
        {
            var numberOfDigitsStartingFrom2 = numberOfDigits + 1;

            var sum = 0;

            for (var i = 0; i < numberOfDigits; i++)
            {
                sum += (numberOfDigitsStartingFrom2 - i) * digits[i];
            }

            var modulo = sum % CPFLength;

            return modulo is 1 or 0
                ? 0
                : CPFLength - modulo;
        }

        private static int[] ToDigitsArray(string value)
        {
            return value
                .Select(c => (int)char.GetNumericValue(c))
                .Where(n => n > -1)
                .ToArray();
        }

        private static bool AreAllDigitsEqual(int[] digits)
        {
            return digits.Skip(1).All(n => n == digits[0]);
        }
    }
}
