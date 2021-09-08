// Copyright (c) Leandro Silva Luz do Carmo
// SPDX-License-Identifier: GPL-3.0-or-later

using System;
using System.IO;
using FluentEmail.MailKitSmtp;
using MailKit.Security;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Nocturne.Auth.Core.Services.Email;

namespace Nocturne.Auth.Configuration.Services
{
    public static class EmailServices
    {
        public static IServiceCollection AddApplicationEmail(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.Configure<EmailOptions>(configuration.GetSection(EmailOptions.Section));

            services.AddSingleton<IEmailService, EmailService>();

            return services;
        }

        public static IServiceCollection AddApplicationEmailService(
            this IServiceCollection services,
            IConfiguration configuration,
            IWebHostEnvironment environment)
        {
            var emailOptions = GetEmailOptions(configuration);
            var emailSettings = GetEmailSettings(emailOptions, environment);

            services.AddSingleton(emailSettings);
            services.AddSingleton(emailOptions);

            services.AddFluentEmailInternal(emailOptions, emailSettings);

            services.AddTransient<IEmailService, EmailService>();

            return services;
        }

        private static void AddFluentEmailInternal(
            this IServiceCollection services,
            EmailOptions emailOptions,
            EmailSettings emailSettings)
        {
            services
                .AddFluentEmail(
                    defaultFromEmail: emailOptions.SenderEmail,
                    defaultFromName: emailOptions.SenderName)
                .AddLiquidRenderer(options =>
                {
                    options.FileProvider = new PhysicalFileProvider(emailSettings.TemplatesPath);
                })
                .AddMailKitSender(GetSmtpOptions(emailOptions));
        }

        private static EmailOptions GetEmailOptions(IConfiguration configuration)
        {
            var options = new EmailOptions();

            configuration.GetSection(EmailOptions.Section).Bind(options);

            return options;
        }

        private static EmailSettings GetEmailSettings(
            EmailOptions options,
            IWebHostEnvironment environment)
        {
            var templatesPath = Path.Combine(
                environment.ContentRootPath,
                options.TemplatesPath);

            return new EmailSettings(
                templatesPath,
                templateExtension: "liquid");
        }

        private static SmtpClientOptions GetSmtpOptions(EmailOptions options)
        {
            return new SmtpClientOptions
            {
                Server = options.Host,
                Port = options.Port,
                UseSsl = options.UseSSL,
                Password = options.Password,
                User = options.Login,
                RequiresAuthentication = options.RequiresAuthentication,
                SocketOptions = ParseSecurityOption(options.Security),
            };
        }

        private static SecureSocketOptions ParseSecurityOption(string value)
        {
            Enum.TryParse<SecureSocketOptions>(value, ignoreCase: true, out var security);

            return security;
        }
    }
}
