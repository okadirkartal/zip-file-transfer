namespace Infrastructure.Security.Contracts;

public interface IEncrypter
{
    Task<string> EncryptAsync(string data);
}