// Copyright (c) Leandro Silva Luz do Carmo
// SPDX-License-Identifier: GPL-3.0-or-later

using System;

namespace Nocturne.Auth.Core.Web
{
    public class WebAssets
    {
        public WebAssets(WebAssetsOptions options)
        {
            Check.NotNull(options, nameof(options));

            BaseUrl = GetBaseUrl(options);
        }

        public string BaseUrl { get; }

        public string Url(string relativePath)
        {
            const char Separator = '/';

            if (string.IsNullOrWhiteSpace(relativePath))
            {
                return BaseUrl;
            }

            return $"{BaseUrl.TrimEnd(Separator)}/{relativePath.TrimStart(Separator)}";
        }

        private static string GetBaseUrl(WebAssetsOptions options)
        {
            if (string.IsNullOrWhiteSpace(options.BaseUrl))
            {
                throw new InvalidOperationException(
                    "The web assets base url address cannot be empty");
            }

            return options.BaseUrl;
        }
    }
}
