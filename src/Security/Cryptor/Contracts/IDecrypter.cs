using System.Threading.Tasks;

namespace Security.Cryptor.Contracts
{
    public interface IDecrypter
    {
        Task<string> DecryptAsync(string data);
    }
}