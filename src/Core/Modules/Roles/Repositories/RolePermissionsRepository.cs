// Copyright (c) Leandro Silva Luz do Carmo
// SPDX-License-Identifier: GPL-3.0-or-later

using Microsoft.EntityFrameworkCore;
using Nocturne.Auth.Core.Modules.Permissions;

namespace Nocturne.Auth.Core.Modules.Roles.Repositories
{
    public class RolePermissionsRepository : IRolePermissionsRepository
    {
        private readonly AuthorizationDbContext context;

        public RolePermissionsRepository(AuthorizationDbContext context)
        {
            this.context = context;
        }

        public async Task AssignPermissionsAsync(Role role, IEnumerable<Permission> permissions)
        {
            var rolePermissions = permissions.Select(p => new RolePermission(role, p));

            context.AddRange(rolePermissions);

            await context.SaveChangesAsync();
        }

        public async Task<IReadOnlyCollection<Permission>> GetUnassignedPermissionsAsync(
            Role role,
            string applicationId)
        {
            return await (
                from permission in context.Set<Permission>()
                join rolePermission in context.Set<RolePermission>()
                    on new { PermissionId = permission.Id, RoleId = role.Id }
                    equals new { rolePermission.PermissionId, rolePermission.RoleId }
                    into rolePermissionJoin
                from rolePermission in rolePermissionJoin.DefaultIfEmpty()
                where permission.ApplicationId == applicationId && rolePermission == null
                select permission)
                .ToListAsync();
        }

        public async Task UnassignPermissionAsync(Role role, Permission permission)
        {
            var rolePermission = await context
                .Set<RolePermission>()
                .FirstOrDefaultAsync(p => p.RoleId == role.Id && p.PermissionId == permission.Id);

            if (rolePermission is null)
            {
                return;
            }

            context.Remove(rolePermission);

            await context.SaveChangesAsync();
        }
    }
}
