# RUNTIME STAGE
# Creates user, set permissions, mount volumes, and expose ports
## Uses Microsoft's ASP.NET 9.0 image as the runtime image
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime

# Define Linux system user arguments for container security
# Creates non-root user to avoid running as root
ARG APP_USER=app_admin
ARG APP_GROUP=app_group
ARG APP_HOME=/app

# Creates non-root Linux system group, non-root Linux system user
RUN addgroup --system $APP_GROUP && \
    adduser --system --ingroup $APP_GROUP --home $APP_HOME $APP_USER && \
    mkdir -p $APP_HOME && \
    chown -R $APP_USER:$APP_GROUP $APP_HOME

# Switch to non-root user for application execution
USER $APP_USER

# BUILD STAGE
# Copies source + restores + builds
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /

# Copy solution and project files for dependency restoration
COPY ModularMonolithDDD.sln Directory.Packages.props Directory.Build.targets ./
COPY ["modular-monolith-ddd/src/API/ModularMonolithDDD.API/ModularMonolithDDD.API.csproj", "modular-monolith-ddd/src/API/ModularMonolithDDD.API/"]
COPY ["modular-monolith-ddd/src/BuildingBlocks/ModularMonolithDDD.BuildingBlocks.Domain/ModularMonolithDDD.BuildingBlocks.Domain.csproj", "modular-monolith-ddd/src/BuildingBlocks/ModularMonolithDDD.BuildingBlocks.Domain/"]
COPY ["modular-monolith-ddd/src/BuildingBlocks/ModularMonolithDDD.BuildingBlocks.Application/ModularMonolithDDD.BuildingBlocks.Application.csproj", "modular-monolith-ddd/src/BuildingBlocks/ModularMonolithDDD.BuildingBlocks.Application/"]
COPY ["modular-monolith-ddd/src/BuildingBlocks/ModularMonolithDDD.BuildingBlocks.Infrastructure/ModularMonolithDDD.BuildingBlocks.Infrastructure.csproj", "modular-monolith-ddd/src/BuildingBlocks/ModularMonolithDDD.BuildingBlocks.Infrastructure/"]
COPY ["modular-monolith-ddd/src/BuildingBlocks/ModularMonolithDDD.BuildingBlocks.Logging/ModularMonolithDDD.BuildingBlocks.Logging.csproj", "modular-monolith-ddd/src/BuildingBlocks/ModularMonolithDDD.BuildingBlocks.Logging/"]
COPY ["modular-monolith-ddd/src/Modules/UserAccess/ModularMonolithDDD.Modules.UserAccess.Domain/ModularMonolithDDD.Modules.UserAccess.Domain.csproj", "modular-monolith-ddd/src/Modules/UserAccess/ModularMonolithDDD.Modules.UserAccess.Domain/"]
COPY ["modular-monolith-ddd/src/Modules/UserAccess/ModularMonolithDDD.Modules.UserAccess.Application/ModularMonolithDDD.Modules.UserAccess.Application.csproj", "modular-monolith-ddd/src/Modules/UserAccess/ModularMonolithDDD.Modules.UserAccess.Application/"]
COPY ["modular-monolith-ddd/src/Modules/UserAccess/ModularMonolithDDD.Modules.UserAccess.Infrastructure/ModularMonolithDDD.Modules.UserAccess.Infrastructure.csproj", "modular-monolith-ddd/src/Modules/UserAccess/ModularMonolithDDD.Modules.UserAccess.Infrastructure/"]
COPY ["modular-monolith-ddd/src/Modules/UserAccess/ModularMonolithDDD.Modules.UserAccess.IntegrationEvents/ModularMonolithDDD.Modules.UserAccess.IntegrationEvents.csproj", "modular-monolith-ddd/src/Modules/UserAccess/ModularMonolithDDD.Modules.UserAccess.IntegrationEvents/"]

# Restore NuGet packages for each project (production only)
RUN dotnet restore modular-monolith-ddd/src/BuildingBlocks/ModularMonolithDDD.BuildingBlocks.Domain/ModularMonolithDDD.BuildingBlocks.Domain.csproj
RUN dotnet restore modular-monolith-ddd/src/BuildingBlocks/ModularMonolithDDD.BuildingBlocks.Application/ModularMonolithDDD.BuildingBlocks.Application.csproj
RUN dotnet restore modular-monolith-ddd/src/BuildingBlocks/ModularMonolithDDD.BuildingBlocks.Infrastructure/ModularMonolithDDD.BuildingBlocks.Infrastructure.csproj
RUN dotnet restore modular-monolith-ddd/src/BuildingBlocks/ModularMonolithDDD.BuildingBlocks.Logging/ModularMonolithDDD.BuildingBlocks.Logging.csproj
RUN dotnet restore modular-monolith-ddd/src/Modules/UserAccess/ModularMonolithDDD.Modules.UserAccess.Domain/ModularMonolithDDD.Modules.UserAccess.Domain.csproj
RUN dotnet restore modular-monolith-ddd/src/Modules/UserAccess/ModularMonolithDDD.Modules.UserAccess.Application/ModularMonolithDDD.Modules.UserAccess.Application.csproj
RUN dotnet restore modular-monolith-ddd/src/Modules/UserAccess/ModularMonolithDDD.Modules.UserAccess.Infrastructure/ModularMonolithDDD.Modules.UserAccess.Infrastructure.csproj
RUN dotnet restore modular-monolith-ddd/src/Modules/UserAccess/ModularMonolithDDD.Modules.UserAccess.IntegrationEvents/ModularMonolithDDD.Modules.UserAccess.IntegrationEvents.csproj
RUN dotnet restore modular-monolith-ddd/src/API/ModularMonolithDDD.API/ModularMonolithDDD.API.csproj

# Copy source code for build
COPY modular-monolith-ddd/src/API ./modular-monolith-ddd/src/API
COPY modular-monolith-ddd/src/BuildingBlocks ./modular-monolith-ddd/src/BuildingBlocks
COPY modular-monolith-ddd/src/Modules ./modular-monolith-ddd/src/Modules

# PUBLISH STAGE
# Publishes to /app/publish
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
WORKDIR /
RUN dotnet publish "modular-monolith-ddd/src/API/ModularMonolithDDD.API/ModularMonolithDDD.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# FINAL STAGE
# Copies the published files from /app/publish and runs
# Final image to run
FROM runtime AS final
WORKDIR $APP_HOME
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ModularMonolithDDD.API.dll"]