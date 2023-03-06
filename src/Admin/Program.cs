// Copyright (c) Leandro Silva Luz do Carmo
// SPDX-License-Identifier: GPL-3.0-or-later

using Nocturne.Auth.Admin.Configuration.Constants;
using Nocturne.Auth.Admin.Configuration.Options;
using Nocturne.Auth.Admin.Configuration.Services;
using Nocturne.Auth.Admin.Services.Initialization;
using Nocturne.Auth.Configuration.Services;

if (HasInitializationArg(args))
{
    var initializationBuilder = WebApplication.CreateBuilder(args);
}

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
    .AddApplicationCustomMvcValidationAttributes();

services
    .AddApplicationAntiforgery(ApplicationConstants.Identifier)
    .AddApplicationWebAssets(configuration)
    .AddApplicationOptions<AdminApplicationOptions>(configuration);

services
    .AddApplicationDbContexts(configuration)
    .AddApplicationHealthChecks()
    .AddApplicationIdentityServicesOnly(configuration)
    .AddApplicationLocalization(configuration)
    .AddApplicationModules()
    .AddApplicationAuthentication(configuration)
    .AddApplicationAccessControl()
    .AddApplicationAuthorization();

services.AddApplicationOpenIddict();

if (HasInitializationArg(args))
{
    builder.Configuration.AddJsonFile("initialize.json", optional: false);

    var initialization = builder.Build();

    InitializationService.Run(initialization.Services);

    return;
}

var app = builder.Build();

if (environment.IsDevelopment())
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

app.MapControllers();
app.MapRazorPages();
app.MapHealthChecks("/health");

app.Run();

static bool HasInitializationArg(string[] args)
{
    return args.Any(arg => string.Equals(arg, "--init", StringComparison.OrdinalIgnoreCase));
}
