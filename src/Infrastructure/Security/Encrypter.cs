using System.Security.Cryptography;
using Domain.Entities;
using Infrastructure.Security.Contracts; 
using Microsoft.Extensions.Options;

namespace Infrastructure.Security.Cryptor;

public sealed class Encrypter : BaseCryptor, IEncrypter
{
    private const CipherMode CipherMode = System.Security.Cryptography.CipherMode.ECB;

    public Encrypter(IOptions<ApplicationOptions> options) : base(options)
    {
    }

    public async Task<string> EncryptAsync(string data)
    {
        if (string.IsNullOrEmpty(data)) return string.Empty;

        using var aes = Aes.Create();
        aes.Key = CryptoKey;
        aes.Mode = CipherMode;
        aes.BlockSize = 128;
        aes.Padding = Padding;

        aes.GenerateIV();

        byte[] encrypted;

        using (var memoryStream = new MemoryStream())
        {
            await using (var cryptoStream =
                         new CryptoStream(memoryStream, aes.CreateEncryptor(), CryptoStreamMode.Write))
            {
                await using (var streamWriter = new StreamWriter(cryptoStream))
                {
                    await streamWriter.WriteAsync(data);
                }

                encrypted = memoryStream.ToArray();
            }
        }

        var combinedIvct = new byte[aes.IV.Length + encrypted.Length];

        Array.Copy(aes.IV, 0, combinedIvct, 0, aes.IV.Length);

        Array.Copy(encrypted, 0, combinedIvct, aes.IV.Length, encrypted.Length);

        return Convert.ToBase64String(combinedIvct);
    }
}