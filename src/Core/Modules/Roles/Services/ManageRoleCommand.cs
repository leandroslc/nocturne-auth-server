// Copyright (c) Leandro Silva Luz do Carmo
// SPDX-License-Identifier: GPL-3.0-or-later

using System.ComponentModel.DataAnnotations;

namespace Nocturne.Auth.Core.Modules.Roles.Services
{
    public abstract class ManageRoleCommand
    {
        protected ManageRoleCommand()
        {
        }

        [Required(ErrorMessage = "The name is required")]
        [MaxLength(200, ErrorMessage = "The name must have less than {1} characters")]
        public string Name { get; set; }

        [MaxLength(400, ErrorMessage = "The description must have less than {1} characters")]
        public string Description { get; set; }
    }
}
