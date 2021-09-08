// Copyright (c) Leandro Silva Luz do Carmo
// SPDX-License-Identifier: GPL-3.0-or-later

using System;
using Nocturne.Auth.Core.Services.OpenIddict;
using Nocturne.Auth.Core.Shared.Extensions;

namespace Nocturne.Auth.Core.Modules.Roles
{
    public class Role
    {
        public Role(
            string name,
            string applicationId)
            : this(name, applicationId, null)
        {
        }

        public Role(
            string name,
            string applicationId,
            string description)
        {
            Check.NotNull(applicationId, nameof(applicationId));

            ApplicationId = applicationId;

            SetName(name);
            SetDescription(description);
        }

        public long Id { get; private set; }

        public string Name { get; private set; }

        public string Description { get; private set; }

        public Application Application { get; private set; }

        public string ApplicationId { get; private set; }

        public string ConcurrencyToken { get; private set; }

        public void SetName(string name)
        {
            Check.NotNull(name, nameof(name));

            Name = name?.RemoveLeadingSpaces();
        }

        public void SetDescription(string description)
        {
            Description = description?.RemoveLeadingSpaces();
        }

        public void UpdateConcurrencyToken()
        {
            ConcurrencyToken = Guid.NewGuid().ToString();
        }
    }
}
