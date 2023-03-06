# Build
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build

WORKDIR /app

COPY . .

WORKDIR /app/src/Server
RUN dotnet restore
RUN dotnet publish -c Release -o ../../publish

# Run
FROM mcr.microsoft.com/dotnet/aspnet:6.0

WORKDIR /app
COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "Nocturne.Auth.Server.dll"]
