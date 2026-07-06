namespace ModularMonolithDDD.Modules.UserAccess.Infrastructure.Configuration.Quartz
{
    /// <summary>
    /// Autofac module for configuring Quartz.NET job dependencies in the UserAccess module.
    /// This module registers all job implementations that implement the IJob interface
    /// with per-dependency lifetime scope for proper job execution.
    /// </summary>
	public class QuartzModule : Module
	{
        /// <summary>
        /// Loads the Quartz job configuration into the Autofac container.
        /// Registers all job implementations from the current assembly that implement IJob.
        /// </summary>
        /// <param name="builder">The Autofac container builder.</param>
		protected override void Load(ContainerBuilder builder)
		{
			// Register all job implementations from the current assembly
			// Jobs are registered with per-dependency scope to ensure fresh instances for each execution
			builder.RegisterAssemblyTypes(ThisAssembly)
				.Where(x => typeof(IJob).IsAssignableFrom(x))
				.InstancePerDependency();
		}
	}
}