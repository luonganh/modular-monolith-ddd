namespace ModularMonolithDDD.Modules.UserAccess.Infrastructure.Configuration.Quartz
{
    /// <summary>
    /// Static class responsible for initializing and configuring Quartz.NET scheduler for the UserAccess module.
    /// This class sets up scheduled jobs for processing outbox messages, inbox messages, and internal commands
    /// with configurable polling intervals or default cron schedules.
    /// </summary>
    public static class QuartzStartup
    {
        /// <summary>
        /// Initializes the Quartz.NET scheduler with all required jobs for the UserAccess module.
        /// Configures and schedules jobs for outbox processing, inbox processing, and internal command processing.
        /// </summary>
        /// <param name="logger">The Serilog logger instance for logging scheduler operations.</param>
        /// <param name="internalProcessingPoolingInterval">Optional polling interval in milliseconds for internal processing jobs. If null, uses default cron schedule.</param>
		public static void Initialize(Serilog.ILogger logger, long? internalProcessingPoolingInterval = null)
        {            
            logger.Information("Quartz starting...");

            // Configure Quartz scheduler with custom instance name
            var schedulerConfiguration = new NameValueCollection();
            schedulerConfiguration.Add("quartz.scheduler.instanceName", "UserAccess");

            // Create and start the scheduler
            ISchedulerFactory schedulerFactory = new StdSchedulerFactory(schedulerConfiguration);
            IScheduler scheduler = schedulerFactory.GetScheduler().GetAwaiter().GetResult();

            // Configure Serilog as the logging provider for Quartz
            LogProvider.SetCurrentLogProvider(new SerilogLogProvider(logger));

            // Start the scheduler
            scheduler.Start().GetAwaiter().GetResult();

            // Create and schedule the outbox processing job
            var processOutboxJob = JobBuilder.Create<ProcessOutboxJob>().Build();
            ITrigger trigger;
            if (internalProcessingPoolingInterval.HasValue)
            {
                // Use custom polling interval if provided
                trigger =
					TriggerBuilder
                        .Create()
                        .StartNow()
                        .WithSimpleSchedule(x =>
                            x.WithInterval(TimeSpan.FromMilliseconds(internalProcessingPoolingInterval.Value))
                                .RepeatForever())
                        .Build();
            }
            else
            {
                // Use default cron schedule (every 2 seconds)
                trigger =
					TriggerBuilder
                        .Create()
                        .StartNow()
                        .WithCronSchedule("0/2 * * ? * *")
                        .Build();
            }

            // Schedule the outbox processing job
            scheduler
                .ScheduleJob(processOutboxJob, trigger)
                .GetAwaiter().GetResult();

            // Create and schedule the inbox processing job
            var processInboxJob = JobBuilder.Create<ProcessInboxJob>().Build();

            ITrigger processInboxTrigger;
            if (internalProcessingPoolingInterval.HasValue)
            {
                // Use custom polling interval if provided
                processInboxTrigger =
					TriggerBuilder
                        .Create()
                        .StartNow()
                        .WithSimpleSchedule(x =>
                            x.WithInterval(TimeSpan.FromMilliseconds(internalProcessingPoolingInterval.Value))
                                .RepeatForever())
                        .Build();
            }
            else
            {
                // Use default cron schedule (every 2 seconds)
                processInboxTrigger =
					TriggerBuilder
                        .Create()
                        .StartNow()
                        .WithCronSchedule("0/2 * * ? * *")
                        .Build();
            }

            // Schedule the inbox processing job
            scheduler
                .ScheduleJob(processInboxJob, processInboxTrigger)
                .GetAwaiter().GetResult();

            // Create and schedule the internal commands processing job
            var processInternalCommandsJob = JobBuilder.Create<ProcessInternalCommandsJob>().Build();

            ITrigger processInternalCommandsTrigger;
            if (internalProcessingPoolingInterval.HasValue)
            {
                // Use custom polling interval if provided
                processInternalCommandsTrigger =
					TriggerBuilder
                        .Create()
                        .StartNow()
                        .WithSimpleSchedule(x =>
                            x.WithInterval(TimeSpan.FromMilliseconds(internalProcessingPoolingInterval.Value))
                                .RepeatForever())
                        .Build();
            }
            else
            {
                // Use default cron schedule (every 2 seconds)
                processInternalCommandsTrigger =
                    TriggerBuilder
                        .Create()
                        .StartNow()
                        .WithCronSchedule("0/2 * * ? * *")
                        .Build();
            }

            // Schedule the internal commands processing job
            scheduler.ScheduleJob(processInternalCommandsJob, processInternalCommandsTrigger).GetAwaiter().GetResult();

            logger.Information("Quartz started.");
			
		}
    }
}