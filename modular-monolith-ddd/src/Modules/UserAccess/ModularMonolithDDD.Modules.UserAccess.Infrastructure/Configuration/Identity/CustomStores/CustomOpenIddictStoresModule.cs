namespace ModularMonolithDDD.Modules.UserAccess.Infrastructure.Configuration.Identity.CustomStores
{
    /// <summary>
    /// Custom OpenIddict stores module configuration for the UserAccess module.
    /// </summary>
    internal class CustomOpenIddictStoresModule : Module
    {        
        /// <summary>
        /// Loads the custom OpenIddict stores module configuration into the Autofac container.
        /// Registers SQL connection factory and repository implementations.
        /// </summary>
        /// <param name="builder">The Autofac container builder.</param>
        protected override void Load(ContainerBuilder builder)
        {            
            // Custom OpenIddict stores (Dapper) – per lifetime scope
            builder.RegisterType<CustomApplicationStore>()
                .As<IOpenIddictApplicationStore<CustomApplication>>()
                .InstancePerLifetimeScope();

            builder.RegisterType<CustomScopeStore>()
                .As<IOpenIddictScopeStore<CustomScope>>()
                .InstancePerLifetimeScope();

            builder.RegisterType<CustomAuthorizationStore>()
                .As<IOpenIddictAuthorizationStore<CustomAuthorization>>()
                .InstancePerLifetimeScope();

            builder.RegisterType<CustomTokenStore>()
                .As<IOpenIddictTokenStore<CustomToken>>()
                .InstancePerLifetimeScope();

        }
    }
}
