 
using System.Security.Cryptography; 
using Infrastructure.Security.Contracts;
using Microsoft.Extensions.Configuration;

namespace  Infrastructure.Security
{
    public class Decrypter : BaseCryptor, IDecrypter
    {
        public Decrypter(IConfiguration configuration) : base(configuration)
        {
        }

        public async Task<string> DecryptAsync(string data)
        {
            if (string.IsNullOrEmpty(data)) return string.Empty;

            byte[] encryptedData = Convert.FromBase64String(data);

            using (Aes aes = Aes.Create())
            {
                aes.Key = _cryptoKey;

                aes.Mode = CipherMode.ECB;

                aes.BlockSize = 128;

                aes.Padding = Padding;

                byte[] IV = new byte[aes.BlockSize / 8];

                byte[] cipherText = new byte[encryptedData.Length - IV.Length];

                Array.Copy(encryptedData, IV, IV.Length);

                Array.Copy(encryptedData, IV.Length, cipherText, 0, cipherText.Length);

                aes.IV = IV;

                ICryptoTransform decrypt = aes.CreateDecryptor(aes.Key, aes.IV);

                string decryptableText = null;
                using (var memoryStream = new MemoryStream(cipherText))
                {
                    await using (var cryptoStream = new CryptoStream(memoryStream, decrypt, CryptoStreamMode.Read))
                    {
                        using (var streamReader = new StreamReader(cryptoStream))
                        {
                            decryptableText = await streamReader.ReadToEndAsync();
                        }
                    }
                }

                return decryptableText;
            }
        }
    }
}