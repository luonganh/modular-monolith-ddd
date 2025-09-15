namespace ModularMonolithDDD.Modules.UserAccess.Infrastructure.Configuration.Security
{
    /// <summary>
    /// Abstraction for symmetric data protection used within the UserAccess module.
    /// Implementations should provide reversible encryption suitable for short-lived secrets
    /// (e.g., tokens, sensitive configuration values) stored or transmitted by the module.
    /// </summary>
    public interface IDataProtector
    {
        /// <summary>
        /// Encrypts the provided plaintext and returns an encoded cipher string.
        /// </summary>
        /// <param name="plainText">The UTF-8 text to encrypt.</param>
        /// <returns>Base64-encoded ciphertext containing any necessary metadata (e.g., IV).</returns>
        string Encrypt(string plainText);

        /// <summary>
        /// Decrypts a previously <see cref="Encrypt(string)"/>-produced cipher string.
        /// </summary>
        /// <param name="encryptedText">The Base64-encoded ciphertext.</param>
        /// <returns>The decrypted UTF-8 plaintext.</returns>
        string Decrypt(string encryptedText);
    }
}