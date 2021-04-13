using System;

namespace Nocturne.Auth.Core
{
    public static class Check
    {
        public static void NotNull<TValue>(TValue value, string paramName)
            where TValue : class
        {
            if (value == null)
            {
                throw new ArgumentNullException(paramName);
            }
        }
    }
}
