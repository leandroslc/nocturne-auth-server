// Copyright (c) Leandro Silva Luz do Carmo
// SPDX-License-Identifier: GPL-3.0-or-later

using Microsoft.AspNetCore.Mvc.Rendering;
using Nocturne.Auth.Server.Configuration.Constants;

namespace Nocturne.Auth.Server.Areas.Identity.Pages.Account.Manage
{
    public static class ManageNavPages
    {
        public const string Index = nameof(Index);

        public const string Email = nameof(Email);

        public const string ChangePassword = nameof(ChangePassword);

        public const string DownloadPersonalData = nameof(DownloadPersonalData);

        public const string DeletePersonalData = nameof(DeletePersonalData);

        public const string ExternalLogins = nameof(ExternalLogins);

        public const string PersonalData = nameof(PersonalData);

        public const string TwoFactorAuthentication = nameof(TwoFactorAuthentication);

        public static string IsActiveItem(ViewContext context, string pageGroup)
            => (context.ViewData[KnownViewData.PageGroup] as string) == pageGroup ? "is-active" : string.Empty;
    }
}
