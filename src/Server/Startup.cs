// Copyright (c) Leandro Silva Luz do Carmo
// SPDX-License-Identifier: GPL-3.0-or-later

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Nocturne.Auth.Configuration.Services;
using Nocturne.Auth.Server.Configuration;
using Nocturne.Auth.Server.Configuration.Constants;
using Nocturne.Auth.Server.Configuration.Options;

namespace Nocturne.Auth.Server
{
    public class Startup
    {
        public Startup(
            IConfiguration configuration,
            IWebHostEnvironment environment)
        {
            Configuration = configuration;
            Environment = environment;
        }

        public IConfiguration Configuration { get; }

        public IWebHostEnvironment Environment { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddApplicationDataProtection(Configuration);

            services
                .AddControllersWithViews()
                .AddApplicationMvcLocalization();

            services
                .AddRazorPages(options =>
                {
                    options.Conventions.AuthorizeAreaFolder("Identity", "/Account/Manage");
                })
                .AddApplicationMvcLocalization();

            services
                .AddApplicationCustomMvcValidationAttributes();

            services
                .AddApplicationAntiforgery(ApplicationConstants.Identifier)
                .AddApplicationWebAssets(Configuration)
                .AddApplicationOptions<ServerApplicationOptions>(Configuration);

            services.AddApplicationLocalization(Configuration);

            services
                .AddApplicationDbContexts(Configuration)
                .AddApplicationIdentity(Configuration, ApplicationConstants.Identifier)
                .AddIdentityEmails()
                .AddApplicationEmailService(Configuration, Environment)
                .AddApplicationModules()
                .AddRequiredApplicationServices(Configuration);

            services
                .AddApplicationOpenIddict()
                .AddApplicationServer(Configuration);
        }

        public void Configure(IApplicationBuilder app)
        {
            if (Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/error/unexpected");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });
        }
    }
}
