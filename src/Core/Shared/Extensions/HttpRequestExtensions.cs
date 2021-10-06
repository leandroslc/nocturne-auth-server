// Copyright (c) Leandro Silva Luz do Carmo
// SPDX-License-Identifier: GPL-3.0-or-later

using System;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.WebUtilities;

namespace Nocturne.Auth.Core.Shared.Extensions
{
    public static class HttpRequestExtensions
    {
        public static Uri CreateUrlWithNewQuery(
            this HttpRequest request,
            params (string, string)[] parameters)
        {
            Check.NotNull(request, nameof(request));

            var values = request.Query.ToDictionary(q => q.Key, q => q.Value.ToString());

            foreach (var parameter in parameters)
            {
                values.AddOrReplace(parameter.Item1, parameter.Item2);
            }

            return new Uri(QueryHelpers.AddQueryString(request.Path, values), UriKind.Relative);
        }
    }
}
