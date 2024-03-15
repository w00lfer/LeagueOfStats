using LeagueOfStats.Infrastructure.Options;
using LeagueOfStats.Jobs.Jobs;
using LeagueOfStats.Jobs.Jobs.JobListeners;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Quartz;
using Quartz.AspNetCore;

namespace LeagueOfStats.Jobs;

public static class DependencyInjection
{
    public static void AddJobsDI(this IServiceCollection services, IConfiguration configuration)
    {
        AddQuartz(services, configuration);
    }
    
    private static void AddQuartz(IServiceCollection services, IConfiguration configuration)
    {
        services.AddQuartz(q =>
        {
            q.UsePersistentStore(c =>
            {
                c.UseSqlServer(sqlServerOptions =>
                {
                    var dbConnectionString = configuration
                        .GetSection(nameof(DatabaseOptions))
                        .GetSection(nameof(DatabaseOptions.DatabaseConnectionString)).Value;
                    
                    var dbAdminPassword = configuration
                        .GetSection(nameof(DatabaseOptions))
                        .GetSection(nameof(DatabaseOptions.DatabaseAdminPassword)).Value;
                    
                    var connectionString = string.Format(dbConnectionString, dbAdminPassword);
                    
                    sqlServerOptions.ConnectionString = connectionString;
                });
                
                c.UseNewtonsoftJsonSerializer();
            });
            
            q.AddJobListener<JobFailureHandler>();
            
            string syncChampionAndSkiNdataAfterPatchJobKey = nameof(SyncChampionAndSkinDataJob);
            q.AddJob<SyncChampionAndSkinDataJob>(opts => opts
                .StoreDurably()
                .WithIdentity(syncChampionAndSkiNdataAfterPatchJobKey));

            q.AddTrigger(opts => opts
                .ForJob(syncChampionAndSkiNdataAfterPatchJobKey)
                .WithIdentity($"{syncChampionAndSkiNdataAfterPatchJobKey}-trigger")
                .StartAt(new DateTimeOffset(2024, 3, 6, 6, 0, 0, TimeSpan.Zero))
                .WithSchedule(CronScheduleBuilder
                    .WeeklyOnDayAndHourAndMinute(DayOfWeek.Tuesday, 6, 0)));
            
            string syncDiscountDataJobKey = nameof(SyncDiscountsDataJob);
            q.AddJob<SyncDiscountsDataJob>(opts => opts
                .StoreDurably()
                .WithIdentity(syncDiscountDataJobKey));

            q.AddTrigger(opts => opts
                .ForJob(syncDiscountDataJobKey)
                .WithIdentity($"{syncDiscountDataJobKey}-trigger")
                .StartAt(new DateTimeOffset(2024, 3, 10, 17, 0, 0, TimeSpan.Zero))
                .WithSchedule(CronScheduleBuilder
                    .WeeklyOnDayAndHourAndMinute(DayOfWeek.Monday, 17, 0)));
            
        });
        
        services.AddQuartzServer(options =>
        {
            options.WaitForJobsToComplete = true;
        });
    }
}