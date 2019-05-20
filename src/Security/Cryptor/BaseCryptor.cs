using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Configuration;

namespace Security.Cryptor
{
    public class BaseCryptor
    {
        protected readonly byte[] _cryptoKey;

        protected const PaddingMode Padding = PaddingMode.PKCS7;

        protected BaseCryptor(IConfiguration configuration)
        {
            _cryptoKey = Encoding.UTF8.GetBytes(configuration[Core.Common.Constants.CryptoKey] ?? "81T1A2L3L4I5NN18");
        }
    }
}