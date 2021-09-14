// Copyright (c) Leandro Silva Luz do Carmo
// SPDX-License-Identifier: GPL-3.0-or-later

namespace Nocturne.Auth.Server.Configuration
{
    public static class AuthorizationEndpoints
    {
        public const string Authorize = "/connect/authorize";

        public const string Intrspect = "/connect/introspect";

        public const string Logout = "/connect/logout";

        public const string Token = "/connect/token";

        public const string UserInfo = "/connect/userinfo";
    }
}
