FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# .csproj Dateien kopieren (Layer-Caching für restore)
COPY ["FootballRadar.WebApp/FootballRadar.WebApp.csproj", "FootballRadar.WebApp/"]
COPY ["FootballRadar.Business.Entities/FootballRadar.Business.Entities.csproj", "FootballRadar.Business.Entities/"]
COPY ["FootballRadar.Business.ViewModels/FootballRadar.Business.ViewModels.csproj", "FootballRadar.Business.ViewModels/"]
COPY ["FootballRadar.Business.Services/FootballRadar.Business.Services.csproj", "FootballRadar.Business.Services/"]
COPY ["FootballRadar.Abstractions/FootballRadar.Abstractions.csproj", "FootballRadar.Abstractions/"]
COPY ["FootballRadar.Data/FootballRadar.Data.csproj", "FootballRadar.Data/"]
COPY ["FootballRadar.EventHandling/FootballRadar.EventHandling.csproj", "FootballRadar.EventHandling/"]
COPY ["FootballRadar.Web.Utilities/FootballRadar.Web.Utilities.csproj", "FootballRadar.Web.Utilities/"]
COPY ["My.Framework.EventHandling/My.Framework.EventHandling.csproj", "My.Framework.EventHandling/"]

RUN dotnet restore "./FootballRadar.WebApp/FootballRadar.WebApp.csproj"

# Nur nötige Source-Ordner kopieren
COPY FootballRadar.WebApp/ FootballRadar.WebApp/
COPY FootballRadar.Business.Entities/ FootballRadar.Business.Entities/
COPY FootballRadar.Business.ViewModels/ FootballRadar.Business.ViewModels/
COPY FootballRadar.Business.Services/ FootballRadar.Business.Services/
COPY FootballRadar.Abstractions/ FootballRadar.Abstractions/
COPY FootballRadar.Data/ FootballRadar.Data/
COPY FootballRadar.EventHandling/ FootballRadar.EventHandling/
COPY FootballRadar.Web.Utilities/ FootballRadar.Web.Utilities/
COPY My.Framework.EventHandling/ My.Framework.EventHandling/

WORKDIR "/src/FootballRadar.WebApp"
RUN dotnet publish "./FootballRadar.WebApp.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "FootballRadar.WebApp.dll"]