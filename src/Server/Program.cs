// Copyright (c) Leandro Silva Luz do Carmo
// SPDX-License-Identifier: GPL-3.0-or-later

using Nocturne.Auth.Configuration.Services;
using Nocturne.Auth.Server.Configuration;
using Nocturne.Auth.Server.Configuration.Constants;
using Nocturne.Auth.Server.Configuration.Options;

var builder = WebApplication.CreateBuilder(args);

builder
    .AddLocalSettings()
    .AddLogging();

var services = builder.Services;
var configuration = builder.Configuration;
var environment = builder.Environment;

services.AddApplicationDataProtection(configuration);

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
    .AddApplicationWebAssets(configuration)
    .AddApplicationOptions<ServerApplicationOptions>(configuration);

services.AddApplicationLocalization(configuration);

services
    .AddApplicationDbContexts(configuration)
    .AddApplicationHealthChecks()
    .AddApplicationIdentity(configuration, ApplicationConstants.Identifier)
    .AddIdentityEmails()
    .AddApplicationEmailService(configuration, environment)
    .AddApplicationModules()
    .AddRequiredApplicationServices(configuration);

services
    .AddApplicationOpenIddict()
    .AddApplicationServer(configuration);

var app = builder.Build();

if (environment.IsDevelopment())
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

app.MapControllers();
app.MapRazorPages();
app.MapHealthChecks("/health");

app.Run();
