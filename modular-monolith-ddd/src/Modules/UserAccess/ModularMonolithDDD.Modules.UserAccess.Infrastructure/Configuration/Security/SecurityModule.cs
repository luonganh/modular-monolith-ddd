namespace ModularMonolithDDD.Modules.UserAccess.Infrastructure.Configuration.Security
{
    /// <summary>
    /// Autofac module that wires up security-related services for the UserAccess module.
    /// Registers <see cref="IDataProtector"/> with an AES-based implementation using the provided key.
    /// </summary>
    internal class SecurityModule : Module
    {
        private readonly string _encryptionKey;

        /// <summary>
        /// Initializes the module with the symmetric encryption key used by data protection services.
        /// </summary>
        /// <param name="encryptionKey">UTF-8 string used to derive the AES key bytes.</param>
        public SecurityModule(string encryptionKey)
        {
            _encryptionKey = encryptionKey;
        }

        /// <inheritdoc />
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<AesDataProtector>()
                .As<IDataProtector>()
                .WithParameter("encryptionKey", _encryptionKey);
        }
    }
}