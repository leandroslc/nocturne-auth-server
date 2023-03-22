# Build
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build

WORKDIR /app

COPY . .

WORKDIR /app/src/Admin
RUN dotnet restore
RUN dotnet publish -c Release -o ../../publish

# Run
FROM mcr.microsoft.com/dotnet/aspnet:6.0

WORKDIR /app
COPY --from=build /app/publish .

EXPOSE 80
EXPOSE 443

ENTRYPOINT ["dotnet", "Nocturne.Auth.Admin.dll"]
