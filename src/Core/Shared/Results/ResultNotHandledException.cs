// Copyright (c) Leandro Silva Luz do Carmo
// SPDX-License-Identifier: GPL-3.0-or-later

using System;
using System.Runtime.Serialization;

namespace Nocturne.Auth.Core.Shared.Results
{
    [Serializable]
    public sealed class ResultNotHandledException : Exception
    {
        public ResultNotHandledException()
            : base()
        {
        }

        public ResultNotHandledException(string message)
            : base(message)
        {
        }

        public ResultNotHandledException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        public ResultNotHandledException(object result)
            : base(GetMessageError(result))
        {
        }

        private ResultNotHandledException(
            SerializationInfo info,
            StreamingContext context)
            : base(info, context)
        {
        }

        private static string GetMessageError(object result)
        {
            Check.NotNull(result, nameof(result));

            var typeName = result.GetType().FullName;

            return $"Result of type {typeName} was not properly handled";
        }
    }
}
