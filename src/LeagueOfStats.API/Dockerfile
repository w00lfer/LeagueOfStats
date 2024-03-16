FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["src/LeagueOfStats.API/LeagueOfStats.API.csproj", "src/LeagueOfStats.API/"]
COPY ["src/LeagueOfStats.Application/LeagueOfStats.Application.csproj", "src/LeagueOfStats.Application/"]
COPY ["src/LeagueOfStats.Domain/LeagueOfStats.Domain.csproj", "src/LeagueOfStats.Domain/"]
COPY ["src/LeagueOfStats.Infrastructure/LeagueOfStats.Infrastructure.csproj", "src/LeagueOfStats.Infrastructure/"]
COPY ["src/LeagueOfStats.Jobs/LeagueOfStats.Jobs.csproj", "src/LeagueOfStats.Jobs/"]
RUN dotnet restore "src/LeagueOfStats.API/LeagueOfStats.API.csproj"
COPY . .
WORKDIR "/src/src/LeagueOfStats.API"
RUN dotnet build "LeagueOfStats.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "LeagueOfStats.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "LeagueOfStats.API.dll"]
