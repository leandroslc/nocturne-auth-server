// Copyright (c) Leandro Silva Luz do Carmo
// SPDX-License-Identifier: GPL-3.0-or-later

using System;
using System.Collections.Generic;
using Nocturne.Auth.Core.Shared.Extensions;

namespace Nocturne.Auth.Core.Shared.Helpers
{
    public static class UriHelper
    {
        public static IEnumerable<Uri> GetUris(string uris)
        {
            if (uris == null)
            {
                yield break;
            }

            foreach (var value in uris.GetDelimitedElements())
            {
                if (TryCreate(value, out var uri))
                {
                    yield return uri;
                }
            }
        }

        public static bool TryCreate(string value, out Uri uri)
        {
            var created = Uri.TryCreate(value, UriKind.Absolute, out uri);

            return created && uri.IsWellFormedOriginalString();
        }
    }
}
