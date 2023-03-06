// Copyright (c) Leandro Silva Luz do Carmo
// SPDX-License-Identifier: GPL-3.0-or-later

namespace Nocturne.Auth.Server.Configuration.Options
{
    public class LoginBackgroundOptions
    {
        public Uri ImageUrl { get; set; }

        public string ImageAttribution { get; set; }

        public bool HasImageUrl => ImageUrl is not null;

        public bool HasImageAttribution => !string.IsNullOrWhiteSpace(ImageAttribution);
    }
}
