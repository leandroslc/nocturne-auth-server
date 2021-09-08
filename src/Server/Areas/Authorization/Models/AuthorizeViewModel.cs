// Copyright (c) Leandro Silva Luz do Carmo
// SPDX-License-Identifier: GPL-3.0-or-later

using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;

namespace Nocturne.Auth.Server.Areas.Authorization.Models
{
    public class AuthorizeViewModel
    {
        private readonly HttpRequest request;

        public AuthorizeViewModel(
            HttpRequest request,
            string userName,
            string applicationName,
            IReadOnlyCollection<ScopeViewModel> scopes)
        {
            this.request = request;

            ApplicationName = applicationName;
            Scopes = scopes;
            UserName = userName;
        }

        public string ApplicationName { get; }

        public IReadOnlyCollection<ScopeViewModel> Scopes { get; }

        public string UserName { get; }

        public IEnumerable<KeyValuePair<string, StringValues>> RequestParameters
            => request.HasFormContentType ? request.Form : request.Query;
    }
}
