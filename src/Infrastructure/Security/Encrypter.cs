 
using System.Security.Cryptography; 
using Infrastructure.Security.Contracts;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Security.Cryptor
{
    public class Encrypter : BaseCryptor, IEncrypter
    {
        private const CipherMode CipherMode = System.Security.Cryptography.CipherMode.ECB;

        public Encrypter(IConfiguration configuration) : base(configuration)
        {
        }

        public async Task<string> EncryptAsync(string data)
        {
            if (string.IsNullOrEmpty(data)) return string.Empty;

            using (Aes aes = Aes.Create())
            {
                aes.Key = _cryptoKey;
                aes.Mode = CipherMode;
                aes.BlockSize = 128;
                aes.Padding = Padding;

                aes.GenerateIV();

                byte[] encrypted;

                using (var memoryStream = new MemoryStream())
                {
                    using (var cryptoStream =
                        new CryptoStream(memoryStream, aes.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        using (var streamWriter = new StreamWriter(cryptoStream))
                        {
                            await streamWriter.WriteAsync(data);
                        }

                        encrypted = memoryStream.ToArray();
                    }
                }

                var combinedIVCT = new byte[aes.IV.Length + encrypted.Length];

                Array.Copy(aes.IV, 0, combinedIVCT, 0, aes.IV.Length);

                Array.Copy(encrypted, 0, combinedIVCT, aes.IV.Length, encrypted.Length);

                return Convert.ToBase64String(combinedIVCT);
            }
        }
    }
}