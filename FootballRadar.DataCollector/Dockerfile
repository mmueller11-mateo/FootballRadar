FROM mcr.microsoft.com/dotnet/runtime:10.0 AS base
USER $APP_UID
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

COPY ["FootballRadar.DataCollector/FootballRadar.DataCollector.csproj", "FootballRadar.DataCollector/"]
COPY ["FootballRadar.Business.Entities/FootballRadar.Business.Entities.csproj", "FootballRadar.Business.Entities/"]

RUN dotnet restore "./FootballRadar.DataCollector/FootballRadar.DataCollector.csproj"

COPY FootballRadar.DataCollector/ FootballRadar.DataCollector/
COPY FootballRadar.Business.Entities/ FootballRadar.Business.Entities/

WORKDIR "/src/FootballRadar.DataCollector"
RUN dotnet publish "./FootballRadar.DataCollector.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "FootballRadar.DataCollector.dll"]