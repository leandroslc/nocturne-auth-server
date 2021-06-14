using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Nocturne.Auth.Configuration.Services;
using Nocturne.Auth.Server.Configuration;
using Nocturne.Auth.Server.Configuration.Constants;

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
                .AddApplicationAntiforgery(ApplicationConstants.Identifier)
                .AddApplicationWebAssets(Configuration);

            services.AddApplicationLocalization(Configuration);

            services
                .AddApplicationDbContexts(Configuration)
                .AddApplicationIdentity(Configuration, ApplicationConstants.Identifier)
                .AddApplicationEmail(Configuration)
                .AddApplicationEncryption(Configuration)
                .AddApplicationModules();

            services
                .AddApplicationOpenIddict()
                .AddApplicationServer(Environment);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
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
