 
namespace Infrastructure.Security.Contracts
{
    public interface IDecrypter
    {
        Task<string> DecryptAsync(string data);
    }
}