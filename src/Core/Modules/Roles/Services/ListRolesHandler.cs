// Copyright (c) Leandro Silva Luz do Carmo
// SPDX-License-Identifier: GPL-3.0-or-later

using System.Diagnostics.CodeAnalysis;
using Nocturne.Auth.Core.Modules.Roles.Repositories;

namespace Nocturne.Auth.Core.Modules.Roles.Services
{
    public sealed class ListRolesHandler
    {
        private readonly IRolesRepository rolesRepository;

        public ListRolesHandler(
            IRolesRepository rolesRepository)
        {
            this.rolesRepository = rolesRepository;
        }

        public async Task<ListRolesResult> HandleAsync(
            ListRolesCommand command)
        {
            var roles = await rolesRepository.Query(query => GetRoles(query, command));

            return ListRolesResult.Success(roles);
        }

        [SuppressMessage("Globalization", "CA1307", Justification = "Will fail within LINQ provider")]
        private static IQueryable<ListRolesItem> GetRoles(
            IQueryable<Role> query,
            ListRolesCommand command)
        {
            if (string.IsNullOrWhiteSpace(command.Name) is false)
            {
                query = query.Where(p => p.Name.Contains(command.Name));
            }

            query = query.OrderBy(p => p.Name);

            return query.Select(p => new ListRolesItem
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
            });
        }
    }
}
