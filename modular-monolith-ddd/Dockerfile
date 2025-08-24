# RUNTIME STAGE
# Creates user, set permissions, mount volumes, and expose ports
## Uses Microsoft's ASP.NET 8.0 image (based on Ubuntu 22.04) as the runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0-jammy AS runtime

ARG APP_USER=app_admin
ARG APP_GROUP=app_group
ARG APP_HOME=/app

# Creates system group and system user
RUN addgroup --system $APP_GROUP && \
    adduser --system --ingroup $APP_GROUP --home $APP_HOME $APP_USER && \
    mkdir -p $APP_HOME && \
    chown -R $APP_USER:$APP_GROUP $APP_HOME
USER $APP_USER

# BUILD STAGE
# Copies source + restores + builds
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/API/ModularMonolithDDD.API/ModularMonolithDDD.API.csproj", "API/ModularMonolithDDD.API/"]
COPY ["src/BuildingBlocks/Common.Logging/Common.Logging.csproj", "BuildingBlocks/Common.Logging/"]
COPY ["src/BuildingBlocks/ModularMonolithDDD.BuildingBlocks.Domain/ModularMonolithDDD.BuildingBlocks.Domain.csproj", "BuildingBlocks/ModularMonolithDDD.BuildingBlocks.Domain/"]
COPY ["src/BuildingBlocks/ModularMonolithDDD.BuildingBlocks.Application/ModularMonolithDDD.BuildingBlocks.Application.csproj", "BuildingBlocks/ModularMonolithDDD.BuildingBlocks.Application/"]
COPY ["src/BuildingBlocks/ModularMonolithDDD.BuildingBlocks.Infrastructure/ModularMonolithDDD.BuildingBlocks.Infrastructure.csproj", "BuildingBlocks/ModularMonolithDDD.BuildingBlocks.Infrastructure/"]

RUN dotnet restore "API/ModularMonolithDDD.API/ModularMonolithDDD.API.csproj"
COPY src/ .
WORKDIR "/src/API/ModularMonolithDDD.API"
# RUN dotnet build "ModularMonolithDDD.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

# PUBLISH STAGE
# Publishes to /app/publish
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "ModularMonolithDDD.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# FINAL STAGE
# Copies the published files from /app/publish and runs
# Final image to run
FROM runtime AS final
WORKDIR $APP_HOME
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ModularMonolithDDD.API.dll"]