FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["src/N5NOW.UserPermissions.API/N5NOW.UserPermissions.API.csproj", "src/N5NOW.UserPermissions.API/"]
COPY ["src/N5NOW.UserPermissions.Application/N5NOW.UserPermissions.Application.csproj", "src/N5NOW.UserPermissions.Application/"]
COPY ["src/N5NOW.UserPermissions.Domain/N5NOW.UserPermissions.Domain.csproj", "src/N5NOW.UserPermissions.Domain/"]
COPY ["src/N5NOW.UserPermissions.Infrastructure/N5NOW.UserPermissions.Infrastructure.csproj", "src/N5NOW.UserPermissions.Infrastructure/"]

RUN dotnet restore "src/N5NOW.UserPermissions.API/N5NOW.UserPermissions.API.csproj"
COPY . .
WORKDIR "/src/src/N5NOW.UserPermissions.API"
RUN dotnet build "N5NOW.UserPermissions.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "N5NOW.UserPermissions.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

EXPOSE 443
EXPOSE 80
ENTRYPOINT ["dotnet", "N5NOW.UserPermissions.API.dll"]