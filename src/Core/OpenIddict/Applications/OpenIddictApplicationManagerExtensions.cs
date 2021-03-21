using System;
using System.Threading;
using System.Threading.Tasks;
using OpenIddict.Abstractions;

namespace Nocturne.Auth.Core.OpenIddict.Applications
{
    public static class OpenIddictApplicationManagerExtensions
    {
        public static ValueTask<string> GetUnprotectedClientSecret<TManager>(
            this TManager manager,
            object application,
            CancellationToken cancelationToken = default)
            where TManager : IOpenIddictApplicationManager
        {
            if (manager is ICustomOpenIddictApplicationManager customManager)
            {
                return customManager.GetUnprotectedClientSecretAsync(application, cancelationToken);
            }

            throw new NotSupportedException("Application manager does not support this method");
        }
    }
}
