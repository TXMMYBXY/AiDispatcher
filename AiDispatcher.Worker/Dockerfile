FROM mcr.microsoft.com/dotnet/runtime:10.0 AS base
USER $APP_UID
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["AiDispatcher.Worker/AiDispatcher.Worker.csproj", "AiDispatcher.Worker/"]
RUN dotnet restore "AiDispatcher.Worker/AiDispatcher.Worker.csproj"
COPY . .
WORKDIR "/src/AiDispatcher.Worker"
RUN dotnet build "./AiDispatcher.Worker.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./AiDispatcher.Worker.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AiDispatcher.Worker.dll"]
