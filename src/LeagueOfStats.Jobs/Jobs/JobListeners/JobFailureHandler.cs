using Quartz;

namespace LeagueOfStats.Jobs.Jobs.JobListeners;

public class JobFailureHandler : IJobListener
{
    public string Name => "FailJobListener";

    public Task JobToBeExecuted(IJobExecutionContext context, CancellationToken ct)
    {
        if (!context.JobDetail.JobDataMap.Contains(JobListenerConstants.NumTriesKey))
        {
            context.JobDetail.JobDataMap.Put(JobListenerConstants.NumTriesKey, 0);
        }

        var numberTries = context.JobDetail.JobDataMap.GetIntValue(JobListenerConstants.NumTriesKey);
        context.JobDetail.JobDataMap.Put(JobListenerConstants.NumTriesKey, ++numberTries);

        return Task.CompletedTask;
    }

    public Task JobExecutionVetoed(IJobExecutionContext context, CancellationToken ct) => 
        Task.CompletedTask;

    public async Task JobWasExecuted(IJobExecutionContext context, JobExecutionException jobException, CancellationToken ct)
    {
        if (jobException == null)
        {
            return;
        }

        var numTries = context.JobDetail.JobDataMap.GetIntValue(JobListenerConstants.NumTriesKey);

        if (numTries > JobListenerConstants.MaxRetries)
        {
            Console.WriteLine($"Job with ID and type: {0}, {1} has run {2} times and has failed each time.",
                context.JobDetail.Key, context.JobDetail.JobType, JobListenerConstants.MaxRetries);

            return;
        }
        
        var trigger = TriggerBuilder
            .Create()
            .WithIdentity($"{Guid.NewGuid().ToString()}-trigger")
            .StartAt(DateTime.Now.AddMinutes(JobListenerConstants.WaitIntervalInMinutes * numTries))
            .Build();

        Console.WriteLine($"Job with ID and type: {0}, {1} has thrown the exception: {2}. Running again in {3} minutes.",
            context.JobDetail.Key, context.JobDetail.JobType, jobException, JobListenerConstants.WaitIntervalInMinutes * numTries);

        await context.Scheduler.RescheduleJob(context.Trigger.Key, trigger);
    }
}