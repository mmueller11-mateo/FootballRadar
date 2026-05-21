# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app

# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copy solution and all project files for restore
COPY ["FootballRadar.sln", "./"]
COPY ["FootballRadar.WebApp/FootballRadar.WebApp.csproj", "FootballRadar.WebApp/"]
COPY ["FootballRadar.Business.Services/FootballRadar.Business.Services.csproj", "FootballRadar.Business.Services/"]
COPY ["FootballRadar.Business.Entities/FootballRadar.Business.Entities.csproj", "FootballRadar.Business.Entities/"]
COPY ["FootballRadar.Business.ViewModels/FootballRadar.Business.ViewModels.csproj", "FootballRadar.Business.ViewModels/"]
COPY ["FootballRadar.Data/FootballRadar.Data.csproj", "FootballRadar.Data/"]
COPY ["FootballRadar.EventHandling/FootballRadar.EventHandling.csproj", "FootballRadar.EventHandling/"]
COPY ["FootballRadar.Web.Utilities/FootballRadar.Web.Utilities.csproj", "FootballRadar.Web.Utilities/"]
COPY ["FootballRadar.Abstractions/FootballRadar.Abstractions.csproj", "FootballRadar.Abstractions/"]
COPY ["My.Framework.EventHandling/My.Framework.EventHandling.csproj", "My.Framework.EventHandling/"]
COPY ["FootballRadar.Admin.Abstractions/FootballRadar.Admin.Abstractions.csproj", "FootballRadar.Admin.Abstractions/"]
COPY ["FootballRadar.Admin.Business.Services/FootballRadar.Admin.Business.Services.csproj", "FootballRadar.Admin.Business.Services/"]
COPY ["FootballRadar.Admin.Data/FootballRadar.Admin.Data.csproj", "FootballRadar.Admin.Data/"]
COPY ["FootballRadar.Admin.WebApp/FootballRadar.Admin.WebApp.csproj", "FootballRadar.Admin.WebApp/"]
COPY ["FootballRadar.Admin.UnitTests/FootballRadar.Admin.UnitTests.csproj", "FootballRadar.Admin.UnitTests/"]
COPY ["FootballRadar.UnitTests/FootballRadar.UnitTests.csproj", "FootballRadar.UnitTests/"]
COPY ["FootballRadar.DataCollector/FootballRadar.DataCollector.csproj", "FootballRadar.DataCollector/"]
COPY ["FootballRadar.DataCollector/FootballRadar.DataCollector.ApiSports.csproj", "FootballRadar.DataCollector/"]
COPY ["FootballRadar.DataCollector.Transfermarkt/FootballRadar.DataCollector.Kaggle.csproj", "FootballRadar.DataCollector.Transfermarkt/"]
COPY ["FootballRadar.TippSpiel.WebApp/FootballRadar.TippSpiel.WebApp.csproj", "FootballRadar.TippSpiel.WebApp/"]
RUN dotnet restore "./FootballRadar.WebApp/FootballRadar.WebApp.csproj"

COPY . .
WORKDIR "/src/FootballRadar.WebApp"
RUN dotnet build "./FootballRadar.WebApp.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./FootballRadar.WebApp.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

# Railway provides PORT dynamically; ensure ASP.NET listens on it
ENV ASPNETCORE_URLS=http://0.0.0.0:${PORT:-8080}

ENTRYPOINT ["dotnet", "FootballRadar.WebApp.dll"]
