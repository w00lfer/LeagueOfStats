FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["LeagueOfStats.API/LeagueOfStats.API.csproj", "LeagueOfStats.API/"]
COPY ["LeagueOfStats.Application/LeagueOfStats.Application.csproj", "LeagueOfStats.Application/"]
COPY ["LeagueOfStats.Domain/LeagueOfStats.Domain.csproj", "LeagueOfStats.Domain/"]
COPY ["LeagueOfStats.Infrastructure/LeagueOfStats.Infrastructure.csproj", "LeagueOfStats.Infrastructure/"]
RUN dotnet restore "LeagueOfStats.API/LeagueOfStats.API.csproj"
COPY . .
WORKDIR "/src/LeagueOfStats.API"
RUN dotnet build "LeagueOfStats.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "LeagueOfStats.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "LeagueOfStats.API.dll"]
