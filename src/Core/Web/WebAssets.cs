// Copyright (c) Leandro Silva Luz do Carmo
// SPDX-License-Identifier: GPL-3.0-or-later

namespace Nocturne.Auth.Core.Web
{
    public class WebAssets
    {
        public WebAssets(WebAssetsOptions options)
        {
            Check.NotNull(options, nameof(options));

            BaseUrl = GetBaseUrl(options);
        }

        public Uri BaseUrl { get; }

        public Uri Url(string relativePath)
        {
            const char Separator = '/';

            if (string.IsNullOrWhiteSpace(relativePath))
            {
                return BaseUrl;
            }

            return new Uri(BaseUrl, relativePath.TrimStart(Separator));
        }

        private static Uri GetBaseUrl(WebAssetsOptions options)
        {
            if (options.BaseUrl is null)
            {
                throw new InvalidOperationException(
                    "The web assets base url address cannot be empty");
            }

            return options.BaseUrl;
        }
    }
}
