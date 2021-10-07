// Copyright (c) Leandro Silva Luz do Carmo
// SPDX-License-Identifier: GPL-3.0-or-later

using System;
using System.Security.Cryptography.X509Certificates;

namespace Nocturne.Auth.Core.Shared.Helpers
{
    public static class X509CertificateLocator
    {
        public static X509Certificate2 FindByThumbprint(string thumbprint)
        {
            return Find(
                StoreName.My,
                StoreLocation.LocalMachine,
                X509FindType.FindByThumbprint,
                thumbprint);
        }

        private static X509Certificate2 Find(
            StoreName storeName,
            StoreLocation storeLocation,
            X509FindType findType,
            object findValue)
        {
            const bool allowSelfSignedCertificates = true;

            using var store = new X509Store(
                storeName,
                storeLocation,
                OpenFlags.ReadOnly);

            var foundCertificates = store.Certificates.Find(
                findType,
                findValue,
                validOnly: !allowSelfSignedCertificates);

            if (foundCertificates.Count == 0)
            {
                throw new InvalidOperationException(
                    $"No certificate found using the specified search criteria");
            }

            return foundCertificates[0];
        }
    }
}
