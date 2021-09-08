// Copyright (c) Leandro Silva Luz do Carmo
// SPDX-License-Identifier: GPL-3.0-or-later

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Nocturne.Auth.Admin.Configuration.Constants;
using Nocturne.Auth.Admin.Configuration.Services;
using Nocturne.Auth.Configuration.Services;

namespace Nocturne.Auth.Admin
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddApplicationDataProtection(Configuration);

            services
                .AddControllersWithViews()
                .AddApplicationMvcLocalization();

            services
                .AddApplicationCustomMvcValidationAttributes();

            services
                .AddApplicationAntiforgery(ApplicationConstants.Identifier)
                .AddApplicationWebAssets(Configuration)
                .AddWebApplicationOptions(Configuration);

            services
                .AddApplicationDbContexts(Configuration)
                .AddApplicationIdentityServicesOnly(Configuration)
                .AddApplicationLocalization(Configuration)
                .AddApplicationModules()
                .AddApplicationAuthentication(Configuration)
                .AddApplicationAccessControl()
                .AddApplicationAuthorization();

            services.AddApplicationOpenIddict();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
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
            });
        }
    }
}
