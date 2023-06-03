using System.Security.Cryptography;
using System.Text;
using Domain.Entities; 
using Microsoft.Extensions.Options;

namespace Infrastructure.Security;

public class BaseCryptor
{
    protected const PaddingMode Padding = PaddingMode.PKCS7;
    protected readonly byte[] CryptoKey;

    protected BaseCryptor(IOptions<ApplicationOptions> options)
    {
        CryptoKey = Encoding.UTF8.GetBytes(options.Value.Security.CryptoKey ?? "81T1A2L3L4I5NN18");
    }
}