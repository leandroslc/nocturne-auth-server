// Copyright (c) Leandro Silva Luz do Carmo
// SPDX-License-Identifier: GPL-3.0-or-later

using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;

namespace Nocturne.Auth.Server.Services
{
    public class CurrentRequestUriBuilder
    {
        private readonly string url;

        private readonly ICollection<KeyValuePair<string, StringValues>> parameters;

        public CurrentRequestUriBuilder(HttpRequest request)
        {
            url = request.PathBase + request.Path;

            parameters = request.HasFormContentType
                ? request.Form.ToList()
                : request.Query.ToList();
        }

        public CurrentRequestUriBuilder ReplaceParameter(
            string name,
            IEnumerable<string> values)
        {
            RemoveParameter(name);

            var value = new StringValues(string.Join(" ", values));

            parameters.Add(KeyValuePair.Create(name, value));

            return this;
        }

        public CurrentRequestUriBuilder RemoveParameter(string name)
        {
            var parametersToRemove = parameters.Where(p => p.Key == name);

            foreach (var parameter in parametersToRemove)
            {
                parameters.Remove(parameter);
            }

            return this;
        }

        public string Build()
        {
            return url + QueryString.Create(parameters);
        }
    }
}
