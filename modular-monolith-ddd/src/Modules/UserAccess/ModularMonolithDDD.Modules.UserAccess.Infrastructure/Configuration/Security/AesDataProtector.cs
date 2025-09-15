namespace ModularMonolithDDD.Modules.UserAccess.Infrastructure.Configuration.Security
{
    /// <summary>
    /// AES-based implementation of <see cref="IDataProtector"/>.
    /// Uses a caller-provided symmetric key and generates a random IV per encryption,
    /// prefixing it to the ciphertext for later decryption.
    /// </summary>
    /// <remarks>
    /// The provided key must be appropriate for the chosen AES algorithm key size.
    /// This class does not manage key storage or rotation.
    /// </remarks>
    public class AesDataProtector : IDataProtector
    {
        private readonly string _encryptionKey;

        /// <summary>
        /// Creates a new AES data protector with the provided symmetric key.
        /// </summary>
        /// <param name="encryptionKey">UTF-8 string used to derive the AES key bytes.</param>
        public AesDataProtector(string encryptionKey)
        {
            _encryptionKey = encryptionKey;
        }

        /// <inheritdoc />
        public string Encrypt(string plainText)
        {
            var key = Encoding.UTF8.GetBytes(_encryptionKey);

            using var aesAlg = Aes.Create();
            using var encryptor = aesAlg.CreateEncryptor(key, aesAlg.IV);
            using var msEncrypt = new MemoryStream();
            using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
            {
                using var swEncrypt = new StreamWriter(csEncrypt);
                swEncrypt.Write(plainText);
            }

            var iv = aesAlg.IV;

            var encryptedContent = msEncrypt.ToArray();

            var result = new byte[iv.Length + encryptedContent.Length];

            Buffer.BlockCopy(iv, 0, result, 0, iv.Length);
            Buffer.BlockCopy(encryptedContent, 0, result, iv.Length, encryptedContent.Length);

            return Convert.ToBase64String(result);
        }

        /// <inheritdoc />
        public string Decrypt(string encryptedText)
        {
            var fullCipher = Convert.FromBase64String(encryptedText);

            var iv = new byte[16];
            var cipher = new byte[fullCipher.Length - iv.Length];

            Buffer.BlockCopy(fullCipher, 0, iv, 0, iv.Length);
            Buffer.BlockCopy(fullCipher, iv.Length, cipher, 0, fullCipher.Length - iv.Length);
            var key = Encoding.UTF8.GetBytes(_encryptionKey);

            using var aesAlg = Aes.Create();
            using var decryptor = aesAlg.CreateDecryptor(key, iv);
            string result;
            using (var msDecrypt = new MemoryStream(cipher))
            {
                using var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read);
                using var srDecrypt = new StreamReader(csDecrypt);
                result = srDecrypt.ReadToEnd();
            }

            return result;
        }
    }
}