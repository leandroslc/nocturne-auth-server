// Copyright (c) Leandro Silva Luz do Carmo
// SPDX-License-Identifier: GPL-3.0-or-later

using System.ComponentModel.DataAnnotations;

namespace Nocturne.Auth.Core.Modules.Permissions.Services
{
    public sealed class DeleteApplicationPermissionCommand
    {
        public DeleteApplicationPermissionCommand()
        {
        }

        public DeleteApplicationPermissionCommand(Permission permission)
        {
            Id = permission.Id;
            Name = permission.Name;
        }

        public long? Id { get; set; }

        [Required(ErrorMessage = "The name is required")]
        public string Name { get; set; }

        [Required(ErrorMessage = "The confirmation name is required")]
        [Compare("Name", ErrorMessage = "The confirmation name does not match")]
        public string NameConfirmation { get; set; }
    }
}
