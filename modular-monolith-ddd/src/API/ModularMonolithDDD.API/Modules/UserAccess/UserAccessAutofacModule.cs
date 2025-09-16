namespace ModularMonolithDDD.API.Modules.UserAccess
{
    public class UserAccessAutofacModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<UserAccessModule>()
                .As<IUserAccessModule>()
                .InstancePerLifetimeScope();
           
        }
    }
}