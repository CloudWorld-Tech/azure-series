FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["CloudWorld.ServiceBus.Producer/CloudWorld.ServiceBus.Producer.csproj", "CloudWorld.ServiceBus.Producer/"]
RUN dotnet restore "CloudWorld.ServiceBus.Producer/CloudWorld.ServiceBus.Producer.csproj"
COPY . .
WORKDIR "/src/CloudWorld.ServiceBus.Producer"
RUN dotnet build "CloudWorld.ServiceBus.Producer.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "CloudWorld.ServiceBus.Producer.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CloudWorld.ServiceBus.Producer.dll"]
