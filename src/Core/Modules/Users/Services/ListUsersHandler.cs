// Copyright (c) Leandro Silva Luz do Carmo
// SPDX-License-Identifier: GPL-3.0-or-later

using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Nocturne.Auth.Core.Services.Identity;
using Nocturne.Auth.Core.Shared.Collections;

namespace Nocturne.Auth.Core.Modules.Users.Services
{
    public sealed class ListUsersHandler
    {
        private readonly UserManager<ApplicationUser> userManager;

        public ListUsersHandler(
            UserManager<ApplicationUser> userManager)
        {
            this.userManager = userManager;
        }

        public async Task<IPagedCollection<ListUsersItem>> HandleAsync(ListUsersCommand command)
        {
            var query = Query(command);

            var total = await query.LongCountAsync();
            var users = await QueryWithPages(query, command).ToListAsync();

            return new PagedCollection<ListUsersItem>(
                users, command.Page, command.PageSize, total);
        }

        [SuppressMessage("Globalization", "CA1307", Justification = "Will fail within LINQ provider")]
        private IQueryable<ListUsersItem> Query(ListUsersCommand command)
        {
            var query = userManager.Users;

            if (string.IsNullOrWhiteSpace(command.Name) is false)
            {
                query = query.Where(u => u.Name.Contains(command.Name));
            }

            query = query.OrderBy(u => u.Name);

            return query.Select(u => new ListUsersItem
            {
                Id = u.Id,
                Name = u.Name,
                UserName = u.UserName,
            });
        }

        private static IQueryable<ListUsersItem> QueryWithPages(
            IQueryable<ListUsersItem> query,
            ListUsersCommand command)
        {
            return query
                .Skip((command.Page - 1) * command.PageSize)
                .Take(command.PageSize);
        }
    }
}
