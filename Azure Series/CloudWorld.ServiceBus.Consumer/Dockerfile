FROM mcr.microsoft.com/dotnet/runtime:8.0 AS base
USER $APP_UID
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["CloudWorld.ServiceBus.Consumer/CloudWorld.ServiceBus.Consumer.csproj", "CloudWorld.ServiceBus.Consumer/"]
RUN dotnet restore "CloudWorld.ServiceBus.Consumer/CloudWorld.ServiceBus.Consumer.csproj"
COPY . .
WORKDIR "/src/CloudWorld.ServiceBus.Consumer"
RUN dotnet build "CloudWorld.ServiceBus.Consumer.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "CloudWorld.ServiceBus.Consumer.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CloudWorld.ServiceBus.Consumer.dll"]
