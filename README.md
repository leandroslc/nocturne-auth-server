
<h1 align="center">Nocturne Auth Server</h1>
<p align="center">An Open ID and OAuth Authorization Server and Administration</p>

## Requirements
- Make sure you have [.NET 5 SDK](https://dotnet.microsoft.com/download) installed.
- Make sure you have at least [SQL Server Express](https://www.microsoft.com/pt-br/sql-server/sql-server-downloads). In the future other databases may be supported.

## Initialization
To setup the server administration and admin user, navigate to `src/Admin` and run `dotnet run --init`.

The default admin user name and password are set in `initialize.json`.

### Initialization configuration
The configuration for setup can be changed before running the initialization.

In a production environment it is highly recommended to change:
- The `Encryption/Key` in `appsettings.json` (can different by environment).
- The `AdminUser` password and username in `initialize.json` (or you can change afterwards).
- The `AdminApplication`'s `ClientId`, `ClientSecret`, `RedirectUri` and others in `initialize.json`.

## Web assets
Some of the web assets, like javascript and css, are shared and served from a remote server, so you must also configure an [Impromptu](https://github.com/leandrolc/impromptu) static server.
