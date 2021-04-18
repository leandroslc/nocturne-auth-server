using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using OpenIddict.Abstractions;
using OpenIddict.Core;

namespace Nocturne.Auth.Core.Services.OpenIddict.Managers
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

            throw MethodNotSupported();
        }

        public static async ValueTask<object> FindByNameAsync<TManager>(
            this TManager manager,
            string name,
            CancellationToken cancellationToken = default)
            where TManager : IOpenIddictApplicationManager
        {
            if (manager is OpenIddictApplicationManager<Application> customManager)
            {
                return await customManager
                    .GetAsync(
                        query => query.Where(a => a.DisplayName == name),
                        cancellationToken);
            }

            throw MethodNotSupported();
        }

        private static NotSupportedException MethodNotSupported()
            => new("The application manager does not support this method");
    }
}
