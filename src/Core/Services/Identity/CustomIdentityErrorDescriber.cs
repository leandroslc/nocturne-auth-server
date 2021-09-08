// Copyright (c) Leandro Silva Luz do Carmo
// SPDX-License-Identifier: GPL-3.0-or-later

using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Localization;

namespace Nocturne.Auth.Core.Services.Identity
{
    public class CustomIdentityErrorDescriber : IdentityErrorDescriber
    {
        private readonly IStringLocalizer localizer;

        public CustomIdentityErrorDescriber(
            IStringLocalizer<CustomIdentityErrorDescriber> localizer)
        {
            this.localizer = localizer;
        }

        public override IdentityError ConcurrencyFailure()
            => Error(
                nameof(ConcurrencyFailure),
                "Concurrency failure, the resource has been modified");

        public override IdentityError DefaultError()
            => Error(
                nameof(DefaultError),
                "An unknown failure has occurred");

        public override IdentityError DuplicateEmail(string email)
            => Error(
                nameof(DuplicateEmail),
                "The email '{0}' is already taken", email);

        public override IdentityError DuplicateRoleName(string role)
            => Error(
                nameof(DuplicateRoleName),
                "Role name '{0}' is already taken", role);

        public override IdentityError DuplicateUserName(string userName)
            => Error(
                nameof(DuplicateUserName),
                "Username '{0}' is already taken", userName);

        public override IdentityError InvalidEmail(string email)
            => Error(
                nameof(InvalidEmail),
                "Email '{0}' is invalid", email);

        public override IdentityError InvalidRoleName(string role)
            => Error(
                nameof(InvalidRoleName),
                "Role name '{0}' is invalid", role);

        public override IdentityError InvalidToken()
            => Error(
                nameof(InvalidToken),
                "Invalid token");

        public override IdentityError InvalidUserName(string userName)
            => Error(
                nameof(InvalidUserName),
                "Username '{0}' can only contain letters or digits", userName);

        public override IdentityError LoginAlreadyAssociated()
            => Error(
                nameof(LoginAlreadyAssociated),
                "A user with this login already exists");

        public override IdentityError PasswordMismatch()
            => Error(
                nameof(PasswordMismatch),
                "Incorrect password");

        public override IdentityError PasswordRequiresDigit()
            => Error(
                nameof(PasswordRequiresDigit),
                "The password must have at least one digit (0-9)");

        public override IdentityError PasswordRequiresLower()
            => Error(
                nameof(PasswordRequiresLower),
                "The password must have at least one lowercase letter (a-z)");

        public override IdentityError PasswordRequiresNonAlphanumeric()
            => Error(
                nameof(PasswordRequiresNonAlphanumeric),
                "The password must have at least one non alphanumeric character");

        public override IdentityError PasswordRequiresUniqueChars(int uniqueChars)
            => Error(
                nameof(PasswordRequiresUniqueChars),
                "The password must use at least {0} different characters", uniqueChars);

        public override IdentityError PasswordRequiresUpper()
            => Error(
                nameof(PasswordRequiresUpper),
                "The password must have at least one uppercase letter (A-Z)");

        public override IdentityError PasswordTooShort(int length)
            => Error(
                nameof(PasswordTooShort),
                "The password must be at least {0} characters", length);

        public override IdentityError RecoveryCodeRedemptionFailed()
            => Error(
                nameof(RecoveryCodeRedemptionFailed),
                "Recovery code redemption failure");

        public override IdentityError UserAlreadyHasPassword()
            => Error(
                nameof(UserAlreadyHasPassword),
                "User already has a password set");

        public override IdentityError UserAlreadyInRole(string role)
            => Error(
                nameof(UserAlreadyInRole),
                "User already in role '{0}'", role);

        public override IdentityError UserLockoutNotEnabled()
            => Error(
                nameof(UserLockoutNotEnabled),
                "Lockout is not enabled for this user");

        public override IdentityError UserNotInRole(string role)
            => Error(
                nameof(UserNotInRole),
                "User is not in role '{0}'", role);

        private IdentityError Error(
            string name,
            string description,
            params object[] parameters)
        {
            return new IdentityError
            {
                Code = name,
                Description = localizer[description, parameters],
            };
        }
    }
}
