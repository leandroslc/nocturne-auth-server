// Copyright (c) Leandro Silva Luz do Carmo
// SPDX-License-Identifier: GPL-3.0-or-later

using Nocturne.Auth.Configuration.Options;

namespace Nocturne.Auth.Server.Configuration.Options
{
    public class ServerApplicationOptions : ApplicationOptions
    {
        public ServerApplicationOptions()
        {
            LoginBackground = new LoginBackgroundOptions();
        }

        public LoginBackgroundOptions LoginBackground { get; set; }
    }
}
