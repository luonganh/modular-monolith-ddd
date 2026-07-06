namespace ModularMonolithDDD.API.Modules.UserAccess
{
    /// <summary>
    /// Autofac module for the UserAccess module.
    /// </summary>
    public class UserAccessAutofacModule : Autofac.Module
    {
        /// <summary>
        /// Loads the module and registers the UserAccessModule.
        /// </summary>
        /// <param name="builder"></param>
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<UserAccessModule>()
                .As<IUserAccessModule>()
                .InstancePerLifetimeScope();
           
        }
    }
}