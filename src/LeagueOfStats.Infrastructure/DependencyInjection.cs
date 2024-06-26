using LeagueOfStats.Domain.Champions;
using LeagueOfStats.Domain.Common.Repositories;
using LeagueOfStats.Domain.Discounts;
using LeagueOfStats.Domain.Matches;
using LeagueOfStats.Domain.Skins;
using LeagueOfStats.Domain.Summoners;
using LeagueOfStats.Infrastructure.ApplicationDbContexts;
using LeagueOfStats.Infrastructure.Champions;
using LeagueOfStats.Infrastructure.Common;
using LeagueOfStats.Infrastructure.Discounts;
using LeagueOfStats.Infrastructure.Matches;
using LeagueOfStats.Infrastructure.Options;
using LeagueOfStats.Infrastructure.Skins;
using LeagueOfStats.Infrastructure.Summoners;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace LeagueOfStats.Infrastructure;

    public static class DependencyInjection
    {
        public static void AddInfrastructureDI(this IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IChampionRepository, ChampionRepository>();
            services.AddScoped<ISummonerRepository, SummonerRepository>();
            services.AddScoped<IMatchRepository, MatchRepository>();
            services.AddScoped<ISkinRepository, SkinRepository>();
            services.AddScoped<IDiscountRepository, DiscountRepository>();

            AddDb(services);
        }

        private static void AddDb(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>((serviceProvider, optionsBuilder) =>
            {
                var databaseOptions = serviceProvider.GetService<IOptions<DatabaseOptions>>().Value;

                var connectionString = string.Format(
                    databaseOptions.DatabaseConnectionString,
                    databaseOptions.DatabaseAdminPassword);
                
                optionsBuilder.UseSqlServer(connectionString, sqlServerOptionsAction =>
                {
                    sqlServerOptionsAction.UseNodaTime();

                    sqlServerOptionsAction.CommandTimeout(databaseOptions.CommandTimeout);
                });

                optionsBuilder.EnableDetailedErrors(databaseOptions.EnableDetailedErrors);

                optionsBuilder.EnableSensitiveDataLogging(databaseOptions.EnablesSensitiveDataLogging);
            });
        }
    }