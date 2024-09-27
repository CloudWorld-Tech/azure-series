FROM mcr.microsoft.com/dotnet/runtime:8.0 AS base
USER $APP_UID
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["CloudWorld.Containers.Dapr.Producer/CloudWorld.Containers.Dapr.Producer.csproj", "CloudWorld.Containers.Dapr.Producer/"]
RUN dotnet restore "CloudWorld.Containers.Dapr.Producer/CloudWorld.Containers.Dapr.Producer.csproj"
COPY . .
WORKDIR "/src/CloudWorld.Containers.Dapr.Producer"
RUN dotnet build "CloudWorld.Containers.Dapr.Producer.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "CloudWorld.Containers.Dapr.Producer.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CloudWorld.Containers.Dapr.Producer.dll"]
