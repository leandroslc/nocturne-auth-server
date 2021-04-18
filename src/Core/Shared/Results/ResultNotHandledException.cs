using System;
using System.Runtime.Serialization;

namespace Nocturne.Auth.Core.Shared.Results
{
    [Serializable]
    public class ResultNotHandledException : Exception
    {
        public ResultNotHandledException(object result)
            : base(GetMessageError(result))
        {
        }

        protected ResultNotHandledException(
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
