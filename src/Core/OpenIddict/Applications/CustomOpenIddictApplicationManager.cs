using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Nocturne.Auth.Core.Crypto;
using OpenIddict.Abstractions;
using OpenIddict.Core;

namespace Nocturne.Auth.Core.OpenIddict.Applications
{
    public class CustomOpenIddictApplicationManager<TApplication>
        : OpenIddictApplicationManager<TApplication>, ICustomOpenIddictApplicationManager
        where TApplication : class
    {
        private readonly IEncryptionService encryptionService;

        public CustomOpenIddictApplicationManager(
            IEncryptionService encryptionService,
            IOpenIddictApplicationCache<TApplication> cache,
            ILogger<OpenIddictApplicationManager<TApplication>> logger,
            IOptionsMonitor<OpenIddictCoreOptions> options,
            IOpenIddictApplicationStoreResolver resolver)
            : base(cache, logger, options, resolver)
        {
            this.encryptionService = encryptionService;
        }

        public async ValueTask<string> GetUnprotectedClientSecretAsync(
            TApplication application,
            CancellationToken cancellationToken = default)
        {
            var secret = await GetClientSecretAsync(application, cancellationToken);

            return secret is not null
                ? encryptionService.Decrypt(secret)
                : null;
        }

        public ValueTask<string> GetClientSecretAsync(
            TApplication application,
            CancellationToken cancellationToken = default)
        {
            if (application == null)
            {
                throw new ArgumentNullException(nameof(application));
            }

            return Store.GetClientSecretAsync(application, cancellationToken);
        }

        protected override ValueTask<string> ObfuscateClientSecretAsync(
            string secret,
            CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(secret))
            {
                throw new ArgumentException("Secret cannot be empty", nameof(secret));
            }

            return ValueTask.FromResult(encryptionService.Encrypt(secret));
        }

        protected override ValueTask<bool> ValidateClientSecretAsync(
            string secret,
            string protectedSecret,
            CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(secret))
            {
                throw new ArgumentException("Secrect cannot be empty", nameof(secret));
            }

            if (string.IsNullOrEmpty(protectedSecret))
            {
                throw new ArgumentException("Value cannot be empty", nameof(protectedSecret));
            }

            try
            {
                var unprotectedSecret = encryptionService.Decrypt(protectedSecret);

                return ValueTask.FromResult(
                    string.Equals(secret, unprotectedSecret, StringComparison.InvariantCulture));
            }
            catch (Exception exception)
            {
                Logger.LogWarning(exception, "Client secrect validation failed");

                return new ValueTask<bool>(false);
            }
        }

        ValueTask<string> ICustomOpenIddictApplicationManager.GetUnprotectedClientSecretAsync(
            object application, CancellationToken cancellationToken)
            => GetUnprotectedClientSecretAsync((TApplication)application, cancellationToken);
    }
}
