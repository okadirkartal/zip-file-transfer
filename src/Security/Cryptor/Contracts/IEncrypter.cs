using System.Threading.Tasks;

namespace Security.Cryptor.Contracts
{
    public interface IEncrypter
    {
        Task<string> EncryptAsync(string data);
    }
}