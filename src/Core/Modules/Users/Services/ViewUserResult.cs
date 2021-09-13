// Copyright (c) Leandro Silva Luz do Carmo
// SPDX-License-Identifier: GPL-3.0-or-later

namespace Nocturne.Auth.Core.Modules.Users.Services
{
    public sealed class ViewUserResult
    {
        public bool IsSuccess { get; private set; }

        public bool IsNotFound { get; private set; }

        public ViewUserItem User { get; private set; }

        public static ViewUserResult Success(ViewUserItem user)
        {
            return new ViewUserResult
            {
                IsSuccess = true,
                User = user,
            };
        }

        public static ViewUserResult NotFound()
        {
            return new ViewUserResult
            {
                IsNotFound = true,
            };
        }
    }
}
