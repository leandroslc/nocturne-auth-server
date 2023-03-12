// Copyright (c) Leandro Silva Luz do Carmo
// SPDX-License-Identifier: GPL-3.0-or-later

using Nocturne.Auth.Core.Shared.Extensions;

namespace Nocturne.Auth.Core.Modules.Roles
{
    public class Role
    {
        public Role(string name)
            : this(name, null)
        {
        }

        public Role(
            string name,
            string description)
        {
            SetName(name);
            SetDescription(description);
        }

        public long Id { get; private set; }

        public string Name { get; private set; }

        public string Description { get; private set; }

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
